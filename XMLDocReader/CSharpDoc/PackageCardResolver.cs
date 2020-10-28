using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class PackageCardResolver
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void FillUpTypeCardsPropetties(List<PackageCard> packageCardsList, bool ignoreErrors)
        {
            //_logger.Info($"ignoreErrors = {ignoreErrors}");

            //_logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();

            var namedElemsList = new List<NamedElementCard>();

            foreach (var packageCard in packageCardsList)
            {
                classesList.AddRange(packageCard.ClassesList);
                namedElemsList.AddRange(packageCard.ClassesList);

                interfacesList.AddRange(packageCard.InterfacesList);
                namedElemsList.AddRange(packageCard.InterfacesList);

                namedElemsList.AddRange(packageCard.EnumsList);
            }

            //_logger.Info($"classesList.Count = {classesList.Count}");
            //_logger.Info($"interfacesList.Count = {interfacesList.Count}");

            var classesAndInterfacesList = classesList.Concat(interfacesList).ToList();

            var classesCardInitialNamesDict = classesAndInterfacesList.ToDictionary(p => p.Name.InitialName, p => p);
            var namedElemsCardInitialNamesDict = namedElemsList.ToDictionary(p => p.Name.InitialName, p => p);

            foreach (var classCard in classesAndInterfacesList)
            {
                FillUpTypeCard(classCard, classesCardInitialNamesDict, namedElemsCardInitialNamesDict, ignoreErrors);
            }

            //_logger.Info("End");
        }

        private static void FillUpTypeCard(ClassCard classCard, Dictionary<string, ClassCard> classesCardInitialNamesDict, Dictionary<string, NamedElementCard> namedElemsCardInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"classCard = {classCard}");

            var baseType = classCard.Type.BaseType;

            if(baseType != null && !TypesHelper.IsSystemOrThirdPartyType(baseType.FullName))
            {
                //_logger.Info($"baseType.FullName = {baseType.FullName}");

                var normalizedFullName = $"T:{NamesHelper.SimplifyFullNameOfType(baseType.FullName)}";

                //_logger.Info($"normalizedFullName = {normalizedFullName}");

                if(classesCardInitialNamesDict.ContainsKey(normalizedFullName))
                {
                    classCard.BaseType = classesCardInitialNamesDict[normalizedFullName];
                }
                else
                {
                    var errorStr = $"'{baseType.FullName}' must be documented.";

                    if(ignoreErrors)
                    {
                        classCard.ErrorsList.Add(errorStr);
                    }
                    else
                    {
                        throw new Exception(errorStr);
                    }                    
                }
            }

            var interfacesList = TypesHelper.GetInterfacesList(classCard.Type, true);

            //_logger.Info($"interfacesList.Count = {interfacesList.Count}");

            if(interfacesList.Any())
            {
                foreach(var interfaceItem in interfacesList)
                {
                    //_logger.Info($"interfaceItem.FullName = {interfaceItem.FullName}");

                    var normalizedFullName = $"T:{NamesHelper.SimplifyFullNameOfType(interfaceItem.FullName)}";

                    //_logger.Info($"normalizedFullName = {normalizedFullName}");

                    if (classesCardInitialNamesDict.ContainsKey(normalizedFullName))
                    {
                        classCard.BaseInterfacesList.Add(classesCardInitialNamesDict[normalizedFullName]);
                    }
                    else
                    {
                        var errorStr = $"'{interfaceItem.FullName}' must be documented.";

                        if (ignoreErrors)
                        {
                            classCard.ErrorsList.Add(errorStr);
                        }
                        else
                        {
                            throw new Exception(errorStr);
                        }
                    }
                }

                var directInterfacesList = TypesHelper.GetDirectlyImplementedInterfacesList(interfacesList, true);

                //_logger.Info($"directInterfacesList.Count = {directInterfacesList.Count}");

                if(directInterfacesList.Any())
                {
                    foreach(var directInterface in directInterfacesList)
                    {
                        //_logger.Info($"directInterface.FullName = {directInterface.FullName}");

                        var normalizedFullName = $"T:{NamesHelper.SimplifyFullNameOfType(directInterface.FullName)}";

                        //_logger.Info($"normalizedFullName = {normalizedFullName}");

                        if (classesCardInitialNamesDict.ContainsKey(normalizedFullName))
                        {
                            classCard.DirectBaseInterfacesList.Add(classesCardInitialNamesDict[normalizedFullName]);
                        }
                        else
                        {
                            var errorStr = $"'{directInterface.FullName}' must be documented.";

                            if (ignoreErrors)
                            {
                                if(!classCard.ErrorsList.Contains(errorStr))
                                {
                                    classCard.ErrorsList.Add(errorStr);
                                }                                
                            }
                            else
                            {
                                throw new Exception(errorStr);
                            }
                        }
                    }
                }
            }

            //_logger.Info($"classCard.PropertiesList.Count = {classCard.PropertiesList.Count}");

            if (classCard.PropertiesList.Any())
            {
                foreach(var property in classCard.PropertiesList)
                {
                    //_logger.Info($"property = {property}");

                    var propertyType = property.PropertyInfo.PropertyType;

                    //_logger.Info($"propertyType = {propertyType}");

                    if(propertyType != null)
                    {
                        var typesTuple = GetTargetTypesForMember(propertyType, namedElemsCardInitialNamesDict, ignoreErrors);

                        //_logger.Info($"typesTuple.Item1 = {typesTuple.Item1}");
                        //_logger.Info($"typesTuple.Item2 = {typesTuple.Item2}");
                        //_logger.Info($"typesTuple.Item3 = {typesTuple.Item3.WriteListToString()}");

                        property.PropertyTypeCard = typesTuple.Item1;
                        property.PropertyTypeName = typesTuple.Item2;
                        property.UsedTypesList = typesTuple.Item3;
                        property.ErrorsList = typesTuple.Item4;

                        //_logger.Info($"property (after)= {property}");
                    }
                }
            }

            //_logger.Info($"classCard.MethodsList.Count = {classCard.MethodsList.Count}");

            if (classCard.MethodsList.Any())
            {
                foreach(var method in classCard.MethodsList)
                {
                    //_logger.Info($"method = {method}");

                    var returnsType = method.MethodInfo.ReturnType;

                    if(returnsType != null)
                    {
                        //_logger.Info($"returnsType.FullName = {returnsType.FullName}");

                        var typesTuple = GetTargetTypesForMember(returnsType, namedElemsCardInitialNamesDict, ignoreErrors);

                        //_logger.Info($"typesTuple.Item1 = {typesTuple.Item1}");
                        //_logger.Info($"typesTuple.Item2 = {typesTuple.Item2}");
                        //_logger.Info($"typesTuple.Item3 = {typesTuple.Item3.WriteListToString()}");

                        method.ReturnsTypeCard = typesTuple.Item1;
                        method.ReturnsTypeName = typesTuple.Item2;
                        method.UsedTypesList = typesTuple.Item3;
                        method.ErrorsList = typesTuple.Item4;

                        //_logger.Info($"method (after) = {method}");
                    }                    

                    if(method.ParamsList.Any())
                    {
                        foreach(var param in method.ParamsList)
                        {
                            //_logger.Info($"param = {param}");

                            var parameterType = param.ParameterInfo.ParameterType;

                            if(parameterType != null)
                            {
                                //_logger.Info($"parameterType.FullName = {parameterType.FullName}");

                                var typesTuple = GetTargetTypesForMember(parameterType, namedElemsCardInitialNamesDict, ignoreErrors);

                                //_logger.Info($"typesTuple.Item1 = {typesTuple.Item1}");
                                //_logger.Info($"typesTuple.Item2 = {typesTuple.Item2}");
                                //_logger.Info($"typesTuple.Item3 = {typesTuple.Item3.WriteListToString()}");

                                param.ParameterTypeCard = typesTuple.Item1;
                                param.ParameterTypeName = typesTuple.Item2;
                                param.UsedTypesList = typesTuple.Item3;
                                param.ErrorsList = typesTuple.Item4;

                                //_logger.Info($"param (after) = {param}");
                            }                            
                        }

                        //_logger.Info($"method (after) = {method}");
                    }                    
                }
            }
        }

        private static (NamedElementCard, MemberName, List<NamedElementCard>, List<string>) GetTargetTypesForMember(Type type, Dictionary<string, NamedElementCard> namedElemsCardInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"type.FullName = {type.FullName}");

            var normalizedFullName = $"T:{NamesHelper.SimplifyFullNameOfType(type.FullName)}";

            //_logger.Info($"normalizedFullName = {normalizedFullName}");

            var name = MemberNameParser.Parse(normalizedFullName);

            //_logger.Info($"name = {name}");

            if(TypesHelper.IsSystemOrThirdPartyType(type.FullName))
            {
                if(name.TypeParametersList.Any())
                {
                    var usedReturnsTypesList = new List<NamedElementCard>();
                    var errorsList = new List<string>();
                    var processedTypeNames = new List<string>();

                    foreach (var typeParametr in name.TypeParametersList)
                    {
                        FillUpUsedReturnsTypesList(typeParametr, usedReturnsTypesList, errorsList, processedTypeNames, namedElemsCardInitialNamesDict, ignoreErrors);
                    }

                    return (null, name, usedReturnsTypesList, errorsList);
                }
                else
                {
                    return (null, name, new List<NamedElementCard>(), new List<string>());
                }                
            }
            else
            {
                if (namedElemsCardInitialNamesDict.ContainsKey(normalizedFullName))
                {
                    var targetCard = namedElemsCardInitialNamesDict[normalizedFullName];

                    return (targetCard, name, new List<NamedElementCard>() { targetCard }, new List<string>());
                }
                else
                {
                    var errorStr = $"'{type.FullName}' must be documented.";

                    if (ignoreErrors)
                    {
                        return (null, name, new List<NamedElementCard>(), new List<string>() { errorStr });
                    }
                    else
                    {
                        throw new Exception(errorStr);
                    }
                }                
            }
        }

        private static void FillUpUsedReturnsTypesList(string nameStr, List<NamedElementCard> result, List<string> errorsList, List<string> processedTypeNames, Dictionary<string, NamedElementCard> namedElemsCardInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"nameStr = '{nameStr}'");

            if(processedTypeNames.Contains(nameStr))
            {
                return;
            }

            processedTypeNames.Add(nameStr);

            var normalizedFullName = $"T:{NamesHelper.SimplifyFullNameOfType(nameStr)}";

            //_logger.Info($"normalizedFullName = {normalizedFullName}");

            if (TypesHelper.IsSystemOrThirdPartyType(nameStr))
            {
                var name = MemberNameParser.Parse(normalizedFullName);

                //_logger.Info($"name = {name}");

                foreach (var typeParametr in name.TypeParametersList)
                {
                    FillUpUsedReturnsTypesList(typeParametr, result, errorsList, processedTypeNames, namedElemsCardInitialNamesDict, ignoreErrors);
                }
            }
            else
            {
                if (namedElemsCardInitialNamesDict.ContainsKey(normalizedFullName))
                {
                    result.Add(namedElemsCardInitialNamesDict[normalizedFullName]);
                }
                else
                {
                    var errorStr = $"'{nameStr}' must be documented.";

                    if (ignoreErrors)
                    {
                        errorsList.Add(errorStr);
                    }
                    else
                    {
                        throw new Exception(errorStr);
                    }
                }
            }                
        }

        public static void ResolveInheritdocAndInclude(List<PackageCard> packageCardsList, bool ignoreErrors)
        {
            //_logger.Info($"ignoreErrors = {ignoreErrors}");

            //_logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach (var packageCard in packageCardsList)
            {
                classesList.AddRange(packageCard.ClassesList);
                interfacesList.AddRange(packageCard.InterfacesList);
                enumsList.AddRange(packageCard.EnumsList);
            }

            //_logger.Info($"classesList.Count = {classesList.Count}");
            //_logger.Info($"interfacesList.Count = {interfacesList.Count}");
            //_logger.Info($"enumsList.Count = {enumsList.Count}");

            //_logger.Info($"classesList.Count(p => p.HasIsInheritdoc) = {classesList.Count(p => p.HasIsInheritdoc)}");
            //_logger.Info($"interfacesList.Count(p => p.HasIsInheritdoc) = {interfacesList.Count(p => p.HasIsInheritdoc)}");
            //_logger.Info($"enumsList.Count(p => p.HasIsInheritdoc) = {enumsList.Count(p => p.HasIsInheritdoc)}");

            var xmlMemberCardsList = classesList.Select(p => p.XMLMemberCard).Concat(interfacesList.Select(p => p.XMLMemberCard));

            //_logger.Info($"xmlMemberCardsList.Count() = {xmlMemberCardsList.Count()}");

            //var xmlMemberCardsFullNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.FullName, p => p);
       
            //_logger.Info($"xmlMemberCardsFullNamesDict.Count = {xmlMemberCardsFullNamesDict.Count}");

            var xmlMemberCardsInitialNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.InitialName, p => p);

            //_logger.Info($"xmlMemberCardsInitialNamesDict.Count = {xmlMemberCardsInitialNamesDict.Count}");

            var classesAndInterfacesList = classesList.Concat(interfacesList).ToList();

            var xmlMemberCardsTypesDict = classesAndInterfacesList.ToDictionary(p => p.Type, p => p.XMLMemberCard);

            //var classCardsFullNamesDict = classesAndInterfacesList.ToDictionary(p => p.Name.FullName, p => p);
            var classCardsInitialNamesDict = classesAndInterfacesList.ToDictionary(p => p.Name.InitialName, p => p);
            var classCardsTypesDict = classesAndInterfacesList.ToDictionary(p => p.Type, p => p);

            var classesWithIncludeList = classesList.Where(p => p.HasIsInclude).ToList();
            var interfacessWithIncludeList = interfacesList.Where(p => p.HasIsInclude).ToList();
            var enumsWithIncludeList = enumsList.Where(p => p.HasIsInclude).ToList();

            //_logger.Info($"classesWithIncludeList.Count = {classesWithIncludeList.Count}");
            //_logger.Info($"interfacessWithIncludeList.Count = {interfacessWithIncludeList.Count}");
            //_logger.Info($"enumsWithIncludeList.Count = {enumsWithIncludeList.Count}");

            if (classesWithIncludeList.Any())
            {
                throw new NotImplementedException();
            }

            if (interfacessWithIncludeList.Any())
            {
                throw new NotImplementedException();
            }

            if (enumsWithIncludeList.Any())
            {
                throw new NotImplementedException();
            }

            foreach (var interfaceCard in interfacesList.Where(p => p.HasIsInheritdoc))
            {
                ResolveInheritdocInClassCard(interfaceCard, xmlMemberCardsTypesDict, xmlMemberCardsInitialNamesDict, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);
            }

            foreach (var classCard in classesList.Where(p => p.HasIsInheritdoc))
            {
                ResolveInheritdocInClassCard(classCard, xmlMemberCardsTypesDict, xmlMemberCardsInitialNamesDict, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);
            }

            //_logger.Info($" = {}");

            //_logger.Info("End");
        }

        private static void ResolveInheritdocInClassCard(ClassCard classCard, Dictionary<Type, XMLMemberCard> xmlMemberCardsTypesDict, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict, Dictionary<Type, ClassCard> classCardsTypesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"classCard = {classCard}");

            var xmlMemberCard = classCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            ResolveInheritdocInParentClasses(classCard, xmlMemberCardsTypesDict, xmlMemberCardsInitialNamesDict, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);

            if (xmlMemberCard.IsInheritdoc && !xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                XMLMemberCard targetXMLMemberCard;

                if (string.IsNullOrWhiteSpace(xmlMemberCard.InheritdocCref))
                {
                    targetXMLMemberCard = ResolveInheritdoc(classCard, classCard.Type, xmlMemberCardsTypesDict, ignoreErrors);
                }
                else
                {
                    targetXMLMemberCard = ResolveInheritdoc(xmlMemberCard.InheritdocCref, xmlMemberCardsInitialNamesDict);

                    //_logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");
                }

                //_logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");

                if (targetXMLMemberCard != null)
                {
                    AssingResolvingInheritdoc(classCard, targetXMLMemberCard);

                    //_logger.Info($"classCard (after) = {classCard}");
                }
            }

            var propertiesList = classCard.PropertiesList.Where(p => p.XMLMemberCard.IsInheritdoc).ToList();

            //_logger.Info($"propertiesList.Count = {propertiesList.Count}");

            if (propertiesList.Any())
            {
                foreach (var propertyCard in propertiesList)
                {
                    ResolveInheritdocInPropertyCard(classCard, propertyCard, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);
                }
            }

            var methodsList = classCard.MethodsList.Where(p => p.XMLMemberCard.IsInheritdoc).ToList();

            //_logger.Info($"methodsList.Count = {methodsList.Count}");

            if (methodsList.Any())
            {
                foreach (var methodCard in methodsList)
                {
                    ResolveInheritdocInMethodCard(classCard, methodCard, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);
                }
            }

            //_logger.Info($"classCard (after) = {classCard}");
        }

        private static void ResolveInheritdocInParentClasses(ClassCard classCard, Dictionary<Type, XMLMemberCard> xmlMemberCardsTypesDict, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict, Dictionary<Type, ClassCard> classCardsTypesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            var baseTypesList = TypesHelper.GetBaseTypesAndInterfacesList(classCard.Type, true);

            //_logger.Info($"baseTypesList.Count = {baseTypesList.Count}");

            foreach(var baseType in baseTypesList)
            {
                //_logger.Info($"baseType.FullName = {baseType.FullName}");

                if(classCardsTypesDict.ContainsKey(baseType))
                {
                    var targetClassCard = classCardsTypesDict[baseType];

                    //_logger.Info($"targetClassCard = {targetClassCard}");

                    ResolveInheritdocInClassCard(targetClassCard, xmlMemberCardsTypesDict, xmlMemberCardsInitialNamesDict, classCardsTypesDict, classCardsInitialNamesDict, ignoreErrors);
                }
            }
        }

        private static void ResolveInheritdocInMethodCard(ClassCard classCard, MethodCard methodCard, Dictionary<Type, ClassCard> classCardsTypesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"methodCard = {methodCard}");

            var xmlMemberCard = methodCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            List<ClassCard> targetClassCardsList;

            if (string.IsNullOrWhiteSpace(xmlMemberCard.Name.ImplInterfaceName))
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInMethodCardBySearching(classCard, methodCard, classCardsTypesDict, ignoreErrors);
            }
            else
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInMethodCardByImplInterface(methodCard, classCardsInitialNamesDict);
            }

            //_logger.Info($"targetClassCardsList.Count = {targetClassCardsList.Count}");

            if (targetClassCardsList.Count == 0)
            {
                return;
            }

            if (targetClassCardsList.Count > 1)
            {
                var errorStr = $"Ambiguous resolving inheritdoc of property {xmlMemberCard.Name.Name} of `{classCard.Type.FullName}`. There are many summares for this property of: {JsonConvert.SerializeObject(targetClassCardsList.Select(p => p.Type.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

                if (ignoreErrors)
                {
                    methodCard.ErrorsList.Add(errorStr);
                    return;
                }
                else
                {
                    throw new Exception(errorStr);
                }
            }

            var targetClassCard = targetClassCardsList.Single();

            //_logger.Info($"targetClassCard = {targetClassCard}");

            var targetMethodCard = GetTargetMethodCardCardOfMethod(methodCard, targetClassCard, ignoreErrors);

            //_logger.Info($"targetMethodCard = {targetMethodCard}");

            if (targetMethodCard != null)
            {
                AssingMethodCardResolvingInheritdoc(methodCard, targetMethodCard);

                //_logger.Info($"methodCard (after) = {methodCard}");
            }
        }

        private static MethodCard GetTargetMethodCardCardOfMethod(MethodCard methodCard, ClassCard targetClassCard, bool ignoreErrors)
        {
            var name = methodCard.Name.Name;

            //_logger.Info($"name = '{name}'");

            var targetMethodCardsList = targetClassCard.MethodsList.Where(p => p.Name.Name == name).ToList();

            //_logger.Info($"targetMethodCardsList.Count = {targetMethodCardsList.Count}");

            if (!targetMethodCardsList.Any())
            {
                return null;
            }

            var methodCardsList = new List<MethodCard>();

            foreach (var targetMethodCard in targetMethodCardsList)
            {
                if (IsFit(methodCard, targetMethodCard))
                {
                    methodCardsList.Add(targetMethodCard);
                }
            }

            //_logger.Info($"methodCardsList.Count = {methodCardsList.Count}");

            if (!methodCardsList.Any())
            {
                return null;
            }

            if (methodCardsList.Count == 1)
            {
                return methodCardsList.Single();
            }

            throw new NotImplementedException();
        }

        public static bool IsFit(MethodCard methodCard, MethodCard targetMethodCard)
        {
            if (methodCard.Name.Name != targetMethodCard.Name.Name)
            {
                return false;
            }

            if (methodCard.ParamsList.Count != targetMethodCard.ParamsList.Count)
            {
                return false;
            }

            if (methodCard.ParamsList.Count == 0)
            {
                return true;
            }

            _logger.Info($"methodCard.ParamsList.Count = {methodCard.ParamsList.Count}");

            var isFit = true;

            var methodCardParamsListEnumerator = methodCard.ParamsList.GetEnumerator();

            foreach (var targetParam in targetMethodCard.ParamsList)
            {
                methodCardParamsListEnumerator.MoveNext();

                var currentMmethodCardParam = methodCardParamsListEnumerator.Current;

                _logger.Info($"targetParam.Name = {targetParam.Name}");
                _logger.Info($"currentMmethodCardParam.Name = {currentMmethodCardParam.Name}");

                throw new NotImplementedException();
            }

            _logger.Info($"isFit = {isFit}");

            throw new NotImplementedException();
        }

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInMethodCardByImplInterface(MethodCard methodCard, Dictionary<string, ClassCard> classCardsInitialNamesDict)
        {
            var implInterfaceName = methodCard.Name.ImplInterfaceName;

            if (classCardsInitialNamesDict.ContainsKey(implInterfaceName))
            {
                return new List<ClassCard>() { classCardsInitialNamesDict[implInterfaceName] };
            }

            return new List<ClassCard>();
        }

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInMethodCardBySearching(ClassCard classCard, MethodCard methodCard, Dictionary<Type, ClassCard> classCardsTypesDict, bool ignoreErrors)
        {
            var baseTypesList = TypesHelper.GetBaseTypesAndInterfacesList(classCard.Type, true);

            //_logger.Info($"baseTypesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(baseTypesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (!baseTypesList.Any())
            {
                return new List<ClassCard>();
            }

            var name = methodCard.Name.Name;

            //_logger.Info($"name = '{name}'");

            var result = new List<ClassCard>();

            foreach (var baseType in baseTypesList)
            {
                //_logger.Info($"baseType.FullName = {baseType.FullName}");

                var methodsList = baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => p.Name == name).ToList();

                //_logger.Info($"methodsList.Count = {methodsList.Count}");

                foreach (var methodInfo in methodsList)
                {
                    if (IsFit(methodCard, methodInfo))
                    {
                        if (classCardsTypesDict.ContainsKey(baseType))
                        {
                            var targetClassCard = classCardsTypesDict[baseType];

                            //_logger.Info($"targetClassCard = {targetClassCard}");

                            result.Add(targetClassCard);
                            break;
                        }

                        var errorStr = $"Documentation of `{baseType.FullName}` must be written!";

                        if (ignoreErrors)
                        {
                            methodCard.ErrorsList.Add(errorStr);
                        }
                        else
                        {
                            throw new Exception(errorStr);
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsFit(MethodCard methodCard, MethodInfo methodInfo)
        {
            var methodInfoParametersList = methodInfo.GetParameters();

            if (methodCard.ParamsList.Count != methodInfoParametersList.Count())
            {
                return false;
            }

            if (methodCard.ParamsList.Count == 0)
            {
                return true;
            }

            var methodCardParamsListEnumerator = methodCard.ParamsList.GetEnumerator();

            var isFit = true;

            foreach (var param in methodInfoParametersList)
            {
                methodCardParamsListEnumerator.MoveNext();

                var currentMethodCardParam = methodCardParamsListEnumerator.Current;

                _logger.Info($"param.Name = {param.Name}");
                _logger.Info($"currentMethodCardParam.Name = {currentMethodCardParam.Name}");

                throw new NotImplementedException();
            }

            _logger.Info($"isFit = {isFit}");

            throw new NotImplementedException();
        }

        private static void ResolveInheritdocInPropertyCard(ClassCard classCard, PropertyCard propertyCard, Dictionary<Type, ClassCard> classCardsTypesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            //_logger.Info($"propertyCard = {propertyCard}");

            var xmlMemberCard = propertyCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            List<ClassCard> targetClassCardsList;

            if (string.IsNullOrWhiteSpace(xmlMemberCard.Name.ImplInterfaceName))
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInPropertyCardBySearching(classCard, propertyCard, classCardsTypesDict);
            }
            else
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInPropertyCardByImplInterface(propertyCard, classCardsInitialNamesDict);
            }

            //_logger.Info($"targetClassCardsList.Count = {targetClassCardsList.Count}");

            if (targetClassCardsList.Count == 0)
            {
                return;
            }

            if (targetClassCardsList.Count > 1)
            {
                var errorStr = $"Ambiguous resolving inheritdoc of property {xmlMemberCard.Name.Name} of `{classCard.Type.FullName}`. There are many summares for this property of: {JsonConvert.SerializeObject(targetClassCardsList.Select(p => p.Type.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

                if (ignoreErrors)
                {
                    propertyCard.ErrorsList.Add(errorStr);
                    return;
                }
                else
                {
                    throw new Exception(errorStr);
                }
            }

            var targetClassCard = targetClassCardsList.Single();

            //_logger.Info($"targetClassCard = {targetClassCard}");

            var targetXMLMemberCard = GetXMLMemberCardOfProperty(propertyCard, targetClassCard, ignoreErrors);

            //_logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");

            if (targetXMLMemberCard != null)
            {
                AssingPropertyCardResolvingInheritdoc(propertyCard, targetXMLMemberCard);

                //_logger.Info($"propertyCard (after) = {propertyCard}");
            }
        }

        private static XMLMemberCard GetXMLMemberCardOfProperty(PropertyCard propertyCard, ClassCard targetClassCard, bool ignoreErrors)
        {
            var name = propertyCard.Name.Name;

            //_logger.Info($"name = '{name}'");

            var targetPropertiesCardsList = targetClassCard.PropertiesList.Where(p => p.Name.Name == name).ToList();

            //_logger.Info($"targetPropertiesCardsList.Count = {targetPropertiesCardsList.Count}");

            if (targetPropertiesCardsList.Count == 0)
            {
                return null;
            }

            if (targetPropertiesCardsList.Count == 1)
            {
                return targetPropertiesCardsList.Single().XMLMemberCard;
            }

            var errorStr = $"Ambiguous resolving inheritdoc of property {propertyCard.XMLMemberCard.Name.Name} of `{propertyCard.Parent.Type.FullName}`. There are many summares for this property of: {targetClassCard.Type.FullName}. Use `cref` for describing target inheritdoc or describe summary.";

            if (ignoreErrors)
            {
                propertyCard.ErrorsList.Add(errorStr);
                return null;
            }
            else
            {
                throw new Exception(errorStr);
            }
        }

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInPropertyCardByImplInterface(PropertyCard propertyCard, Dictionary<string, ClassCard> classCardsInitialNamesDict)
        {
            var implInterfaceName = propertyCard.Name.ImplInterfaceName;

            if (classCardsInitialNamesDict.ContainsKey(implInterfaceName))
            {
                return new List<ClassCard>() { classCardsInitialNamesDict[implInterfaceName] };
            }

            return null;
        }

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInPropertyCardBySearching(ClassCard classCard, PropertyCard propertyCard, Dictionary<Type, ClassCard> classCardsTypesDict)
        {
            var baseTypesList = TypesHelper.GetBaseTypesAndInterfacesList(classCard.Type, true);

            //_logger.Info($"baseTypesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(baseTypesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (!baseTypesList.Any())
            {
                return new List<ClassCard>();
            }

            var name = propertyCard.Name.Name;

            //_logger.Info($"name = '{name}'");

            var result = new List<ClassCard>();

            foreach (var baseType in baseTypesList)
            {
                //_logger.Info($"baseType.FullName = {baseType.FullName}");

                var property = baseType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (property == null)
                {
                    continue;
                }

                //_logger.Info($"property = {property}");

                if (classCardsTypesDict.ContainsKey(baseType))
                {
                    var targetCard = classCardsTypesDict[baseType];

                    //_logger.Info($"targetCard = {targetCard}");

                    result.Add(targetCard);
                }
            }

            return result;
        }

        private static void AssingMethodCardResolvingInheritdoc(MethodCard dest, MethodCard source)
        {
            dest.Returns = source.Returns;
            dest.ParamsList = source.ParamsList.ToList();

            AssingResolvingInheritdoc(dest, source.XMLMemberCard);
        }

        private static void AssingPropertyCardResolvingInheritdoc(PropertyCard dest, XMLMemberCard source)
        {
            dest.Value = source.Value;

            AssingResolvingInheritdoc(dest, source);
        }

        private static void AssingResolvingInheritdoc(NamedElementCard dest, XMLMemberCard source)
        {
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList.ToList();

            dest.XMLMemberCard.IsInheritdocOrIncludeResolved = true;
        }

        private static XMLMemberCard ResolveInheritdoc(string cRef, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            //_logger.Info($"cRef = '{cRef}'");

            if (xmlMemberCardsInitialNamesDict.ContainsKey(cRef))
            {
                var targetXmlMemberCard = xmlMemberCardsInitialNamesDict[cRef];

                if ((targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude) && !targetXmlMemberCard.IsInheritdocOrIncludeResolved)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }

        private static XMLMemberCard ResolveInheritdoc(ClassCard classCard, Type type, Dictionary<Type, XMLMemberCard> xmlMemberCardsTypesDict, bool ignoreErrors)
        {
            var interfacesList = type.GetInterfaces().Where(p => !TypesHelper.IsSystemOrThirdPartyType(p.FullName)).ToList();

            //_logger.Info($"interfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            //_logger.Info($"type.BaseType?.FullName = {type.BaseType?.FullName}");

            if (type.BaseType == null || TypesHelper.IsSystemOrThirdPartyType(type.BaseType.FullName))
            {
                if (!interfacesList.Any())
                {
                    return null;
                }
                else
                {
                    return ResolveInheritdocByInterfaces(classCard, type, interfacesList, xmlMemberCardsTypesDict, ignoreErrors);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static XMLMemberCard ResolveInheritdocByInterfaces(ClassCard classCard, Type type, List<Type> interfacesList, Dictionary<Type, XMLMemberCard> xmlMemberCardsTypesDict, bool ignoreErrors)
        {
            if (interfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(interfacesList, xmlMemberCardsTypesDict);
            }

            var directlyImplementedInterfacesList = TypesHelper.GetDirectlyImplementedInterfacesList(interfacesList, true);

            //_logger.Info($"directlyImplementedInterfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(directlyImplementedInterfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (directlyImplementedInterfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(directlyImplementedInterfacesList, xmlMemberCardsTypesDict);
            }

            var errorStr = $"Ambiguous resolving inheritdoc of `{type.FullName}`. There are many summares for this type of: {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

            if (ignoreErrors)
            {
                classCard.ErrorsList.Add(errorStr);
                return null;
            }
            else
            {
                throw new Exception(errorStr);
            }
        }

        private static XMLMemberCard ResolveInheritdocBySingleInterface(List<Type> interfacesList, Dictionary<Type, XMLMemberCard> xmlMemberCardsTypesDict)
        {
            var targetInterface = interfacesList.Single();

            if (xmlMemberCardsTypesDict.ContainsKey(targetInterface))
            {
                var targetXmlMemberCard = xmlMemberCardsTypesDict[targetInterface];

                if ((targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude) && !targetXmlMemberCard.IsInheritdocOrIncludeResolved)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }
    }
}
