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

        public static void Resolve(List<PackageCard> packageCardsList, bool ignoreErrors)
        {
            _logger.Info($"ignoreErrors = {ignoreErrors}");

            _logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach (var packageCard in packageCardsList)
            {
                classesList.AddRange(packageCard.ClassesList);
                interfacesList.AddRange(packageCard.InterfacesList);
                enumsList.AddRange(packageCard.EnumsList);
            }

            _logger.Info($"classesList.Count = {classesList.Count}");
            _logger.Info($"interfacesList.Count = {interfacesList.Count}");
            _logger.Info($"enumsList.Count = {enumsList.Count}");

            _logger.Info($"classesList.Count(p => p.HasIsInheritdoc) = {classesList.Count(p => p.HasIsInheritdoc)}");
            _logger.Info($"interfacesList.Count(p => p.HasIsInheritdoc) = {interfacesList.Count(p => p.HasIsInheritdoc)}");
            _logger.Info($"enumsList.Count(p => p.HasIsInheritdoc) = {enumsList.Count(p => p.HasIsInheritdoc)}");

            var xmlMemberCardsList = classesList.Select(p => p.XMLMemberCard).Concat(interfacesList.Select(p => p.XMLMemberCard));

            _logger.Info($"xmlMemberCardsList.Count() = {xmlMemberCardsList.Count()}");

            var xmlMemberCardsFullNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.FullName, p => p);

            _logger.Info($"xmlMemberCardsFullNamesDict.Count = {xmlMemberCardsFullNamesDict.Count}");

            var xmlMemberCardsInitialNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.InitialName, p => p);

            _logger.Info($"xmlMemberCardsInitialNamesDict.Count = {xmlMemberCardsInitialNamesDict.Count}");

            var classesAndInterfacesList = classesList.Concat(interfacesList).ToList();

            var classCardsFullNamesDict = classesAndInterfacesList.ToDictionary(p => p.Name.FullName, p => p);
            var classCardsInitialNamesDict = classesAndInterfacesList.ToDictionary(p => p.Name.InitialName, p => p);

            var classesWithIncludeList = classesList.Where(p => p.HasIsInclude).ToList();
            var interfacessWithIncludeList = interfacesList.Where(p => p.HasIsInclude).ToList();
            var enumsWithIncludeList = enumsList.Where(p => p.HasIsInclude).ToList();

            _logger.Info($"classesWithIncludeList.Count = {classesWithIncludeList.Count}");
            _logger.Info($"interfacessWithIncludeList.Count = {interfacessWithIncludeList.Count}");
            _logger.Info($"enumsWithIncludeList.Count = {enumsWithIncludeList.Count}");

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
                ResolveInheritdocInClassCard(interfaceCard, xmlMemberCardsFullNamesDict, xmlMemberCardsInitialNamesDict, classCardsFullNamesDict, classCardsInitialNamesDict, ignoreErrors);
            }

            foreach (var classCard in classesList.Where(p => p.HasIsInheritdoc))
            {
                ResolveInheritdocInClassCard(classCard, xmlMemberCardsFullNamesDict, xmlMemberCardsInitialNamesDict, classCardsFullNamesDict, classCardsInitialNamesDict, ignoreErrors);
            }

            _logger.Info("End");
        }

        private static void ResolveInheritdocInClassCard(ClassCard classCard, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict, Dictionary<string, ClassCard> classCardsFullNamesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            _logger.Info($"classCard = {classCard}");

            var xmlMemberCard = classCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            if (xmlMemberCard.IsInheritdoc && !xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                XMLMemberCard targetXMLMemberCard;

                if (string.IsNullOrWhiteSpace(xmlMemberCard.InheritdocCref))
                {
                    targetXMLMemberCard = ResolveInheritdoc(classCard, classCard.Type, xmlMemberCardsFullNamesDict, ignoreErrors);
                }
                else
                {
                    targetXMLMemberCard = ResolveInheritdoc(xmlMemberCard.InheritdocCref, xmlMemberCardsInitialNamesDict);

                    _logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");
                }

                _logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");

                if (targetXMLMemberCard != null)
                {
                    AssingResolvingInheritdoc(classCard, targetXMLMemberCard);

                    _logger.Info($"classCard (after) = {classCard}");
                }
            }

            var propertiesList = classCard.PropertiesList.Where(p => p.XMLMemberCard.IsInheritdoc).ToList();

            _logger.Info($"propertiesList.Count = {propertiesList.Count}");

            if (propertiesList.Any())
            {
                foreach (var propertyCard in propertiesList)
                {
                    ResolveInheritdocInPropertyCard(classCard, propertyCard, classCardsFullNamesDict, classCardsInitialNamesDict, ignoreErrors);
                }
            }

            var methodsList = classCard.MethodsList.Where(p => p.XMLMemberCard.IsInheritdoc).ToList();

            _logger.Info($"methodsList.Count = {methodsList.Count}");

            if (methodsList.Any())
            {
                foreach (var methodCard in methodsList)
                {
                    ResolveInheritdocInMethodCard(classCard, methodCard, classCardsFullNamesDict, classCardsInitialNamesDict, ignoreErrors);
                }
            }

            _logger.Info($"classCard (after) = {classCard}");
        }

        private static void ResolveInheritdocInMethodCard(ClassCard classCard, MethodCard methodCard, Dictionary<string, ClassCard> classCardsFullNamesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            _logger.Info($"methodCard = {methodCard}");

            var xmlMemberCard = methodCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            List<ClassCard> targetClassCardsList;

            if (string.IsNullOrWhiteSpace(xmlMemberCard.Name.ImplInterfaceName))
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInMethodCardBySearching(classCard, methodCard, classCardsFullNamesDict, ignoreErrors);
            }
            else
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInMethodCardByImplInterface(methodCard, classCardsInitialNamesDict);
            }

            _logger.Info($"targetClassCardsList.Count = {targetClassCardsList.Count}");

            if (targetClassCardsList.Count == 0)
            {
                return;
            }

            if (targetClassCardsList.Count > 1)
            {
                var errorStr = $"Ambiguous resolving inheritdoc of property {xmlMemberCard.Name.Name} of `{classCard.Type.FullName}`. There are many summares for this property of: {JsonConvert.SerializeObject(targetClassCardsList.Select(p => p.Type.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

                if (ignoreErrors)
                {
                    methodCard.XMLMemberCard.ErrorsList.Add(errorStr);
                    return;
                }
                else
                {
                    throw new Exception(errorStr);
                }
            }

            var targetClassCard = targetClassCardsList.Single();

            _logger.Info($"targetClassCard = {targetClassCard}");

            var targetMethodCard = GetTargetMethodCardCardOfMethod(methodCard, targetClassCard, ignoreErrors);

            _logger.Info($"targetMethodCard = {targetMethodCard}");

            if (targetMethodCard != null)
            {
                AssingMethodCardResolvingInheritdoc(methodCard, targetMethodCard);

                _logger.Info($"methodCard (after) = {methodCard}");
            }
        }

        private static MethodCard GetTargetMethodCardCardOfMethod(MethodCard methodCard, ClassCard targetClassCard, bool ignoreErrors)
        {
            var name = methodCard.Name.Name;

            _logger.Info($"name = '{name}'");

            var targetMethodCardsList = targetClassCard.MethodsList.Where(p => p.Name.Name == name).ToList();

            _logger.Info($"targetMethodCardsList.Count = {targetMethodCardsList.Count}");

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

            _logger.Info($"methodCardsList.Count = {methodCardsList.Count}");

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

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInMethodCardBySearching(ClassCard classCard, MethodCard methodCard, Dictionary<string, ClassCard> classCardsFullNamesDict, bool ignoreErrors)
        {
            var baseTypesList = GetBaseTypesAndInterfacesList(classCard.Type, true);

            _logger.Info($"baseTypesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(baseTypesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (!baseTypesList.Any())
            {
                return new List<ClassCard>();
            }

            var name = methodCard.Name.Name;

            _logger.Info($"name = '{name}'");

            var result = new List<ClassCard>();

            foreach (var baseType in baseTypesList)
            {
                _logger.Info($"baseType.FullName = {baseType.FullName}");

                var methodsList = baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => p.Name == name).ToList();

                _logger.Info($"methodsList.Count = {methodsList.Count}");

                foreach (var methodInfo in methodsList)
                {
                    if (IsFit(methodCard, methodInfo))
                    {
                        var normalizedFullName = NamesHelper.SimplifyFullNameOfType(baseType.FullName);

                        _logger.Info($"normalizedFullName = {normalizedFullName}");

                        if (classCardsFullNamesDict.ContainsKey(normalizedFullName))
                        {
                            var targetClassCard = classCardsFullNamesDict[normalizedFullName];

                            _logger.Info($"targetClassCard = {targetClassCard}");

                            result.Add(targetClassCard);
                            break;
                        }

                        var errorStr = $"Documentation of `{baseType.FullName}` must be written!";

                        if (ignoreErrors)
                        {
                            methodCard.XMLMemberCard.ErrorsList.Add(errorStr);
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

        private static void ResolveInheritdocInPropertyCard(ClassCard classCard, PropertyCard propertyCard, Dictionary<string, ClassCard> classCardsFullNamesDict, Dictionary<string, ClassCard> classCardsInitialNamesDict, bool ignoreErrors)
        {
            _logger.Info($"propertyCard = {propertyCard}");

            var xmlMemberCard = propertyCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            List<ClassCard> targetClassCardsList;

            if (string.IsNullOrWhiteSpace(xmlMemberCard.Name.ImplInterfaceName))
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInPropertyCardBySearching(classCard, propertyCard, classCardsFullNamesDict);
            }
            else
            {
                targetClassCardsList = GetClassCardsForResolvingInheritdocInPropertyCardByImplInterface(propertyCard, classCardsInitialNamesDict);
            }

            _logger.Info($"targetClassCardsList.Count = {targetClassCardsList.Count}");

            if (targetClassCardsList.Count == 0)
            {
                return;
            }

            if (targetClassCardsList.Count > 1)
            {
                var errorStr = $"Ambiguous resolving inheritdoc of property {xmlMemberCard.Name.Name} of `{classCard.Type.FullName}`. There are many summares for this property of: {JsonConvert.SerializeObject(targetClassCardsList.Select(p => p.Type.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

                if (ignoreErrors)
                {
                    propertyCard.XMLMemberCard.ErrorsList.Add(errorStr);
                    return;
                }
                else
                {
                    throw new Exception(errorStr);
                }
            }

            var targetClassCard = targetClassCardsList.Single();

            _logger.Info($"targetClassCard = {targetClassCard}");

            var targetXMLMemberCard = GetXMLMemberCardOfProperty(propertyCard, targetClassCard, ignoreErrors);

            _logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");

            if (targetXMLMemberCard != null)
            {
                AssingPropertyCardResolvingInheritdoc(propertyCard, targetXMLMemberCard);

                _logger.Info($"propertyCard (after) = {propertyCard}");
            }
        }

        private static XMLMemberCard GetXMLMemberCardOfProperty(PropertyCard propertyCard, ClassCard targetClassCard, bool ignoreErrors)
        {
            var name = propertyCard.Name.Name;

            _logger.Info($"name = '{name}'");

            var targetPropertiesCardsList = targetClassCard.PropertiesList.Where(p => p.Name.Name == name).ToList();

            _logger.Info($"targetPropertiesCardsList.Count = {targetPropertiesCardsList.Count}");

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
                propertyCard.XMLMemberCard.ErrorsList.Add(errorStr);
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

        private static List<ClassCard> GetClassCardsForResolvingInheritdocInPropertyCardBySearching(ClassCard classCard, PropertyCard propertyCard, Dictionary<string, ClassCard> classCardsFullNamesDict)
        {
            var baseTypesList = GetBaseTypesAndInterfacesList(classCard.Type, true);

            _logger.Info($"baseTypesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(baseTypesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (!baseTypesList.Any())
            {
                return new List<ClassCard>();
            }

            var name = propertyCard.Name.Name;

            _logger.Info($"name = '{name}'");

            var result = new List<ClassCard>();

            foreach (var baseType in baseTypesList)
            {
                _logger.Info($"baseType.FullName = {baseType.FullName}");

                var property = baseType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (property == null)
                {
                    continue;
                }

                _logger.Info($"property = {property}");

                if (baseType.FullName.Contains("[["))
                {
                    throw new NotImplementedException();
                }

                if (classCardsFullNamesDict.ContainsKey(baseType.FullName))
                {
                    var targetCard = classCardsFullNamesDict[baseType.FullName];

                    _logger.Info($"targetCard = {targetCard}");

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
            _logger.Info($"cRef = '{cRef}'");

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

        private static XMLMemberCard ResolveInheritdoc(ClassCard classCard, Type type, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict, bool ignoreErrors)
        {
            var interfacesList = type.GetInterfaces().Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();

            _logger.Info($"interfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            _logger.Info($"type.BaseType?.FullName = {type.BaseType?.FullName}");

            if (type.BaseType == null || IsSystemOrThirdPartyType(type.BaseType.FullName))
            {
                if (!interfacesList.Any())
                {
                    return null;
                }
                else
                {
                    return ResolveInheritdocByInterfaces(classCard, type, interfacesList, xmlMemberCardsFullNamesDict, ignoreErrors);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static XMLMemberCard ResolveInheritdocByInterfaces(ClassCard classCard, Type type, List<Type> interfacesList, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict, bool ignoreErrors)
        {
            if (interfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(interfacesList, xmlMemberCardsFullNamesDict);
            }

            var directlyImplementedInterfacesList = GetDirectlyImplementedInterfacesList(interfacesList, true);

            _logger.Info($"directlyImplementedInterfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(directlyImplementedInterfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if (directlyImplementedInterfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(directlyImplementedInterfacesList, xmlMemberCardsFullNamesDict);
            }

            var errorStr = $"Ambiguous resolving inheritdoc of `{type.FullName}`. There are many summares for this type of: {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.";

            if (ignoreErrors)
            {
                classCard.XMLMemberCard.ErrorsList.Add(errorStr);
                return null;
            }
            else
            {
                throw new Exception(errorStr);
            }
        }

        private static XMLMemberCard ResolveInheritdocBySingleInterface(List<Type> interfacesList, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict)
        {
            var targetInterface = interfacesList.Single();

            var targetInterfaceFullName = targetInterface.FullName;

            if (targetInterfaceFullName.Contains("[["))
            {
                throw new NotImplementedException();
            }

            if (xmlMemberCardsFullNamesDict.ContainsKey(targetInterfaceFullName))
            {
                var targetXmlMemberCard = xmlMemberCardsFullNamesDict[targetInterfaceFullName];

                if ((targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude) && !targetXmlMemberCard.IsInheritdocOrIncludeResolved)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }

        private static bool IsSystemOrThirdPartyType(string fullName)
        {
            if (fullName.StartsWith("System."))
            {
                return true;
            }

            return false;
        }

        private static List<Type> GetBaseTypesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var curentBaseType = type.BaseType;

            var typesList = new List<Type>();

            while (curentBaseType != null)
            {
                if (onlyNoSystemOrThirdPartyType && IsSystemOrThirdPartyType(curentBaseType.FullName))
                {
                    return typesList;
                }

                typesList.Add(curentBaseType);

                curentBaseType = curentBaseType.BaseType;
            }

            return typesList;
        }

        private static List<Type> GetBaseTypesAndInterfacesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var typesList = GetBaseTypesList(type, onlyNoSystemOrThirdPartyType);

            var interfacesList = type.GetInterfaces().Distinct().ToList();

            if (onlyNoSystemOrThirdPartyType)
            {
                interfacesList = interfacesList.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            typesList.AddRange(interfacesList);

            return typesList;
        }

        public static List<Type> GetDirectlyImplementedInterfacesList(List<Type> interfacesList, bool onlyNoSystemOrThirdPartyType)
        {
            var childInterfacesList = new List<Type>();

            foreach (var tmpInterface in interfacesList)
            {
                _logger.Info($"tmpInterface.FullName = {tmpInterface.FullName}");

                if (onlyNoSystemOrThirdPartyType)
                {
                    childInterfacesList.AddRange(tmpInterface.GetInterfaces().Where(p => !IsSystemOrThirdPartyType(p.FullName)));
                }
                else
                {
                    childInterfacesList.AddRange(tmpInterface.GetInterfaces());
                }
            }

            _logger.Info($"childInterfacesList.Count = {childInterfacesList.Count}");

            _logger.Info($"childInterfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(childInterfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            var result = interfacesList.Except(childInterfacesList).ToList();

            if (onlyNoSystemOrThirdPartyType)
            {
                result = result.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            return result;
        }
    }
}
