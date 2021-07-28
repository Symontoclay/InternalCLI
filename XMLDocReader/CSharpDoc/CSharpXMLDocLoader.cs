﻿using Newtonsoft.Json;
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
        public static List<PackageCard> Load(CSharpXMLDocLoaderOptions options)
        {
#if DEBUG
            //_logger.Info($"options = {options}");
#endif
            var settingsList = PackageCardReaderSettingsHelper.ConvertXMLFileNamesListToSettingsList(options.XmlFileNamesList);

#if DEBUG
            //_logger.Info($"settingsList = {JsonConvert.SerializeObject(settingsList, Formatting.Indented)}");
#endif
            var packageCardsList = PackageCardReader.Read(settingsList);

            PackageCardResolver.FillUpTypeCardsPropetties(packageCardsList, options.IgnoreErrors);
            PackageCardResolver.ResolveInheritdocAndInclude(packageCardsList, options.IgnoreErrors);

            if(options.PublicMembersOnly || options.TargetRootTypeNamesList.Any())
            {
                var repackingTypeCardOptions = new RepackingTypeCardOptions() { PublicMembersOnly = options.PublicMembersOnly };

                PackageCardCleaner.Clean(packageCardsList, options.TargetRootTypeNamesList, repackingTypeCardOptions);
            }

            FillUpHrefAndTargetFullFileName(packageCardsList, options.BaseHref, options.SourceDir, options.DestDir);

            return packageCardsList;
        }

        private static void FillUpHrefAndTargetFullFileName(List<PackageCard> packageCardsList, string baseHref, string sourceDir, string destDir)
        {
#if DEBUG
            //_logger.Info($"baseHref = '{baseHref}'");
            //_logger.Info($"sourceDir = '{sourceDir}'");
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

            item.Href = Path.Combine(baseHref, preparedName).Replace("\\", "/");
            item.TargetFullFileName = Path.Combine(destDir, preparedName).Replace("\\", "/");
        }

        private static string PrepareName(string initialName)
        {
            var preparedInitialName = initialName.Substring(2).Replace("#", "_").Replace("[", "_").Replace("]", "_").Replace("(", "_").Replace(")", "_").Replace("{", "_").Replace("}", "_").Replace(",", "_").ToLower();

            return $"{preparedInitialName}.html";
        }
    }
}
