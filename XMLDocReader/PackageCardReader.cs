using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XMLDocReader
{
    public static class PackageCardReader
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static List<PackageCard> Read(List<PackageCardReaderSettings> settingsList)
        {
            var packageCardsList = new List<PackageCard>();

            foreach (var settings in settingsList)
            {
                var packageCard = Read(settings);
                packageCardsList.Add(packageCard);
            }

            return packageCardsList;
        }

        public static PackageCard Read(PackageCardReaderSettings settings)
        {
            var packageCard = new PackageCard();

            var targetAssembly = Assembly.LoadFrom(settings.AssemblyFileName);
            packageCard.AssemblyName = targetAssembly.GetName();

            var typesDict = new Dictionary<string, Type>();

            foreach (var type in targetAssembly.GetTypes())
            {
                typesDict[type.FullName] = type;
            }

            var memberCardsList = XMLMemberCardsReader.Read(settings.XMLDocFileName);

            var cardsOfTypesList = new List<XMLMemberCard>();
            var cardsOfMembersList = new List<XMLMemberCard>();

            foreach (var memberCard in memberCardsList)
            {
                if (memberCard.Name.Kind == KindOfMember.Type)
                {
                    cardsOfTypesList.Add(memberCard);
                }
                else
                {
                    cardsOfMembersList.Add(memberCard);
                }
            }

            var cardsOfMembersDict = cardsOfMembersList.GroupBy(p => p.Name.Path).ToDictionary(p => p.Key, p => p.ToList());

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach (var typeCard in cardsOfTypesList)
            {
                typeCard.IsBuiltTypeOrMemberCard = true;

                var fullName = typeCard.Name.FullName;

                var type = typesDict[fullName];

                var kindOfType = GetKindOfType(type);

                List<XMLMemberCard> membersList = null;

                if (cardsOfMembersDict.ContainsKey(fullName))
                {
                    membersList = cardsOfMembersDict[fullName];
                }
                else
                {
                    membersList = new List<XMLMemberCard>();
                }

                switch (kindOfType)
                {
                    case KindOfType.Class:
                        classesList.Add(ProcessClassOrInterface(typeCard, type, membersList, kindOfType));
                        break;

                    case KindOfType.Interface:
                        interfacesList.Add(ProcessClassOrInterface(typeCard, type, membersList, kindOfType));
                        break;

                    case KindOfType.Enum:
                        enumsList.Add(ProcessEnum(typeCard, type, membersList));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfType), kindOfType, null);
                }
            }

            var fullNamesList = classesList.Select(p => p.Name.Path).Concat(interfacesList.Select(p => p.Name.Path)).Concat(enumsList.Select(p => p.Name.Path)).Distinct().ToList();

            var classesDict = classesList.ToDictionary(p => p.Name.FullName, p => p);
            var interfacesDict = interfacesList.ToDictionary(p => p.Name.FullName, p => p);

            var namespacesList = new List<NamespaceCard>();
            var namespacesDict = new Dictionary<string, NamespaceCard>();

            FillUpNamespacesList(1, packageCard, namespacesList, namespacesDict, classesDict, interfacesDict, fullNamesList);

            packageCard.NamespacesList = namespacesList;

            foreach (var classCard in classesList)
            {
                classCard.Package = packageCard;

                var path = classCard.Name.Path;

                if (namespacesDict.ContainsKey(path))
                {
                    var namespaceCard = namespacesDict[path];
                    namespaceCard.ClassesList.Add(classCard);
                    classCard.Parent = namespaceCard;
                }
                else
                {
                    if (classesDict.ContainsKey(path))
                    {
                        var parentClassCard = classesDict[path];
                        parentClassCard.ClassesList.Add(classCard);
                        classCard.Parent = parentClassCard;
                    }
                    else
                    {
                        if (interfacesDict.ContainsKey(path))
                        {
                            var parentInterfaceCard = interfacesDict[path];
                            parentInterfaceCard.ClassesList.Add(classCard);
                            classCard.Parent = parentInterfaceCard;
                        }
                    }
                }
            }

            packageCard.ClassesList = classesList;

            foreach (var interfaceCard in interfacesList)
            {
                interfaceCard.Package = packageCard;

                var path = interfaceCard.Name.Path;

                if (namespacesDict.ContainsKey(path))
                {
                    var namespaceCard = namespacesDict[path];
                    namespaceCard.InterfacesList.Add(interfaceCard);
                    interfaceCard.Parent = namespaceCard;
                }
                else
                {
                    if (classesDict.ContainsKey(path))
                    {
                        var parentClassCard = classesDict[path];
                        parentClassCard.InterfacesList.Add(interfaceCard);
                        interfaceCard.Parent = parentClassCard;
                    }
                    else
                    {
                        if (interfacesDict.ContainsKey(path))
                        {
                            var parentInterfaceCard = interfacesDict[path];
                            parentInterfaceCard.InterfacesList.Add(interfaceCard);
                            interfaceCard.Parent = parentInterfaceCard;
                        }
                    }
                }
            }

            packageCard.InterfacesList = interfacesList;

            foreach (var enumCard in enumsList)
            {
                enumCard.Package = packageCard;

                var path = enumCard.Name.Path;

                if (namespacesDict.ContainsKey(path))
                {
                    var namespaceCard = namespacesDict[path];
                    namespaceCard.EnumsList.Add(enumCard);
                    enumCard.Parent = namespaceCard;
                }
                else
                {
                    if (classesDict.ContainsKey(path))
                    {
                        var parentClassCard = classesDict[path];
                        parentClassCard.EnumsList.Add(enumCard);
                        enumCard.Parent = parentClassCard;
                    }
                    else
                    {
                        if (interfacesDict.ContainsKey(path))
                        {
                            var parentInterfaceCard = interfacesDict[path];
                            parentInterfaceCard.EnumsList.Add(enumCard);
                            enumCard.Parent = parentInterfaceCard;
                        }
                    }
                }
            }

            packageCard.EnumsList = enumsList;

            packageCard.XMLCardsWithoutTypeList = cardsOfMembersList.Where(p => !p.IsBuiltTypeOrMemberCard).ToList();

            return packageCard;
        }

        private static void FillUpNamespacesList(int n, PackageCard packageCard, List<NamespaceCard> namespacesList, Dictionary<string, NamespaceCard> namespacesDict, Dictionary<string, ClassCard> classesDict, Dictionary<string, ClassCard> interfacesDict, List<string> fullNamesList)
        {
            foreach (var fullName in fullNamesList)
            {
                if (classesDict.ContainsKey(fullName))
                {
                    continue;
                }

                if (interfacesDict.ContainsKey(fullName))
                {
                    continue;
                }

                FillUpNamespace(n, packageCard, null, namespacesList, namespacesDict, fullName);
            }
        }

        private static void FillUpNamespace(int n, PackageCard packageCard, NamespaceCard parentNamespaceCard, List<NamespaceCard> namespacesList, Dictionary<string, NamespaceCard> namespacesDict, string fullName)
        {
            string targetFullName;
            string targetName;
            string path;
            var needNextProcess = false;

            var dotPos = GetDotPos(fullName, n);

            if (dotPos == -1)
            {
                targetFullName = fullName;

                if (targetFullName.IndexOf(".") == -1)
                {
                    targetName = fullName;
                    path = string.Empty;
                }
                else
                {
                    dotPos = GetDotPos(targetFullName, n - 1);
                    path = targetFullName.Substring(0, dotPos);

                    targetName = targetFullName.Substring(dotPos + 1);
                }
            }
            else
            {
                targetFullName = fullName.Substring(0, dotPos);

                if (targetFullName.IndexOf(".") == -1)
                {
                    targetName = targetFullName;
                    path = string.Empty;
                }
                else
                {
                    dotPos = GetDotPos(targetFullName, n - 1);
                    path = targetFullName.Substring(0, dotPos);
                    targetName = targetFullName.Substring(dotPos + 1);
                }

                if (fullName != targetFullName)
                {
                    needNextProcess = true;
                }
            }

            NamespaceCard namespaceCard;

            if (namespacesDict.ContainsKey(targetFullName))
            {
                namespaceCard = namespacesDict[targetFullName];
            }
            else
            {
                var namespaceName = new MemberName();
                namespaceName.Kind = KindOfMember.Namespace;
                namespaceName.InitialName = targetFullName;
                namespaceName.FullName = targetFullName;
                namespaceName.Name = targetName;
                namespaceName.Path = path;

                namespaceCard = new NamespaceCard();
                namespaceCard.Package = packageCard;
                namespaceCard.Name = namespaceName;

                if (parentNamespaceCard != null)
                {
                    namespaceCard.Parent = parentNamespaceCard;
                    parentNamespaceCard.NamespacesList.Add(namespaceCard);
                }

                namespacesDict[targetFullName] = namespaceCard;
                namespacesList.Add(namespaceCard);
            }

            if (needNextProcess)
            {
                FillUpNamespace(n + 1, packageCard, namespaceCard, namespacesList, namespacesDict, fullName);
            }
        }

        private static int GetDotPos(string inputStr, int n)
        {
            var count = 0;
            var i = 0;

            foreach (var ch in inputStr)
            {
                if (ch == '.')
                {
                    count++;
                }

                if (count == n)
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        private static EnumCard ProcessEnum(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList)
        {
            var result = new EnumCard();
            result.Type = type;
            result.IsPublic = type.IsPublic;

            FillUpNamedElementCard(typeCard, result);

            foreach (var memberCard in membersList)
            {
                switch (memberCard.Name.Kind)
                {
                    case KindOfMember.Field:
                        {
                            var field = ProcessField(memberCard, type);
                            field.Parent = result;
                            result.FieldsList.Add(field);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(memberCard.Name.Kind), memberCard.Name.Kind, null);
                }
            }

            return result;
        }

        private static EnumFieldCard ProcessField(XMLMemberCard memberCard, Type parentType)
        {
            var result = new EnumFieldCard();

            FillUpNamedElementCard(memberCard, result);

            result.FieldInfo = parentType.GetField(memberCard.Name.Name);
            result.KindOfMemberAccess = GetKindOfMemberAccess(result.FieldInfo);

            return result;
        }

        private static ClassCard ProcessClassOrInterface(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList, KindOfType kindOfType)
        {
            var result = new ClassCard();
            result.KindOfType = kindOfType;
            result.Type = type;
            result.IsPublic = type.IsPublic;

            FillUpNamedElementCard(typeCard, result);

            foreach (var memberCard in membersList)
            {
                switch (memberCard.Name.Kind)
                {
                    case KindOfMember.Property:
                        {
                            var property = ProcessProperty(memberCard, type);
                            property.Parent = result;
                            result.PropertiesList.Add(property);
                        }
                        break;

                    case KindOfMember.Method:
                        {
                            var method = ProcessMethod(memberCard, type);
                            method.Parent = result;
                            result.MethodsList.Add(method);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(memberCard.Name.Kind), memberCard.Name.Kind, null);
                }
            }

            return result;
        }

        private static MethodCard ProcessMethod(XMLMemberCard memberCard, Type parentType)
        {
            var result = new MethodCard();

            FillUpNamedElementCard(memberCard, result);

            var name = memberCard.Name.Name;

            if (!string.IsNullOrWhiteSpace(memberCard.Name.ImplInterfaceName))
            {
                name = $"{memberCard.Name.ImplInterfaceName}.{name}";
            }

            var methodsList = parentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => p.Name == name).ToList();

            var methodsCount = methodsList.Count;

            switch (methodsCount)
            {
                case 0:
                    throw new NotImplementedException();

                case 1:
                    result.MethodInfo = methodsList.Single();
                    break;

                default:
                    if (memberCard.IsInclude || memberCard.IsInheritdoc)
                    {
                        result.MethodInfo = GetMethodInfoByTypeNames(methodsList, memberCard);
                    }
                    else
                    {
                        result.MethodInfo = GetMethodInfo(methodsList, memberCard);
                    }
                    break;
            }

            result.KindOfMemberAccess = GetKindOfMemberAccess(result.MethodInfo);

            result.Returns = memberCard.Returns;

            if (memberCard.ParamsList.Any())
            {
                var methodReflectionParamsList = result.MethodInfo.GetParameters();

                var i = 0;

                foreach (var methodParamCard in memberCard.ParamsList)
                {
                    var parameter = new MethodParamCard();
                    parameter.Name = methodParamCard.Name;
                    parameter.ParameterInfo = methodReflectionParamsList[i];
                    parameter.Summary = methodParamCard.Value;
                    parameter.XMLParamCard = methodParamCard;

                    result.ParamsList.Add(parameter);

                    i++;
                }
            }

            if (memberCard.TypeParamsList.Any())
            {
                throw new NotImplementedException();
            }

            if (memberCard.ExceptionsList.Any())
            {
                throw new NotImplementedException();
            }

            return result;
        }

        private static MethodInfo GetMethodInfoByTypeNames(List<MethodInfo> methodsList, XMLMemberCard memberCard)
        {
            var xmlParamsList = memberCard.Name.ParametersList;

            var xmlParamsCount = xmlParamsList.Count;

            foreach (var method in methodsList)
            {
                var paramsList = method.GetParameters();

                if (paramsList.Length != xmlParamsCount)
                {
                    continue;
                }

                var xmlParamEnumerator = xmlParamsList.GetEnumerator();

                var isFit = true;

                foreach (var param in paramsList)
                {
                    xmlParamEnumerator.MoveNext();

                    var currentXMLParam = xmlParamEnumerator.Current;

                    if (NamesHelper.SimplifyFullNameOfType(param.ParameterType.FullName) != currentXMLParam)
                    {
                        isFit = false;
                        break;
                    }
                }

                if (isFit)
                {
                    return method;
                }
            }

            throw new NotImplementedException();
        }

        private static MethodInfo GetMethodInfo(List<MethodInfo> methodsList, XMLMemberCard memberCard)
        {
            var xmlParamsList = memberCard.ParamsList;

            var xmlParamsCount = xmlParamsList.Count;

            foreach (var method in methodsList)
            {
                var paramsList = method.GetParameters();

                if (paramsList.Length != xmlParamsCount)
                {
                    continue;
                }

                var xmlParamEnumerator = xmlParamsList.GetEnumerator();

                var isFit = true;

                foreach (var param in paramsList)
                {
                    xmlParamEnumerator.MoveNext();

                    var currentXMLParam = xmlParamEnumerator.Current;

                    if (param.Name != currentXMLParam.Name)
                    {
                        isFit = false;
                        break;
                    }
                }

                if (isFit)
                {
                    return method;
                }
            }

            throw new NotImplementedException();
        }

        private static KindOfMemberAccess GetKindOfMemberAccess(FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic)
            {
                return KindOfMemberAccess.Public;
            }

            if (fieldInfo.IsFamily)
            {
                return KindOfMemberAccess.Protected;
            }

            return KindOfMemberAccess.Private;
        }

        private static KindOfMemberAccess GetKindOfMemberAccess(MethodInfo methodInfo)
        {
            if (methodInfo.IsPublic)
            {
                return KindOfMemberAccess.Public;
            }

            if (methodInfo.IsFamily)
            {
                return KindOfMemberAccess.Protected;
            }

            return KindOfMemberAccess.Private;
        }

        private static PropertyCard ProcessProperty(XMLMemberCard memberCard, Type parentType)
        {
            var result = new PropertyCard();

            FillUpNamedElementCard(memberCard, result);

            var property = parentType.GetProperty(memberCard.Name.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (property == null)
            {
                property = parentType.GetProperty(memberCard.Name.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (property == null)
                {
                    throw new NotImplementedException();
                }

                result.KindOfMemberAccess = KindOfMemberAccess.Protected;
            }
            else
            {
                result.KindOfMemberAccess = KindOfMemberAccess.Public;
            }

            result.PropertyInfo = property;
            result.Value = memberCard.Value;

            return result;
        }

        private static void FillUpNamedElementCard(XMLMemberCard source, NamedElementCard dest)
        {
            dest.Name = source.Name;
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList;
            dest.XMLMemberCard = source;
            source.IsBuiltTypeOrMemberCard = true;
        }

        private static Type _delegateType = typeof(Delegate);

        private static KindOfType GetKindOfType(Type type)
        {
            if (type.IsClass)
            {
                if (_delegateType.IsAssignableFrom(type))
                {
                    return KindOfType.Delegate;
                }
                return KindOfType.Class;
            }

            if (type.IsInterface)
            {
                return KindOfType.Interface;
            }

            if (type.IsEnum)
            {
                return KindOfType.Enum;
            }

            throw new NotImplementedException();
        }
    }
}
