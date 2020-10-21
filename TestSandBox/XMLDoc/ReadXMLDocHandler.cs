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

            var targetXMLDocFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml");

            _logger.Info($"targetXMLDocFileName = {targetXMLDocFileName}");

            var targetAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.dll");

            _logger.Info($"targetAssemblyFileName = {targetAssemblyFileName}");

            var settings = new PackageCardReaderSettings();
            settings.XMLDocFileName = targetXMLDocFileName;
            settings.AssemblyFileName = targetAssemblyFileName;

            var packageCard = PackageCardReader.Read(settings);

            _logger.Info($"packageCard = {packageCard}");

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
