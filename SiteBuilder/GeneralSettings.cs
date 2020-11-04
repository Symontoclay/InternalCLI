using CommonUtils;
using CommonUtils.DebugHelpers;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace SiteBuilder
{
    public static class GeneralSettings
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static GeneralSettings()
        {
            SiteName = ConfigurationManager.AppSettings["siteName"];

#if DEBUG
            _logger.Info($"SiteName = {SiteName}");
#endif

            var rootPath = ConfigAppSettingsHelper.GetExistingDirectoryName("rootPath");

#if DEBUG
            _logger.Info($"GeneralSettings() rootPath = {rootPath}");
#endif

            EVPath.RegVar("SITE_ROOT_PATH", rootPath);

            SourcePath = EVPath.Normalize(ConfigurationManager.AppSettings["sourcePath"]);

#if DEBUG
            _logger.Info($"GeneralSettings() SourcePath = {SourcePath}");
#endif

            EVPath.RegVar("SITE_SOURCE_PATH", SourcePath);

            DestPath = EVPath.Normalize(ConfigurationManager.AppSettings["destPath"]);

#if DEBUG
            _logger.Info($"GeneralSettings() DestPath = {DestPath}");
            _logger.Info($"ConfigurationManager.AppSettings['tempPath'] = {ConfigurationManager.AppSettings["tempPath"]}");
#endif

            EVPath.RegVar("SITE_DEST_PATH", DestPath);

            var initTempPath = ConfigurationManager.AppSettings["tempPath"];

#if DEBUG
            _logger.Info($"initTempPath = {initTempPath}");
#endif

            if (!string.IsNullOrWhiteSpace(initTempPath))
            {
#if DEBUG
                _logger.Info($"GeneralSettings() initTempPath = {initTempPath}");
#endif

                TempPath = EVPath.Normalize(initTempPath);

#if DEBUG
                _logger.Info($"GeneralSettings() TempPath = {TempPath}");
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

        public static string SiteName { get; private set; } = string.Empty;

        public static string SourcePath { get; private set; } = string.Empty;

        public static string DestPath { get; private set; } = string.Empty;

        public static string TempPath { get; private set; } = string.Empty;

        public static SiteInfo SiteSettings { get; private set; }

        public static bool UseMinification { get; set; } = false;

        public static List<RoadMapItem> RoadMapItemsList { get; private set; } = new List<RoadMapItem>();
        public static List<ReleaseItem> ReleaseItemsList { get; private set; } = new List<ReleaseItem>();

        private static void ReadSiteSettings()
        {
            var tmpSiteSettingsPath = Path.Combine(SourcePath, "site.site");

            SiteSettings = SiteInfo.LoadFromFile(tmpSiteSettingsPath);

#if DEBUG
            _logger.Info($"SiteSettings = {SiteSettings}");
#endif

            var roadMapFileName = SiteSettings.RoadMapJsonPath;

            if (!string.IsNullOrWhiteSpace(roadMapFileName))
            {
                RoadMapItemsList = RoadMapReader.ReadAndPrepare(roadMapFileName);
            }

#if DEBUG
            _logger.Info($"RoadMapItemsList = {RoadMapItemsList.WriteListToString()}");
#endif

            var releaseNotesFileName = SiteSettings.ReleaseNotesJsonPath;

            if(!string.IsNullOrWhiteSpace(releaseNotesFileName))
            {
                ReleaseItemsList = ReleaseItemsReader.Read(releaseNotesFileName);
            }

#if DEBUG
            _logger.Info($"ReleaseItemsList = {ReleaseItemsList.WriteListToString()}");
#endif

            var csharpApiJsonPath = SiteSettings.CSharpApiJsonPath;

            if (!string.IsNullOrWhiteSpace(csharpApiJsonPath))
            {
                throw new NotImplementedException();
            }
        }
    }
}
