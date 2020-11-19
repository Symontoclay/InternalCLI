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

            FillUpHrefAndTargetFullFileName(packageCardsList, options.BaseHref, options.BasePath, options.SourceDir, options.DestDir);

            return packageCardsList;
        }

        private static void FillUpHrefAndTargetFullFileName(List<PackageCard> packageCardsList, string baseHref, string basePath, string sourceDir, string destDir)
        {
            var relativePath = basePath.Replace(sourceDir, string.Empty);

#if DEBUG
            //_logger.Info($"relativePath = {relativePath}");
#endif

            foreach (var packageCard in packageCardsList)
            {
#if DEBUG
                //_logger.Info($"packageCard = {packageCard.ToBriefString()}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.InterfacesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.ClassesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.EnumsList.Count}");
#endif

                foreach (var item in packageCard.InterfacesList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    var relativeItemName = Path.Combine(relativePath, $"{item.Name.FullName.ToLower()}.html");

#if DEBUG
                    //_logger.Info($"relativeItemName = {relativeItemName}");
#endif

                    item.Href = Path.Combine(baseHref, relativeItemName).Replace("\\", "/");
                    item.TargetFullFileName = Path.Combine(destDir, relativeItemName).Replace("\\", "/");

#if DEBUG
                    //_logger.Info($"item.Href (after) = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName (after) = {item.TargetFullFileName}");
#endif

                    foreach(var prop in item.PropertiesList)
                    {
#if DEBUG
                        //_logger.Info($"prop.Name.InitialName = {prop.Name.InitialName}");
                        //_logger.Info($"prop.Name.FullName = {prop.Name.FullName}");
                        //_logger.Info($"prop.Href = {prop.Href}");
                        //_logger.Info($"prop.TargetFullFileName = {prop.TargetFullFileName}");
#endif

                        var relativeName = GetRelativeName(relativePath, prop.Name.InitialName);

#if DEBUG
                        //_logger.Info($"relativeName = {relativeName}");
#endif

                        prop.Href = Path.Combine(baseHref, relativeName).Replace("\\", "/");
                        prop.TargetFullFileName = Path.Combine(destDir, relativeName).Replace("\\", "/");

#if DEBUG
                        //_logger.Info($"prop.Href (after) = {prop.Href}");
                        //_logger.Info($"prop.TargetFullFileName (after) = {prop.TargetFullFileName}");
#endif
                    }

                    foreach (var method in item.MethodsList)
                    {
#if DEBUG
                        //_logger.Info($"method.Name.InitialName = {method.Name.InitialName}");
                        //_logger.Info($"method.Name.FullName = {method.Name.FullName}");
                        //_logger.Info($"method.Href = {method.Href}");
                        //_logger.Info($"method.TargetFullFileName = {method.TargetFullFileName}");
#endif

                        var relativeName = GetRelativeName(relativePath, method.Name.InitialName);

#if DEBUG
                        //_logger.Info($"relativeName = {relativeName}");
#endif

                        method.Href = Path.Combine(baseHref, relativeName).Replace("\\", "/");
                        method.TargetFullFileName = Path.Combine(destDir, relativeName).Replace("\\", "/");

#if DEBUG
                        //_logger.Info($"method.Href (after) = {method.Href}");
                        //_logger.Info($"method.TargetFullFileName (after) = {method.TargetFullFileName}");
#endif
                    }
                }

                foreach (var item in packageCard.ClassesList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    var relativeItemName = Path.Combine(relativePath, $"{item.Name.FullName.ToLower()}.html");

#if DEBUG
                    //_logger.Info($"relativeItemName = {relativeItemName}");
#endif

                    item.Href = Path.Combine(baseHref, relativeItemName).Replace("\\", "/");
                    item.TargetFullFileName = Path.Combine(destDir, relativeItemName).Replace("\\", "/");

#if DEBUG
                    //_logger.Info($"item.Href (after) = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName (after) = {item.TargetFullFileName}");
#endif

                    foreach (var prop in item.PropertiesList)
                    {
#if DEBUG
                        //_logger.Info($"prop.Name.InitialName = {prop.Name.InitialName}");
                        //_logger.Info($"prop.Name.FullName = {prop.Name.FullName}");
                        //_logger.Info($"prop.Href = {prop.Href}");
                        //_logger.Info($"prop.TargetFullFileName = {prop.TargetFullFileName}");
#endif

                        var relativeName = GetRelativeName(relativePath, prop.Name.InitialName);

#if DEBUG
                        //_logger.Info($"relativeName = {relativeName}");
#endif

                        prop.Href = Path.Combine(baseHref, relativeName).Replace("\\", "/");
                        prop.TargetFullFileName = Path.Combine(destDir, relativeName).Replace("\\", "/");

#if DEBUG
                        //_logger.Info($"prop.Href (after) = {prop.Href}");
                        //_logger.Info($"prop.TargetFullFileName (after) = {prop.TargetFullFileName}");
#endif
                    }

                    foreach (var method in item.MethodsList)
                    {
#if DEBUG
                        //_logger.Info($"method.Name.InitialName = {method.Name.InitialName}");
                        //_logger.Info($"method.Name.FullName = {method.Name.FullName}");
                        //_logger.Info($"method.Href = {method.Href}");
                        //_logger.Info($"method.TargetFullFileName = {method.TargetFullFileName}");
#endif

                        var relativeName = GetRelativeName(relativePath, method.Name.InitialName);

#if DEBUG
                        //_logger.Info($"relativeName = {relativeName}");
#endif

                        method.Href = Path.Combine(baseHref, relativeName).Replace("\\", "/");
                        method.TargetFullFileName = Path.Combine(destDir, relativeName).Replace("\\", "/");

#if DEBUG
                        //_logger.Info($"method.Href (after) = {method.Href}");
                        //_logger.Info($"method.TargetFullFileName (after) = {method.TargetFullFileName}");
#endif
                    }
                }

                foreach (var item in packageCard.EnumsList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    var relativeItemName = Path.Combine(relativePath, $"{item.Name.FullName.ToLower()}.html");

#if DEBUG
                    //_logger.Info($"relativeItemName = {relativeItemName}");
#endif

                    item.Href = Path.Combine(baseHref, relativeItemName).Replace("\\", "/");
                    item.TargetFullFileName = Path.Combine(destDir, relativeItemName).Replace("\\", "/");

#if DEBUG
                    //_logger.Info($"item.Href (after) = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName (after) = {item.TargetFullFileName}");
#endif
                }
            }
        }

        private static string GetRelativeName(string relativePath, string initialName)
        {
            var preparedInitialName = initialName.Substring(2).Replace("(", "_").Replace(")", "_").Replace("{", "_").Replace("}", "_").Replace(",", "_").ToLower();

#if DEBUG
            //_logger.Info($"preparedInitialName = {preparedInitialName}");
#endif

            return Path.Combine(relativePath, $"{preparedInitialName}.html");
        }
    }
}
