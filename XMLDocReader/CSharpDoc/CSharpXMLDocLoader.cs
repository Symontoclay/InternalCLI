using CommonUtils;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class CSharpXMLDocLoader
    {
#if DEBUG
        //private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        public static List<PackageCard> Load(CSharpDocLoaderOptions options)
        {
#if DEBUG
            //_logger.Info($"options = {options}");
#endif

            var packageCardsList = ReadJsonFiles(options.FileNamesList);

            if (options.PublicMembersOnly || options.TargetRootTypeNamesList.Any())
            {
                var repackingTypeCardOptions = new RepackingTypeCardOptions() { PublicMembersOnly = options.PublicMembersOnly };

                PackageCardCleaner.Clean(packageCardsList, options.TargetRootTypeNamesList, repackingTypeCardOptions);
            }

            FillUpHrefAndTargetFullFileName(packageCardsList, options.BaseHref, options.DestDir);

            return packageCardsList;
        }

        public static List<PackageCard> LoadOrigin(CSharpOriginDocLoaderOptions options)
        {
#if DEBUG
            //_logger.Info($"options = {options}");
#endif
            var settingsList = PackageCardReaderSettingsHelper.ConvertXMLFileNamesListToSettingsList(options.FileNamesList);

#if DEBUG
            //_logger.Info($"settingsList = {JsonConvert.SerializeObject(settingsList, Formatting.Indented)}");
#endif

            var packageCardsList = PackageCardReader.Read(settingsList);

            PackageCardResolver.FillUpTypeCardsPropetties(packageCardsList, options.IgnoreErrors);
            PackageCardResolver.ResolveInheritdocAndInclude(packageCardsList, options.IgnoreErrors);

            return packageCardsList;
        }

        private static List<PackageCard> ReadJsonFiles(List<string> fileNamesList)
        {
            var result = new List<PackageCard>();

            foreach(var fileName in fileNamesList)
            {
                result.Add(JsonSerializationHelper.DeserializeObjectFromFile(fileName));
            }

            return result;
        }

        private static void FillUpHrefAndTargetFullFileName(List<PackageCard> packageCardsList, string baseHref, string destDir)
        {
#if DEBUG
            //_logger.Info($"baseHref = '{baseHref}'");
            //_logger.Info($"destDir = '{destDir}'");
#endif

            foreach (var packageCard in packageCardsList)
            {
                foreach (var item in packageCard.InterfacesList)
                {
                    FillUpPathsOfTypeCard(item, baseHref, destDir);

                    foreach(var prop in item.PropertiesList)
                    {
                        FillUpPathsOfMemberCard(prop, baseHref, destDir);
                    }

                    foreach (var method in item.MethodsList)
                    {
                        FillUpPathsOfMemberCard(method, baseHref, destDir);
                    }
                }

                foreach (var item in packageCard.ClassesList)
                {
                    FillUpPathsOfTypeCard(item, baseHref, destDir);

                    foreach(var constructor in item.ConstructorsList)
                    {
                        FillUpPathsOfMemberCard(constructor, baseHref, destDir);
                    }

                    foreach (var prop in item.PropertiesList)
                    {
                        FillUpPathsOfMemberCard(prop, baseHref, destDir);
                    }

                    foreach (var method in item.MethodsList)
                    {
                        FillUpPathsOfMemberCard(method, baseHref, destDir);
                    }
                }

                foreach (var item in packageCard.EnumsList)
                {
                    FillUpPathsOfTypeCard(item, baseHref, destDir);
                }
            }

            //throw new NotImplementedException();
        }

        private static void FillUpPathsOfTypeCard(IDocFileEditeblePaths item, string baseHref, string destDir)
        {
#if DEBUG
            //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
            //_logger.Info($"item.Href = {item.Href}");
            //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

            var preparedItemName = $"{item.Name.FullName.ToLower()}.html";

#if DEBUG
            //_logger.Info($"preparedItemName = {preparedItemName}");
            //_logger.Info($"preparedItemName.Length = {preparedItemName.Length}");
#endif

            preparedItemName = MinifyHref(preparedItemName);

#if DEBUG
            //_logger.Info($"preparedItemName (after) = {preparedItemName}");
            //_logger.Info($"preparedItemName.Length (after) = {preparedItemName.Length}");
#endif

            item.Href = Path.Combine(baseHref, preparedItemName).Replace("\\", "/");
            item.TargetFullFileName = Path.Combine(destDir, preparedItemName).Replace("\\", "/");

#if DEBUG
            //_logger.Info($"item.Href (after) = {item.Href}");
            //_logger.Info($"item.TargetFullFileName (after) = {item.TargetFullFileName}");
#endif
        }

        private static void FillUpPathsOfMemberCard(IDocFileEditeblePaths item, string baseHref, string destDir)
        {
            var preparedName = PrepareName(item.Name.InitialName);

#if DEBUG
            //_logger.Info($"preparedName = {preparedName}");
            //_logger.Info($"preparedName.Length = {preparedName.Length}");
#endif

            preparedName = MinifyHref(preparedName);

#if DEBUG
            //_logger.Info($"preparedName (after) = {preparedName}");
            //_logger.Info($"preparedName.Length (after) = {preparedName.Length}");
#endif

            item.Href = Path.Combine(baseHref, preparedName).Replace("\\", "/");
            item.TargetFullFileName = Path.Combine(destDir, preparedName).Replace("\\", "/");
        }

        private static string PrepareName(string initialName)
        {
            var preparedInitialName = initialName.Substring(2).Replace("#", "_").Replace("[", "_").Replace("]", "_").Replace("(", "_").Replace(")", "_").Replace("{", "_").Replace("}", "_").Replace(",", "_").ToLower();

            return $"{preparedInitialName}.html";
        }

        private static string MinifyHref(string value)
        {
            return $"{MD5Helper.GetHash(value)}.html";
        }
    }
}
