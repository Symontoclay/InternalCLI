using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder
{
    public class SiteBuilder
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public void Run()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            ClearDir();

            var rootSiteElement = SiteElementInfoReader.Read(GeneralSettings.SourcePath, GeneralSettings.DestPath, GeneralSettings.SiteName, new List<string>() { GeneralSettings.TempPath }, new List<string>() { ".gitignore", "roadMap.json", "site.site" });

#if DEBUG
            _logger.Info($"rootSiteElement = {rootSiteElement}");
#endif

#if DEBUG
            //_logger.Info($" = {}");
#endif

            throw new NotImplementedException();
        }

        private void ClearDir()
        {
#if DEBUG
            _logger.Info($"GeneralSettings.DestPath = {GeneralSettings.DestPath}");
#endif

            var dirs = Directory.GetDirectories(GeneralSettings.DestPath);

            foreach (var subDir in dirs)
            {
                var tmpDirInfo = new DirectoryInfo(subDir);

                if (tmpDirInfo.Name == GeneralSettings.IgnoreDestDir)
                {
                    continue;
                }

                if (tmpDirInfo.Name == GeneralSettings.IgnoreGitDir)
                {
                    continue;
                }

                if (tmpDirInfo.Name == GeneralSettings.VSDir)
                {
                    continue;
                }

                tmpDirInfo.Delete(true);
            }

            var files = Directory.GetFiles(GeneralSettings.DestPath);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            if (!string.IsNullOrWhiteSpace(GeneralSettings.TempPath))
            {
                if (Directory.Exists(GeneralSettings.TempPath))
                {
                    files = Directory.GetFiles(GeneralSettings.TempPath);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
