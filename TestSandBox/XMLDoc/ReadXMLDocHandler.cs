using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using XMLDocReader;

namespace TestSandBox.XMLDoc
{
    public class ReadXMLDocHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //var path = Environment.GetEnvironmentVariable("PATH");

            //_logger.Info($"path = {path}");

            //Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + Directory.GetCurrentDirectory());

            var targetXMLDocFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml");

            _logger.Info($"targetXMLDocFileName = {targetXMLDocFileName}");

            var typesDict = new Dictionary<string, Type>();

            _logger.Info($"typesDict.GetType().FullName = {typesDict.GetType().FullName}");

            var coreHelperAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClayCoreHelper.dll");

            _logger.Info($"coreHelperAssemblyFileName = {coreHelperAssemblyFileName}");

            var coreHelperAssembly = Assembly.LoadFrom(coreHelperAssemblyFileName);

            foreach (var type in coreHelperAssembly.GetTypes())
            {
                //_logger.Info($"type.FullName = {type.FullName}");

                typesDict[type.FullName] = type;
            }

            var coreAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.Core.dll");

            _logger.Info($"coreAssemblyFileName = {coreAssemblyFileName}");

            var coreAssembly = Assembly.LoadFrom(coreAssemblyFileName);

            var targetAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.dll");

            var targetAssembly = Assembly.LoadFrom(targetAssemblyFileName);

            //_logger.Info($"AppDomain.CurrentDomain.GetAssemblies() = {JsonConvert.SerializeObject(AppDomain.CurrentDomain.GetAssemblies().Select(p => p.GetName().Name), Formatting.Indented)}");

            foreach (var type in targetAssembly.GetTypes())
            {
                //_logger.Info($"type.FullName = {type.FullName}");

                typesDict[type.FullName] = type;
            }

            var memberCardsList = XMLMemberCardsReader.Read(targetXMLDocFileName);

            _logger.Info($"memberCardsList.Count = {memberCardsList.Count}");

            var cardsOfTypesList = new List<XMLMemberCard>();
            var cardsOfMembersList = new List<XMLMemberCard>();

