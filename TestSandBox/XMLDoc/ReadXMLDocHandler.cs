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
using XMLDocReader.CSharpDoc;

namespace TestSandBox.XMLDoc
{
    public class ReadXMLDocHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var targetRootTypeName = "T:SymOntoClay.UnityAsset.Core.WorldFactory";

            _logger.Info($"targetRootTypeName = {targetRootTypeName}");

            var fileNamesList = new List<string>() 
            {
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClayCoreHelper.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.Core.xml"),                
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml")
            };

            _logger.Info($"fileNamesList = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");

            var settingsList = PackageCardReaderSettingsHelper.ConvertXMLFileNamesListToSettingsList(fileNamesList);

            //_logger.Info($"settingsList = {JsonConvert.SerializeObject(settingsList, Formatting.Indented)}");

            var ignoreErrors = true;

            var packageCardsList = PackageCardReader.Read(settingsList);

            PackageCardResolver.FillUpTypeCardsPropetties(packageCardsList, ignoreErrors);
            PackageCardResolver.ResolveInheritdocAndInclude(packageCardsList, ignoreErrors);

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

            var classesInitialNamesDict = classesList.ToDictionary(p => p.Name.InitialName, p => p);
            var interfacesInitialNamesDict = interfacesList.ToDictionary(p => p.Name.InitialName, p => p);
            var enumsInitialNamesDict = enumsList.ToDictionary(p => p.Name.InitialName, p => p);

            _logger.Info($"classesInitialNamesDict.Count = {classesInitialNamesDict.Count}");
            _logger.Info($"interfacesInitialNamesDict.Count = {interfacesInitialNamesDict.Count}");
            _logger.Info($"enumsInitialNamesDict.Count = {enumsInitialNamesDict.Count}");

            RepackTypeCard(targetRootTypeName, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void RepackTypeCard(string initialTypeName, Dictionary<string, ClassCard> classesInitialNamesDict, Dictionary<string, ClassCard> interfacesInitialNamesDict, Dictionary<string, EnumCard> enumsInitialNamesDict)
        {
            _logger.Info($"initialTypeName = '{initialTypeName}'");

            if(classesInitialNamesDict.ContainsKey(initialTypeName))
            {
                throw new NotImplementedException();
            }
            else
            {
                if(interfacesInitialNamesDict.ContainsKey(initialTypeName))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    if(enumsInitialNamesDict.ContainsKey(initialTypeName))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new Exception($"'{initialTypeName}' must be documented.");
                    }
                }
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
