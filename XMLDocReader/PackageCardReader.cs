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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static PackageCard Read(PackageCardReaderSettings settings)
        {
            _logger.Info($"settings = {settings}");

            var packageCard = new PackageCard();

            var targetAssembly = Assembly.LoadFrom(settings.AssemblyFileName);

            //_logger.Info($"AppDomain.CurrentDomain.GetAssemblies() = {JsonConvert.SerializeObject(AppDomain.CurrentDomain.GetAssemblies().Select(p => p.GetName().Name), Formatting.Indented)}");

            packageCard.AssemblyName = targetAssembly.GetName();

            var typesDict = new Dictionary<string, Type>();

            foreach (var type in targetAssembly.GetTypes())
            {
                //_logger.Info($"type.FullName = {type.FullName}");

                typesDict[type.FullName] = type;
            }

            var memberCardsList = XMLMemberCardsReader.Read(settings.XMLDocFileName);

            _logger.Info($"memberCardsList.Count = {memberCardsList.Count}");

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

            _logger.Info($"cardsOfTypesList.Count = {cardsOfTypesList.Count}");
            _logger.Info($"cardsOfMembersList.Count = {cardsOfMembersList.Count}");

            var cardsOfMembersDict = cardsOfMembersList.GroupBy(p => p.Name.Path).ToDictionary(p => p.Key, p => p.ToList());

            _logger.Info($"cardsOfMembersDict.Count = {cardsOfMembersDict.Count}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach (var typeCard in cardsOfTypesList)
            {
                _logger.Info($"typeCard = {typeCard}");

                typeCard.IsProcessed = true;

                var fullName = typeCard.Name.FullName;

                var type = typesDict[fullName];

                //_logger.Info($"type.IsPublic = {type.IsPublic}");
                //_logger.Info($"type.IsNotPublic = {type.IsNotPublic}");

                var kindOfType = GetKindOfType(type);

                _logger.Info($"kindOfType = {kindOfType}");

                //continue;

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

                //_logger.Info($" = {}");
            }

            _logger.Info($"classesList.Count = {classesList.Count}");
            _logger.Info($"interfacesList.Count = {interfacesList.Count}");
            _logger.Info($"enumsList.Count = {enumsList.Count}");

            var fullNamesList = classesList.Select(p => p.Name.Path).Concat(interfacesList.Select(p => p.Name.Path)).Concat(enumsList.Select(p => p.Name.Path)).Distinct().ToList();

            _logger.Info($"fullNamesList = {JsonConvert.SerializeObject(fullNamesList, Formatting.Indented)}");

            var classesDict = classesList.ToDictionary(p => p.Name.FullName, p => p);
            var interfacesDict = interfacesList.ToDictionary(p => p.Name.FullName, p => p);

            var namespacesList = new List<NamespaceCard>();
            var namespacesDict = new Dictionary<string, NamespaceCard>();

            FillUpNamespacesList(1, packageCard, namespacesList, namespacesDict, classesDict, interfacesDict, fullNamesList);

            _logger.Info($"namespacesList.Count = {namespacesList.Count}");

            packageCard.NamespacesList = namespacesList;

            foreach (var classCard in classesList)
            {
                _logger.Info($"classCard = {classCard}");

                classCard.Package = packageCard;

                var path = classCard.Name.Path;

                _logger.Info($"path = '{path}'");

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
                _logger.Info($"interfaceCard = {interfaceCard}");

                interfaceCard.Package = packageCard;

                var path = interfaceCard.Name.Path;

                _logger.Info($"path = '{path}'");

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
                _logger.Info($"interfaceCard = {enumCard}");

                enumCard.Package = packageCard;

                var path = enumCard.Name.Path;

                _logger.Info($"path = '{path}'");

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

            var notProcessedCardsOfMembersList = cardsOfMembersList.Where(p => !p.IsProcessed).ToList();

            _logger.Info($"notProcessedCardsOfMembersList.Count = {notProcessedCardsOfMembersList.Count}");

            packageCard.XMLCardsWithoutTypeList = notProcessedCardsOfMembersList;

            return packageCard;
        }

        private static void FillUpNamespacesList(int n, PackageCard packageCard, List<NamespaceCard> namespacesList, Dictionary<string, NamespaceCard> namespacesDict, Dictionary<string, ClassCard> classesDict, Dictionary<string, ClassCard> interfacesDict, List<string> fullNamesList)
        {
            //_logger.Info($"n = {n}");

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
            //_logger.Info($"n = {n}");
            //_logger.Info($"fullName = '{fullName}'");

            string targetFullName;
            string targetName;
            string path;
            var needNextProcess = false;

            var dotPos = GetDotPos(fullName, n);

            //_logger.Info($"dotPos = {dotPos}");

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

                    //_logger.Info($"dotPos (2) = {dotPos}");

                    path = targetFullName.Substring(0, dotPos);

                    //_logger.Info($"path = '{path}'");

                    targetName = targetFullName.Substring(dotPos + 1);

                    //_logger.Info($"targetName = '{targetName}'");
                }
            }
            else
            {
                targetFullName = fullName.Substring(0, dotPos);

                //_logger.Info($"targetFullName = '{targetFullName}'");

                if (targetFullName.IndexOf(".") == -1)
                {
                    targetName = targetFullName;
                    path = string.Empty;
                }
                else
                {
                    dotPos = GetDotPos(targetFullName, n - 1);

                    //_logger.Info($"dotPos (2) = {dotPos}");

                    path = targetFullName.Substring(0, dotPos);

                    //_logger.Info($"path = '{path}'");

                    targetName = targetFullName.Substring(dotPos + 1);

                    //_logger.Info($"targetName = '{targetName}'");
                }

                if (fullName != targetFullName)
                {
                    needNextProcess = true;
                }
            }

            //_logger.Info($"targetFullName = '{targetFullName}'");
            //_logger.Info($"targetName = '{targetName}'");
            //_logger.Info($"path = '{path}'");

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

                //_logger.Info($"namespaceName = {namespaceName}");

                namespaceCard = new NamespaceCard();
                namespaceCard.Package = packageCard;
                namespaceCard.Name = namespaceName;

                if (parentNamespaceCard != null)
                {
                    namespaceCard.Parent = parentNamespaceCard;
                    parentNamespaceCard.NamespacesList.Add(namespaceCard);
                }

                //_logger.Info($"namespaceCard = {namespaceCard}");

                namespacesDict[targetFullName] = namespaceCard;
                namespacesList.Add(namespaceCard);
            }

            //_logger.Info($"needNextProcess = {needNextProcess}");

            //if(targetFullName == "SymOntoClay.UnityAsset.Core.InternalImplementations.Player")
            //{
            //    throw new NotImplementedException();
            //}

            if (needNextProcess)
            {
                FillUpNamespace(n + 1, packageCard, namespaceCard, namespacesList, namespacesDict, fullName);
            }
        }

        private static int GetDotPos(string inputStr, int n)
        {
            //_logger.Info($"inputStr = '{inputStr}'");
            //_logger.Info($"n = {n}");

            var count = 0;
            var i = 0;

            foreach (var ch in inputStr)
            {
                //_logger.Info($"ch = '{ch}'");

                if (ch == '.')
                {
                    count++;
                }

                //_logger.Info($"count = {count}");
                //_logger.Info($"i = {i}");

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

            _logger.Info($"membersList.Count = {membersList.Count}");

            foreach (var memberCard in membersList)
            {
                _logger.Info($"memberCard = {memberCard}");

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

            _logger.Info($"result = {result}");

            return result;
        }

        private static EnumFieldCard ProcessField(XMLMemberCard memberCard, Type parentType)
        {
            var result = new EnumFieldCard();

            FillUpNamedElementCard(memberCard, result);

            result.FieldInfo = parentType.GetField(memberCard.Name.Name);

            result.KindOfMemberAccess = GetKindOfMemberAccess(result.FieldInfo);

            _logger.Info($"result = {result}");

            return result;
        }

        private static ClassCard ProcessClassOrInterface(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList, KindOfType kindOfType)
        {
            var result = new ClassCard();
            result.KindOfType = kindOfType;
            result.Type = type;
            result.IsPublic = type.IsPublic;

            FillUpNamedElementCard(typeCard, result);

            _logger.Info($"membersList.Count = {membersList.Count}");

            foreach (var memberCard in membersList)
            {
                _logger.Info($"memberCard = {memberCard}");

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

            _logger.Info($"result = {result}");

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

            _logger.Info($"name = {name}");

            var methodsList = parentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => p.Name == name).ToList();

            var methodsCount = methodsList.Count;

            _logger.Info($"methodsCount = {methodsCount}");

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

                    _logger.Info($"parameter.ParameterInfo.Name = {parameter.ParameterInfo.Name}");
                    _logger.Info($"parameter = {parameter}");

                    result.ParamsList.Add(parameter);

                    i++;
                }

                _logger.Info($"result = {result}");
            }

            if (memberCard.TypeParamsList.Any())
            {
                throw new NotImplementedException();
            }

            if (memberCard.ExceptionsList.Any())
            {
                throw new NotImplementedException();
            }

            _logger.Info($"result = {result}");

            return result;
        }

        private static MethodInfo GetMethodInfoByTypeNames(List<MethodInfo> methodsList, XMLMemberCard memberCard)
        {
            _logger.Info($"methodsList.Count = {methodsList.Count}");

            var xmlParamsList = memberCard.Name.ParametersList;

            var xmlParamsCount = xmlParamsList.Count;

            _logger.Info($"xmlParamsCount = {xmlParamsCount}");

            foreach (var method in methodsList)
            {
                var paramsList = method.GetParameters();

                _logger.Info($"paramsList.Length = {paramsList.Length}");

                if (paramsList.Length != xmlParamsCount)
                {
                    continue;
                }

                var xmlParamEnumerator = xmlParamsList.GetEnumerator();

                var isFit = true;

                foreach (var param in paramsList)
                {
                    _logger.Info($"param.Name = {param.Name}");
                    _logger.Info($"param.ParameterType.Name = '{param.ParameterType.Name}'");
                    _logger.Info($"param.ParameterType.FullName = '{param.ParameterType.FullName}'");
                    _logger.Info($"SimplifyFullNameOfType(param.ParameterType.FullName) = '{NamesHelper.SimplifyFullNameOfType(param.ParameterType.FullName)}'");

                    xmlParamEnumerator.MoveNext();

                    var currentXMLParam = xmlParamEnumerator.Current;

                    _logger.Info($"currentXMLParam = '{currentXMLParam}'");

                    if (NamesHelper.SimplifyFullNameOfType(param.ParameterType.FullName) != currentXMLParam)
                    {
                        isFit = false;
                        break;
                    }
                }

                _logger.Info($"isFit = {isFit}");

                if (isFit)
                {
                    return method;
                }
            }

            throw new NotImplementedException();
        }

        private static MethodInfo GetMethodInfo(List<MethodInfo> methodsList, XMLMemberCard memberCard)
        {
            _logger.Info($"methodsList.Count = {methodsList.Count}");

            var xmlParamsList = memberCard.ParamsList;

            var xmlParamsCount = xmlParamsList.Count;

            _logger.Info($"xmlParamsCount = {xmlParamsCount}");

            foreach (var method in methodsList)
            {
                var paramsList = method.GetParameters();

                _logger.Info($"paramsList.Length = {paramsList.Length}");

                if (paramsList.Length != xmlParamsCount)
                {
                    continue;
                }

                var xmlParamEnumerator = xmlParamsList.GetEnumerator();

                var isFit = true;

                foreach (var param in paramsList)
                {
                    _logger.Info($"param.Name = {param.Name}");
                    _logger.Info($"param.ParameterType = {param.ParameterType}");

                    xmlParamEnumerator.MoveNext();

                    var currentXMLParam = xmlParamEnumerator.Current;

                    _logger.Info($"currentXMLParam = {currentXMLParam}");

                    if (param.Name != currentXMLParam.Name)
                    {
                        isFit = false;
                        break;
                    }
                }

                _logger.Info($"isFit = {isFit}");

                if (isFit)
                {
                    return method;
                }
            }

            throw new NotImplementedException();
        }

        private static KindOfMemberAccess GetKindOfMemberAccess(FieldInfo fieldInfo)
        {
            _logger.Info($"fieldInfo.IsPublic = {fieldInfo.IsPublic}");
            _logger.Info($"fieldInfo.IsPublic = {fieldInfo.IsFamily}");

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
            _logger.Info($"methodInfo.IsPublic = {methodInfo.IsPublic}");
            _logger.Info($"methodInfo.IsPublic = {methodInfo.IsFamily}");

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

            _logger.Info($"result = {result}");

            return result;
        }

        private static void FillUpNamedElementCard(XMLMemberCard source, NamedElementCard dest)
        {
            dest.Name = source.Name;
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList;
            dest.XMLMemberCard = source;
            source.IsProcessed = true;
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
