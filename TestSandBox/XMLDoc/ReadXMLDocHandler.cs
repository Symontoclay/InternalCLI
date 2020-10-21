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

            foreach (var classCard in classesList.Where(p => p.HasIsInheritdoc))
            {
                _logger.Info($"classCard = {classCard}");

                var xmlMemberCard = classCard.XMLMemberCard;

                if(xmlMemberCard != null && xmlMemberCard.IsInheritdoc)
                {
                    if(string.IsNullOrWhiteSpace(xmlMemberCard.InheritdocCref))
                    {
                        var targetXMLMemberCard = ResolveInheritdoc(xmlMemberCard, classCard.Type, xmlMemberCardsFullNamesDict);

                        _logger.Info($"targetXMLMemberCard = {targetXMLMemberCard}");



                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            _logger.Info($"classesList.Count(p => p.HasIsInclude) = {classesList.Count(p => p.HasIsInclude)}");
            _logger.Info($"interfacesList.Count(p => p.HasIsInclude) = {interfacesList.Count(p => p.HasIsInclude)}");
            _logger.Info($"enumsList.Count(p => p.HasIsInclude) = {enumsList.Count(p => p.HasIsInclude)}");

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private XMLMemberCard ResolveInheritdoc(XMLMemberCard source, Type type, Dictionary<string, XMLMemberCard> xmlMemberCardsFullNamesDict)
        {
            var typesList = new List<Type>();

            var curentBaseType = type.BaseType;

            if(!IsSystemOrThirdPartyType(curentBaseType.FullName))
            {
                throw new NotImplementedException();
            }

            var targetNamesList = new List<string>();

            while (curentBaseType != null)
            {
                typesList.Add(curentBaseType);

                curentBaseType = curentBaseType.BaseType;
            }

            _logger.Info($"typesList.Select(p => p.FullName) (1) = {JsonConvert.SerializeObject(typesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            targetNamesList.AddRange(typesList.Select(p => p.FullName).Where(p => !IsSystemOrThirdPartyType(p)));

            var interfacesList = type.GetInterfaces();

            _logger.Info($"interfacesList.Select(p => p.FullName) (1) = {JsonConvert.SerializeObject(interfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            targetNamesList.AddRange(interfacesList.Select(p => p.FullName).Where(p => !IsSystemOrThirdPartyType(p)));

            _logger.Info($"targetNamesList = {JsonConvert.SerializeObject(targetNamesList, Formatting.Indented)}");

            if(!targetNamesList.Any())
            {
                return null;
            }

            var possibleXMLMemberCardList = new List<XMLMemberCard>();

            foreach(var targetName in targetNamesList)
            {
                _logger.Info($"targetName = '{targetName}'");

                if(xmlMemberCardsFullNamesDict.ContainsKey(targetName))
                {
                    var possibleXMLMemberCard = xmlMemberCardsFullNamesDict[targetName];

                    if(possibleXMLMemberCard.IsInheritdoc || possibleXMLMemberCard.IsInclude)
                    {
                        continue;
                    }

                    possibleXMLMemberCardList.Add(possibleXMLMemberCard);
                }
            }

            _logger.Info($"possibleXMLMemberCardList.Count = {possibleXMLMemberCardList.Count}");

            foreach(var possibleXMLMemberCard in possibleXMLMemberCardList)
            {
                _logger.Info($"possibleXMLMemberCard = {possibleXMLMemberCard}");
            }

            if(possibleXMLMemberCardList.Count == 0)
            {
                return null;
            }

            if(possibleXMLMemberCardList.Count == 1)
            {
                return possibleXMLMemberCardList.Single();
            }

            throw new Exception($"Ambiguous resolving inheritdoc of `{type.FullName}`. There are many summares for this type of: {JsonConvert.SerializeObject(possibleXMLMemberCardList.Select(p => p.Name.FullName), Formatting.Indented)}. Use `cref` for describing target inheritdoc or describe summary.");
        }

        private bool IsSystemOrThirdPartyType(string fullName)
        {
            if(fullName.StartsWith("System."))
            {
                return true;
            }

            return false;
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
