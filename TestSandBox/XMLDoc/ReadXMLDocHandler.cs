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

            //var targetRootTypeName = "T:SymOntoClay.UnityAsset.Core.WorldFactory";
            var targetRootTypeName = "SymOntoClay.UnityAsset.Core.IWorld";
            //var targetRootTypeName = "T:SymOntoClay.UnityAsset.Core.WorldSettings";

            _logger.Info($"targetRootTypeName = {targetRootTypeName}");

            var fileNamesList = new List<string>() 
            {
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClayCoreHelper.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.Core.xml"),                
                Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml")
            };

            _logger.Info($"fileNamesList = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");

            var ignoreErrors = true;

            var options = new CSharpDocLoaderOptions() 
            {
                FileNamesList = fileNamesList,
                TargetRootTypeNamesList = new List<string>() { targetRootTypeName },
                PublicMembersOnly = true,
                IgnoreErrors = ignoreErrors
            };

            _logger.Info($"options = {options}");

            var packageCardsList = CSharpXMLDocLoader.Load(options);

            var classesCleanedList = new List<ClassCard>();
            var interfacesCleanedList = new List<ClassCard>();
            var enumsCleanedList = new List<EnumCard>();

            _logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            foreach (var packageCard in packageCardsList)
            {
                _logger.Info($"packageCard.AssemblyName = {packageCard.AssemblyName}");

                classesCleanedList.AddRange(packageCard.ClassesList);
                interfacesCleanedList.AddRange(packageCard.InterfacesList);
                enumsCleanedList.AddRange(packageCard.EnumsList);
            }

            _logger.Info($"classesCleanedList.Count = {classesCleanedList.Count}");

            foreach (var classCard in classesCleanedList)
            {
                _logger.Info($"classCard.Name.InitialName = {classCard.Name.InitialName}");
            }

            _logger.Info($"interfacesCleanedList.Count = {interfacesCleanedList.Count}");

            foreach (var interfaceCard in interfacesCleanedList)
            {
                _logger.Info($"interfaceCard = {interfaceCard.Name.InitialName}");
            }

            _logger.Info($"enumsCleanedList.Count = {enumsCleanedList.Count}");

            foreach (var enumCard in enumsCleanedList)
            {
                _logger.Info($"enumCard = {enumCard.Name.InitialName}");
            }

            //_logger.Info($" = {}");

            _logger.Info("End");
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
