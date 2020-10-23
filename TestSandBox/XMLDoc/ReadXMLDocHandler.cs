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

            var fileNamesList = new List<string>() 
            {
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClayCoreHelper.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.Core.xml"),                
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml")
            };

            _logger.Info($"fileNamesList = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");

            var settingsList = PackageCardReaderSettingsHelper.ConvertXMLFileNamesListToSettingsList(fileNamesList);

            //_logger.Info($"settingsList = {JsonConvert.SerializeObject(settingsList, Formatting.Indented)}");

            var packageCardsList = PackageCardReader.Read(settingsList);

            _logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach(var packageCard in packageCardsList)
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

            var xmlMemberCardsList = classesList.Select(p => p.XMLMemberCard).Concat(interfacesList.Select(p => p.XMLMemberCard)).Concat(enumsList.Select(p => p.XMLMemberCard));

            _logger.Info($"xmlMemberCardsList.Count() = {xmlMemberCardsList.Count()}");

            var xmlMemberCardsFullNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.FullName, p => p);

            _logger.Info($"xmlMemberCardsFullNamesDict.Count = {xmlMemberCardsFullNamesDict.Count}");

            var xmlMemberCardsInitialNamesDict = xmlMemberCardsList.ToDictionary(p => p.Name.InitialName, p => p);

            _logger.Info($"xmlMemberCardsInitialNamesDict.Count = {xmlMemberCardsInitialNamesDict.Count}");

            foreach (var interfaceCard in interfacesList.Where(p => p.HasIsInheritdoc))
            {
                ResolveInheritdocInClassCard(interfaceCard, xmlMemberCardsFullNamesDict, xmlMemberCardsInitialNamesDict);
            }

            foreach (var classCard in classesList.Where(p => p.HasIsInheritdoc))
            {
                ResolveInheritdocInClassCard(classCard, xmlMemberCardsFullNamesDict, xmlMemberCardsInitialNamesDict);
            }

            _logger.Info($"classesList.Count(p => p.HasIsInclude) = {classesList.Count(p => p.HasIsInclude)}");
            _logger.Info($"interfacesList.Count(p => p.HasIsInclude) = {interfacesList.Count(p => p.HasIsInclude)}");
            _logger.Info($"enumsList.Count(p => p.HasIsInclude) = {enumsList.Count(p => p.HasIsInclude)}");

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void ResolveInheritdocInClassCard(ClassCard classCard, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            _logger.Info($"classCard = {classCard}");

            var xmlMemberCard = classCard.XMLMemberCard;

            if(xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            if (xmlMemberCard.IsInheritdoc && !xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                XMLMemberCard targetXMLMemberCard;

                if (string.IsNullOrWhiteSpace(xmlMemberCard.InheritdocCref))
                {
                    targetXMLMemberCard = ResolveInheritdoc(classCard.Type, xmlMemberCardsFullNamesDict);
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
                    ResolveInheritdocInPropertyCard(classCard, propertyCard, xmlMemberCardsInitialNamesDict);
                }
            }
        }

        private void ResolveInheritdocInPropertyCard(ClassCard classCard, PropertyCard propertyCard, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            _logger.Info($"propertyCard = {propertyCard}");

            var xmlMemberCard = propertyCard.XMLMemberCard;

            if (xmlMemberCard.IsInheritdocOrIncludeResolved)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(xmlMemberCard.Name.ImplInterfaceName))
            {
                ResolveInheritdocInPropertyCardBySearching(classCard, propertyCard);
                return;
            }

            ResolveInheritdocInPropertyCardByImplInterface(propertyCard, xmlMemberCardsInitialNamesDict);
        }

        private void ResolveInheritdocInPropertyCardByImplInterface(PropertyCard propertyCard, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            var implInterfaceName = propertyCard.Name.ImplInterfaceName;

            var targetXMLMemberCard = GetXMLMemberCardByImplInterfaceName(implInterfaceName, xmlMemberCardsInitialNamesDict);

            if(targetXMLMemberCard != null)
            {
                AssingPropertyCardResolvingInheritdoc(propertyCard, targetXMLMemberCard);
            }
        }

        private XMLMemberCard GetXMLMemberCardByImplInterfaceName(string implInterfaceName, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            if (xmlMemberCardsInitialNamesDict.ContainsKey(implInterfaceName))
            {
                var targetXmlMemberCard = xmlMemberCardsInitialNamesDict[implInterfaceName];

                if (targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }

        private void ResolveInheritdocInPropertyCardBySearching(ClassCard classCard, PropertyCard propertyCard)
        {
            var baseTypesList = GetBaseTypesAndInterfacesList(classCard.Type, true);

            _logger.Info($"baseTypesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(baseTypesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if(!baseTypesList.Any())
            {
                return;
            }

            var name = propertyCard.Name.Name;

            _logger.Info($"name = '{name}'");

            var propertiesList = new List<PropertyInfo>();

            //foreach()
            //{

            //}

            throw new NotImplementedException();
        }

        private void AssingPropertyCardResolvingInheritdoc(PropertyCard dest, XMLMemberCard source)
        {
            throw new NotImplementedException();
        }

        private void AssingResolvingInheritdoc(NamedElementCard dest, XMLMemberCard  source)
        {
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList.ToList();

            dest.XMLMemberCard.IsInheritdocOrIncludeResolved = true;
        }

        private XMLMemberCard ResolveInheritdoc(string cRef, Dictionary<string, XMLMemberCard> xmlMemberCardsInitialNamesDict)
        {
            _logger.Info($"cRef = '{cRef}'");

            if(xmlMemberCardsInitialNamesDict.ContainsKey(cRef))
            {
                var targetXmlMemberCard = xmlMemberCardsInitialNamesDict[cRef];

                if (targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }

        private XMLMemberCard ResolveInheritdoc(Type type, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict)
        {
            var interfacesList = type.GetInterfaces().Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();

            _logger.Info($"interfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            _logger.Info($"type.BaseType?.FullName = {type.BaseType?.FullName}");

            if(type.BaseType == null || IsSystemOrThirdPartyType(type.BaseType.FullName))
            {
                if(!interfacesList.Any())
                {
                    return null;
                }
                else
                {
                    return ResolveInheritdocByInterfaces(type, interfacesList, xmlMemberCardsFullNamesDict);
                }                
            }
            else
            {
                throw new NotImplementedException();
            }

            //var possibleXMLMemberCardList = new List<XMLMemberCard>();

            //foreach(var targetName in targetNamesList)
            //{
            //    _logger.Info($"targetName = '{targetName}'");

            //    if(xmlMemberCardsFullNamesDict.ContainsKey(targetName))
            //    {
            //        var possibleXMLMemberCard = xmlMemberCardsFullNamesDict[targetName];

            //        if(possibleXMLMemberCard.IsInheritdoc || possibleXMLMemberCard.IsInclude)
            //        {
            //            continue;
            //        }

            //        possibleXMLMemberCardList.Add(possibleXMLMemberCard);
            //    }
            //}

            //_logger.Info($"possibleXMLMemberCardList.Count = {possibleXMLMemberCardList.Count}");

            //foreach(var possibleXMLMemberCard in possibleXMLMemberCardList)
            //{
            //    _logger.Info($"possibleXMLMemberCard = {possibleXMLMemberCard}");
            //}

            //if(possibleXMLMemberCardList.Count == 0)
            //{
            //    return null;
            //}

            //if(possibleXMLMemberCardList.Count == 1)
            //{
            //    return possibleXMLMemberCardList.Single();
            //}
        }

        private XMLMemberCard ResolveInheritdocByInterfaces(Type type, List<Type> interfacesList, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict)
        {
            if(interfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(interfacesList, xmlMemberCardsFullNamesDict);
            }

            var directlyImplementedInterfacesList = GetDirectlyImplementedInterfacesList(interfacesList, true);

            _logger.Info($"directlyImplementedInterfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(directlyImplementedInterfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            if(directlyImplementedInterfacesList.Count == 1)
            {
                return ResolveInheritdocBySingleInterface(directlyImplementedInterfacesList, xmlMemberCardsFullNamesDict);
            }

            throw new Exception($"Ambiguous resolving inheritdoc of `{type.FullName}`. There are many summares for this type of: {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.");
        }

        private XMLMemberCard ResolveInheritdocBySingleInterface(List<Type> interfacesList, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict)
        {
            var targetInterface = interfacesList.Single();

            var targetInterfaceFullName = targetInterface.FullName;

            if (targetInterfaceFullName.Contains("["))
            {
                throw new NotImplementedException();
            }

            if (xmlMemberCardsFullNamesDict.ContainsKey(targetInterfaceFullName))
            {
                var targetXmlMemberCard = xmlMemberCardsFullNamesDict[targetInterfaceFullName];

                if (targetXmlMemberCard.IsInheritdoc || targetXmlMemberCard.IsInclude)
                {
                    return null;
                }

                return targetXmlMemberCard;
            }

            return null;
        }

        private bool IsSystemOrThirdPartyType(string fullName)
        {
            if(fullName.StartsWith("System."))
            {
                return true;
            }

            return false;
        }

        private List<Type> GetBaseTypesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var curentBaseType = type.BaseType;

            var typesList = new List<Type>();

            while (curentBaseType != null)
            {
                if(onlyNoSystemOrThirdPartyType && IsSystemOrThirdPartyType(curentBaseType.FullName))
                {
                    return typesList;
                }

                typesList.Add(curentBaseType);

                curentBaseType = curentBaseType.BaseType;
            }

            return typesList;
        }

        private List<Type> GetBaseTypesAndInterfacesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var typesList = GetBaseTypesList(type, onlyNoSystemOrThirdPartyType);

            var interfacesList = type.GetInterfaces().Distinct().ToList();

            if(onlyNoSystemOrThirdPartyType)
            {
                interfacesList = interfacesList.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            typesList.AddRange(interfacesList);

            return typesList;
        }

        public List<Type> GetDirectlyImplementedInterfacesList(List<Type> interfacesList, bool onlyNoSystemOrThirdPartyType)
        {
            var childInterfacesList = new List<Type>();

            foreach (var tmpInterface in interfacesList)
            {
                _logger.Info($"tmpInterface.FullName = {tmpInterface.FullName}");

                if(onlyNoSystemOrThirdPartyType)
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

            if(onlyNoSystemOrThirdPartyType)
            {
                result = result.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            return result;
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