            foreach (var memberCard in memberCardsList)
            {
                if(memberCard.Name.Kind == KindOfMember.Type)
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

            var classesOrInterfacesList = new List<ClassCard>();
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

                switch(kindOfType)
                {
                    case KindOfType.Class:
                    case KindOfType.Interface:
                        classesOrInterfacesList.Add(ProcessClassOrInterface(typeCard, type, membersList, kindOfType));
                        break;

                    case KindOfType.Enum:
                        enumsList.Add(ProcessEnum(typeCard, type, membersList));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfType), kindOfType, null);
                }

                //_logger.Info($" = {}");
            }

            var notProcessedCardsOfMembersList = cardsOfMembersList.Where(p => !p.IsProcessed).ToList();

            _logger.Info($"notProcessedCardsOfMembersList.Count = {notProcessedCardsOfMembersList.Count}");

            throw new NotImplementedException();

            foreach (var memberCard in notProcessedCardsOfMembersList)
            {
                _logger.Info($"memberCard = {memberCard}");
            }

            _logger.Info("End");
        }

        private EnumCard ProcessEnum(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList)
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

        private EnumFieldCard ProcessField(XMLMemberCard memberCard, Type parentType)
        {
            var result = new EnumFieldCard();

            FillUpNamedElementCard(memberCard, result);

            result.FieldInfo = parentType.GetField(memberCard.Name.Name);

            result.KindOfMemberAccess = GetKindOfMemberAccess(result.FieldInfo);

            _logger.Info($"result = {result}");

            return result;
        }

        private ClassCard ProcessClassOrInterface(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList, KindOfType kindOfType)
        {
            var result = new ClassCard();
            result.KindOfType = kindOfType;
            result.Type = type;
            result.IsPublic = type.IsPublic;

            FillUpNamedElementCard(typeCard, result);

            _logger.Info($"membersList.Count = {membersList.Count}");

            foreach(var memberCard in membersList)
            {
                _logger.Info($"memberCard = {memberCard}");

                switch(memberCard.Name.Kind)
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

        private MethodCard ProcessMethod(XMLMemberCard memberCard, Type parentType)
        {
            var result = new MethodCard();

            FillUpNamedElementCard(memberCard, result);

            var name = memberCard.Name.Name;

            if(!string.IsNullOrWhiteSpace(memberCard.Name.ImplInterfaceName))
            {
                name = $"{memberCard.Name.ImplInterfaceName}.{name}";
            }

            _logger.Info($"name = {name}");

            var methodsList = parentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => p.Name == name).ToList();

            var methodsCount = methodsList.Count;

            _logger.Info($"methodsCount = {methodsCount}");

            switch(methodsCount)
            {
                case 0:
                    throw new NotImplementedException();

                case 1:
                    result.MethodInfo = methodsList.Single();
                    break;

                default:
                    if(memberCard.IsInclude || memberCard.IsInheritdoc)
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

            if(memberCard.ParamsList.Any())
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

            if(memberCard.TypeParamsList.Any())
            {
                throw new NotImplementedException();
            }

            if(memberCard.ExceptionsList.Any())
            {
                throw new NotImplementedException();
            }

            _logger.Info($"result = {result}");

            return result;
        }

        private MethodInfo GetMethodInfoByTypeNames(List<MethodInfo> methodsList, XMLMemberCard memberCard)
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

                    if(NamesHelper.SimplifyFullNameOfType(param.ParameterType.FullName) != currentXMLParam)
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

        private MethodInfo GetMethodInfo(List<MethodInfo> methodsList, XMLMemberCard memberCard)
        {
            _logger.Info($"methodsList.Count = {methodsList.Count}");

            var xmlParamsList = memberCard.ParamsList;

            var xmlParamsCount = xmlParamsList.Count;

            _logger.Info($"xmlParamsCount = {xmlParamsCount}");

            foreach (var method in methodsList)
            {
                var paramsList = method.GetParameters();

                _logger.Info($"paramsList.Length = {paramsList.Length}");

                if(paramsList.Length != xmlParamsCount)
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

                    if(param.Name != currentXMLParam.Name)
                    {
                        isFit = false;
                        break;
                    }
                }

                _logger.Info($"isFit = {isFit}");

                if(isFit)
                {
                    return method;
                }
            }            

            throw new NotImplementedException();
        }

        private KindOfMemberAccess GetKindOfMemberAccess(FieldInfo fieldInfo)
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

        private KindOfMemberAccess GetKindOfMemberAccess(MethodInfo methodInfo)
        {
            _logger.Info($"methodInfo.IsPublic = {methodInfo.IsPublic}");
            _logger.Info($"methodInfo.IsPublic = {methodInfo.IsFamily}");

            if(methodInfo.IsPublic)
            {
                return KindOfMemberAccess.Public;
            }

            if(methodInfo.IsFamily)
            {
                return KindOfMemberAccess.Protected;
            }

            return KindOfMemberAccess.Private;
        }

        private PropertyCard ProcessProperty(XMLMemberCard memberCard, Type parentType)
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

        private void FillUpNamedElementCard(XMLMemberCard source, NamedElementCard dest)
        {
            dest.Name = source.Name;
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList;
            dest.XMLMemberCard = source;
            source.IsProcessed = true;
        }

        private Type _delegateType = typeof(Delegate);

        private KindOfType GetKindOfType(Type type)
        {
            if(type.IsClass)
            {
                if(_delegateType.IsAssignableFrom(type))
                {
                    return KindOfType.Delegate;
                }
                return KindOfType.Class;
            }

            if(type.IsInterface)
            {
                return KindOfType.Interface;
            }

            if(type.IsEnum)
            {
                return KindOfType.Enum;
            }

            throw new NotImplementedException();
        }

        public void ParseGenericType()
        {
            _logger.Info("Begin");

            var fullTypeName = "System.Collections.Generic.IList{SymOntoClay.UnityAsset.Core.DeviceOfBiped}";

            var figureBracketPos = fullTypeName.IndexOf("{");

            _logger.Info($"figureBracketPos = {figureBracketPos}");

            var name = fullTypeName.Substring(0, figureBracketPos);

            _logger.Info($"name = {name}");

            var strWithParameters = fullTypeName.Substring(figureBracketPos + 1, fullTypeName.Length - figureBracketPos - 2);

            _logger.Info($"strWithParameters = {strWithParameters}");

            var parametersList = MemberNameParser.GetParametersList(strWithParameters);

            _logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList, Formatting.Indented)}");

            _logger.Info("End");
        }
    }
}
