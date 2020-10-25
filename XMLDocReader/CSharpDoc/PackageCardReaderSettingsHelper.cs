using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class PackageCardReaderSettingsHelper
    {
        public static List<PackageCardReaderSettings> ConvertXMLFileNamesListToSettingsList(List<string> xmlFileNamesList)
        {
            var settingsList = new List<PackageCardReaderSettings>();

            foreach (var fileName in xmlFileNamesList)
            {
                var settings = new PackageCardReaderSettings();
                settings.XMLDocFileName = fileName;
                settings.AssemblyFileName = fileName.Replace(".xml", ".dll");
                settingsList.Add(settings);
            }

            return settingsList;
        }
    }
}
