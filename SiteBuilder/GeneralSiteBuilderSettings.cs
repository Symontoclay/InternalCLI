using CommonUtils;
using CommonUtils.DebugHelpers;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XMLDocReader;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class GeneralSiteBuilderSettings
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public GeneralSiteBuilderSettings(GeneralSiteBuilderSettingsOptions options)
        {
            var rootPath = EVPath.Normalize(options.RootPath);

#if DEBUG
            //_logger.Info($"GeneralSettings() rootPath = {rootPath}");
#endif

            EVPath.RegVar("SITE_ROOT_PATH", rootPath);

            KindOfTargetUrl = options.KindOfTargetUrl;

#if DEBUG
            //_logger.Info($"GeneralSettings() KindOfTargetUrl = {KindOfTargetUrl}");
#endif

            SiteName = options.SiteName;

#if DEBUG
            //_logger.Info($"SiteName = {SiteName}");
#endif

            switch(KindOfTargetUrl)
            {
                case KindOfTargetUrl.Domain:
                    SiteHref = $"https://{SiteName}";
                    break;

                case KindOfTargetUrl.Localhost:
                    SiteHref = "http://localhost";
                    break;

                case KindOfTargetUrl.Path:
                    SiteHref = rootPath;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(KindOfTargetUrl), KindOfTargetUrl, null);
            }

#if DEBUG
            //_logger.Info($"SiteHref = {SiteHref}");
#endif

            SourcePath = EVPath.Normalize(options.SourcePath);

#if DEBUG
            //_logger.Info($"GeneralSettings() SourcePath = {SourcePath}");
#endif

            EVPath.RegVar("SITE_SOURCE_PATH", SourcePath);

            DestPath = EVPath.Normalize(options.DestPath);

#if DEBUG
            //_logger.Info($"GeneralSettings() DestPath = {DestPath}");
            //_logger.Info($"options.TempPath = {options.TempPath}");
#endif

            EVPath.RegVar("SITE_DEST_PATH", DestPath);

            var initTempPath = EVPath.Normalize(options.TempPath);

#if DEBUG
            //_logger.Info($"initTempPath = {initTempPath}");
#endif

            if (!string.IsNullOrWhiteSpace(initTempPath))
            {
#if DEBUG
                //_logger.Info($"GeneralSettings() initTempPath = {initTempPath}");
#endif

                TempPath = EVPath.Normalize(initTempPath);

                IgnoredDirsList.Add(TempPath);

#if DEBUG
                //_logger.Info($"GeneralSettings() TempPath = {TempPath}");
#endif

                EVPath.RegVar("SITE_TEMP_PATH", TempPath);

                if (!Directory.Exists(TempPath))
                {
                    Directory.CreateDirectory(TempPath);
                }
            }

            ReadSiteSettings();

#if DEBUG
            //_logger.Info($" = {}");
