using CommonUtils.DebugHelpers;
using dotless.Core;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

#if DEBUG
            _logger.Info($"GeneralSettings.IgnoredDirsList = {JsonConvert.SerializeObject(GeneralSettings.IgnoredDirsList, Formatting.Indented)}");
#endif

            var rootSiteElement = SiteElementInfoReader.Read(GeneralSettings.SourcePath, GeneralSettings.DestPath, GeneralSettings.SiteHref, GeneralSettings.IgnoredDirsList, new List<string>() { ".gitignore", "roadMap.json", "site.site" });

#if DEBUG
            //_logger.Info($"rootSiteElement = {rootSiteElement}");
#endif

            var siteElementsList = LinearizeSiteElementInfoTreeWithoutRoot(rootSiteElement);

#if DEBUG
            _logger.Info($"siteElementsList.Count = {siteElementsList.Count}");
            //_logger.Info($"siteElementsList = {siteElementsList.WriteListToString()}");
#endif

            ProcessSiteElementsList(siteElementsList);

            GenerateSiteMap(siteElementsList);

#if DEBUG
            _logger.Info("End");
#endif
        }

        private void GenerateSiteMap(List<SiteElementInfo> siteElementsList)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            sb.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>");
            foreach (var siteElement in siteElementsList.Where(p => p.Kind == KindOfSiteElement.Page))
            {
                //var uriBuilder = new UriBuilder();
                //uriBuilder.Scheme = "https";
                //uriBuilder.Host = GeneralSettings.SiteName;
                //uriBuilder.Path = page.RelativeHref;

                //var lastMod = DateTime.Now;

                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{siteElement.Href}</loc>");
                //sb.AppendLine($"<lastmod>{lastMod.ToString("yyyy-MM-dd")}</lastmod>");
                //sb.AppendLine("<changefreq>always</changefreq>");
                //sb.AppendLine("<priority>0.8</priority>");
                sb.AppendLine("</url>");
            }
            sb.AppendLine("</urlset>");

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"PredictionProcessingOfConcreteDir sb.ToString() = {sb.ToString()}");
#endif

            var newPath = Path.Combine(GeneralSettings.DestPath, "sitemap.xml");

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"PredictionProcessingOfConcreteDir newPath = {newPath}");
#endif

            using (var tmpTextWriter = new StreamWriter(newPath, false, new UTF8Encoding(true)))
            {
                tmpTextWriter.Write(sb.ToString());
                tmpTextWriter.Flush();
            }
        }

        private void ProcessSiteElementsList(List<SiteElementInfo> siteElementsList)
        {
            foreach(var siteElement in siteElementsList)
            {
#if DEBUG
                _logger.Info($"siteElement = {siteElement}");
#endif

                if (!Directory.Exists(siteElement.DirectoryName))
                {
                    Directory.CreateDirectory(siteElement.DirectoryName);
                }

                var kind = siteElement.Kind;

                switch(kind)
                {
                    case KindOfSiteElement.Page:
                        {
                            var pageProcessor = new PageProcessor(siteElement);
                            pageProcessor.Run();
                        }
                        break;

                    case KindOfSiteElement.File:
                        File.Copy(siteElement.InitialFullFileName, siteElement.TargetFullFileName);
                        break;

                    case KindOfSiteElement.Less:
                        {
                            var lessContent = File.ReadAllText(siteElement.InitialFullFileName);

#if DEBUG
                            _logger.Info($"lessContent = {lessContent}");
#endif

                            var cssContent = Less.Parse(lessContent);

#if DEBUG
                            _logger.Info($"cssContent = {cssContent}");
#endif

                            File.WriteAllText(siteElement.TargetFullFileName, cssContent);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
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

            if(siteElement.Kind != KindOfSiteElement.Root && !result.Contains(siteElement))
            {
                result.Add(siteElement);
            }

            foreach(var subItem in siteElement.SubItemsList)
            {
                LinearizeSiteElementInfoTreeWithoutRoot(subItem, result);
            }
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
