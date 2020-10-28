using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class CSharpXMLDocLoader
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static List<PackageCard> Load(CSharpXMLDocLoaderOptions options)
        {
            //_logger.Info($"options = {options}");

            var settingsList = PackageCardReaderSettingsHelper.ConvertXMLFileNamesListToSettingsList(options.XmlFileNamesList);

            //_logger.Info($"settingsList = {JsonConvert.SerializeObject(settingsList, Formatting.Indented)}");

            var packageCardsList = PackageCardReader.Read(settingsList);

            PackageCardResolver.FillUpTypeCardsPropetties(packageCardsList, options.IgnoreErrors);
            PackageCardResolver.ResolveInheritdocAndInclude(packageCardsList, options.IgnoreErrors);

            if(options.PublicMembersOnly || options.TargetRootTypeNamesList.Any())
            {
                var repackingTypeCardOptions = new RepackingTypeCardOptions() { PublicMembersOnly = options.PublicMembersOnly };

                PackageCardCleaner.Clean(packageCardsList, options.TargetRootTypeNamesList, repackingTypeCardOptions);
            }

            return packageCardsList;
        }
    }
}