#endif
        }

        public const string IgnoreDestDir = "siteSource";

        public const string IgnoreGitDir = ".git";
        public const string VSDir = ".vs";

        public string SiteName { get; private set; } = string.Empty;
        public string SiteHref { get; private set; } = string.Empty;

        public string SourcePath { get; private set; } = string.Empty;

        public string DestPath { get; private set; } = string.Empty;

        public string TempPath { get; private set; } = string.Empty;

        public List<string> IgnoredDirsList { get; private set; } = new List<string>();

        public SiteInfo SiteSettings { get; private set; }

        public bool UseMinification { get; set; } = false;

        public KindOfTargetUrl KindOfTargetUrl { get; set; }

        public List<SiteElementInfo> SiteElementsList { get; private set; }
        public SiteElementInfo RootCSharpUserApiXMLDocSiteElement { get; private set; }

        public List<RoadMapItem> RoadMapItemsList { get; private set; } = new List<RoadMapItem>();
        public List<ReleaseItem> ReleaseItemsList { get; private set; } = new List<ReleaseItem>();
        public List<PackageCard> CSharpUserApiXMLDocsList { get; private set; } = new List<PackageCard>();
        public Dictionary<string, ICodeDocument> CSharpUserApiXMLDocsCodeDocumentDict = new Dictionary<string, ICodeDocument>();

        private void ReadSiteSettings()
        {
            var tmpSiteSettingsPath = Path.Combine(SourcePath, "site.site");

            SiteSettings = SiteInfo.LoadFromFile(tmpSiteSettingsPath);

#if DEBUG
            //_logger.Info($"SiteSettings = {SiteSettings}");
#endif

            var rootSiteElement = SiteElementInfoReader.Read(SourcePath, DestPath, SiteHref, IgnoredDirsList, new List<string>() { /*".gitignore",*/ "roadMap.json", "site.site" }, this);

#if DEBUG
            //_logger.Info($"rootSiteElement = {rootSiteElement}");
#endif

            SiteElementsList = LinearizeSiteElementInfoTreeWithoutRoot(rootSiteElement);

#if DEBUG
            //_logger.Info($"SiteElementsList.Count = {SiteElementsList.Count}");
            //_logger.Info($"SiteElementsList = {SiteElementsList.WriteListToString()}");
#endif

            if(!string.IsNullOrWhiteSpace(SiteSettings.BaseCSharpUserApiPath))
            {
                var rootCSharpUserApiXMLDocSiteElementFullName = Path.Combine(SiteSettings.BaseCSharpUserApiPath, "index.sp");

#if DEBUG
                //_logger.Info($"rootCSharpUserApiXMLDocSiteElementFullName = {rootCSharpUserApiXMLDocSiteElementFullName}");
#endif

                RootCSharpUserApiXMLDocSiteElement = SiteElementsList.SingleOrDefault(p => p.InitialFullFileName == rootCSharpUserApiXMLDocSiteElementFullName);

#if DEBUG
                //_logger.Info($"RootCSharpUserApiXMLDocSiteElement = {RootCSharpUserApiXMLDocSiteElement}");
#endif
            }

            var roadMapFileName = SiteSettings.RoadMapJsonPath;

            if (!string.IsNullOrWhiteSpace(roadMapFileName))
            {
                RoadMapItemsList = RoadMapReader.ReadAndPrepare(roadMapFileName);
            }

#if DEBUG
            //_logger.Info($"RoadMapItemsList = {RoadMapItemsList.WriteListToString()}");
#endif

            var releaseNotesFileName = SiteSettings.ReleaseNotesJsonPath;

            if(!string.IsNullOrWhiteSpace(releaseNotesFileName))
            {
                ReleaseItemsList = ReleaseItemsReader.Read(releaseNotesFileName, SiteHref, SiteSettings.BaseReleaseNotesPath, SourcePath, DestPath);
            }

#if DEBUG
            //_logger.Info($"ReleaseItemsList = {ReleaseItemsList.WriteListToString()}");
#endif

            ReadCSharpUserApiXMLDocsList();
        }

        private List<SiteElementInfo> LinearizeSiteElementInfoTreeWithoutRoot(SiteElementInfo rootSiteElement)
        {
            var result = new List<SiteElementInfo>();

            LinearizeSiteElementInfoTreeWithoutRoot(rootSiteElement, result);

            return result;
        }

        private void LinearizeSiteElementInfoTreeWithoutRoot(SiteElementInfo siteElement, List<SiteElementInfo> result)
        {
#if DEBUG
            //_logger.Info($"siteElement = {siteElement}");
#endif

            if (siteElement.Kind != KindOfSiteElement.Root && !result.Contains(siteElement))
            {
                result.Add(siteElement);
            }

            foreach (var subItem in siteElement.SubItemsList)
            {
                LinearizeSiteElementInfoTreeWithoutRoot(subItem, result);
            }
        }

        private void ReadCSharpUserApiXMLDocsList()
        {
            var csharpApiJsonPath = SiteSettings.CSharpUserApiJsonPath;

#if DEBUG
            //_logger.Info($"csharpApiJsonPath = {csharpApiJsonPath}");
#endif

            if (string.IsNullOrWhiteSpace(csharpApiJsonPath))
            {
                return;
            }

            var csharpApiOptions = CSharpApiOptions.LoadFromFile(csharpApiJsonPath);

#if DEBUG
            //_logger.Info($"csharpApiOptions = {csharpApiOptions}");
#endif

            var targetSolutionDir = EVPath.Normalize(csharpApiOptions.SolutionDir);

            //_logger.Info($"targetSolutionDir = {targetSolutionDir}");

            if(string.IsNullOrWhiteSpace(targetSolutionDir) || !Directory.Exists(targetSolutionDir))
            {
                targetSolutionDir = EVPath.Normalize(csharpApiOptions.AlternativeSolutionDir);

                if(string.IsNullOrWhiteSpace(targetSolutionDir) || !Directory.Exists(targetSolutionDir))
                {
                    throw new Exception($"Both '{csharpApiOptions.SolutionDir}' and '{csharpApiOptions.AlternativeSolutionDir}' paths are invalid.");
                }                
            }

#if DEBUG
            //_logger.Info($"targetSolutionDir (after) = {targetSolutionDir}");
#endif

            IgnoredDirsList.Add(targetSolutionDir);

            if (!csharpApiOptions.XmlDocFiles.Any())
            {
                throw new Exception("There are not any xml documentation files.");
            }

            var fileNamesList = new List<string>();

            foreach (var xmlDocFile in csharpApiOptions.XmlDocFiles)
            {
                fileNamesList.Add(Path.Combine(targetSolutionDir, xmlDocFile));
            }

            var options = new CSharpXMLDocLoaderOptions()
            {
                XmlFileNamesList = fileNamesList,
                TargetRootTypeNamesList = csharpApiOptions.UnityAssetCoreRootTypes,
                PublicMembersOnly = csharpApiOptions.PublicMembersOnly,
                IgnoreErrors = csharpApiOptions.IgnoreErrors,
                BaseHref = SiteHref,
                BasePath = SiteSettings.BaseCSharpUserApiPath,
                SourceDir = SourcePath,
                DestDir = DestPath
            };

#if DEBUG
            //_logger.Info($"options = {options}");
#endif
            CSharpUserApiXMLDocsList = CSharpXMLDocLoader.Load(options);

#if DEBUG
            //_logger.Info($"CSharpApiXMLDocsList.Count = {CSharpUserApiXMLDocsList.Count}");
#endif

            foreach (var packageCard in CSharpUserApiXMLDocsList)
            {
                foreach (var item in packageCard.InterfacesList)
                {
                    CSharpUserApiXMLDocsCodeDocumentDict[item.InitialName] = item;

                    foreach (var prop in item.PropertiesList)
                    {
                        CSharpUserApiXMLDocsCodeDocumentDict[prop.InitialName] = prop;
                    }

                    foreach (var method in item.MethodsList)
                    {
                        CSharpUserApiXMLDocsCodeDocumentDict[method.InitialName] = method;
                    }
                }

                foreach (var item in packageCard.ClassesList)
                {
                    CSharpUserApiXMLDocsCodeDocumentDict[item.InitialName] = item;

                    foreach (var prop in item.PropertiesList)
                    {
                        CSharpUserApiXMLDocsCodeDocumentDict[prop.InitialName] = prop;
                    }

                    foreach (var method in item.MethodsList)
                    {
                        CSharpUserApiXMLDocsCodeDocumentDict[method.InitialName] = method;
                    }
                }

                foreach (var item in packageCard.EnumsList)
                {
                    CSharpUserApiXMLDocsCodeDocumentDict[item.InitialName] = item;
                }
            }
        }
    }
}
