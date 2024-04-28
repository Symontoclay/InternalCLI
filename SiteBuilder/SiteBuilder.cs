using dotless.Core;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class SiteBuilder
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public SiteBuilder(SiteBuilderOptions options)
        {
            _generalSiteBuilderSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions
            {
                KindOfTargetUrl = options.KindOfTargetUrl,
                SiteName = options.SiteName,
                SourcePath = options.SourcePath,
                DestPath = options.DestPath,
                TempPath = options.TempPath
            });
        }

        private readonly GeneralSiteBuilderSettings _generalSiteBuilderSettings;

        public void Run()
        {
#if DEBUG
            //_logger.Info("Begin");
#endif

            _hrefsList = new List<string>();

            ClearDir();

#if DEBUG
            //_logger.Info($"GeneralSettings.IgnoredDirsList = {JsonConvert.SerializeObject(GeneralSettings.IgnoredDirsList, Formatting.Indented)}");
#endif

            ProcessReleaseNotesPages();
            ProcessCSharpUserApiXMLDocsList();

            ProcessSiteElementsList(_generalSiteBuilderSettings.SiteElementsList);

            GenerateSiteMap();

#if DEBUG
            //_logger.Info("End");
#endif
        }

        private List<string> _hrefsList;

        private void ProcessCSharpUserApiXMLDocsList()
        {
            foreach (var packageCard in _generalSiteBuilderSettings.CSharpUserApiXMLDocsList)
            {
#if DEBUG
                //_logger.Info($"packageCard = {packageCard.ToBriefString()}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.InterfacesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.ClassesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.EnumsList.Count}");
#endif

                foreach(var item in packageCard.InterfacesList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    _hrefsList.Add(item.Href);

                    var fileInfo = new FileInfo(item.TargetFullFileName);

                    if(!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }

                    var pageProcessor = new InterfaceCSharpUserApiXMLDocPageProcessor(item, _generalSiteBuilderSettings);
                    pageProcessor.Run();

                    ProcessCSharpUserApiXMLDocsClassCardMembersList(item, pageProcessor.SiteElementInfo);
                }

                foreach(var item in packageCard.ClassesList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    _hrefsList.Add(item.Href);

                    var fileInfo = new FileInfo(item.TargetFullFileName);

                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }

                    var pageProcessor = new ClassCSharpUserApiXMLDocPageProcessor(item, _generalSiteBuilderSettings);
                    pageProcessor.Run();

                    ProcessCSharpUserApiXMLDocsClassCardMembersList(item, pageProcessor.SiteElementInfo);
                }

                foreach (var item in packageCard.EnumsList)
                {
#if DEBUG
                    //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                    //_logger.Info($"item.Href = {item.Href}");
                    //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                    _hrefsList.Add(item.Href);

                    var fileInfo = new FileInfo(item.TargetFullFileName);

                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }

                    var pageProcessor = new EnumCSharpUserApiXMLDocPageProcessor(item, _generalSiteBuilderSettings);
                    pageProcessor.Run();
                }
            }
        }

        private void ProcessCSharpUserApiXMLDocsClassCardMembersList(ClassCard classCard, SiteElementInfo parent)
        {
            foreach (var prop in classCard.PropertiesList)
            {
#if DEBUG
                //_logger.Info($"prop.Name.InitialName = {prop.Name.InitialName}");
                //_logger.Info($"prop.Name.FullName = {prop.Name.FullName}");
                //_logger.Info($"prop.Href = {prop.Href}");
                //_logger.Info($"prop.TargetFullFileName = {prop.TargetFullFileName}");
#endif

                _hrefsList.Add(prop.Href);

                var pageProcessor = new PropertyCSharpUserApiXMLDocPageProcessor(prop, parent, _generalSiteBuilderSettings);
                pageProcessor.Run();
            }

            foreach (var constructor in classCard.ConstructorsList)
            {
#if DEBUG
                //_logger.Info($"constructor.Name.InitialName = {constructor.Name.InitialName}");
                //_logger.Info($"constructor.Name.FullName = {constructor.Name.FullName}");
                //_logger.Info($"constructor.Href = {constructor.Href}");
                //_logger.Info($"constructor.TargetFullFileName = {constructor.TargetFullFileName}");
#endif

                _hrefsList.Add(constructor.Href);

                var pageProcessor = new ConstructorCSharpUserApiXMLDocPageProcessor(constructor, parent, _generalSiteBuilderSettings);
                pageProcessor.Run();
            }

            foreach (var method in classCard.MethodsList)
            {
#if DEBUG
                //_logger.Info($"method.Name.InitialName = {method.Name.InitialName}");
                //_logger.Info($"method.Name.FullName = {method.Name.FullName}");
                //_logger.Info($"method.Href = {method.Href}");
                //_logger.Info($"method.TargetFullFileName = {method.TargetFullFileName}");
#endif

                _hrefsList.Add(method.Href);


                var pageProcessor = new MethodCSharpUserApiXMLDocPageProcessor(method, parent, _generalSiteBuilderSettings);
                pageProcessor.Run();
            }
        }

        private void ProcessReleaseNotesPages()
        {
            foreach(var releaseItem in _generalSiteBuilderSettings.ReleaseItemsList)
            {
#if DEBUG
                //_logger.Info($"releaseItem = {releaseItem}");
#endif

                var fileInfo = new FileInfo(releaseItem.TargetFullFileName);

                if(!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                _hrefsList.Add(releaseItem.Href);

                var pageProcessor = new ReleaseItemPageProcessor(releaseItem, _generalSiteBuilderSettings);
                pageProcessor.Run();
            }
        }

        private void GenerateSiteMap()
        {
            _hrefsList = _hrefsList.Distinct().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            sb.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>");
            foreach (var href in _hrefsList)
            {
#if DEBUG
                //_logger.Info($"href = '{href}'");
#endif

                //var uriBuilder = new UriBuilder();
                //uriBuilder.Scheme = "https";
                //uriBuilder.Host = GeneralSettings.SiteName;
                //uriBuilder.Path = page.RelativeHref;

                //var lastMod = DateTime.Now;

                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{href}</loc>");
                //sb.AppendLine($"<lastmod>{lastMod.ToString("yyyy-MM-dd")}</lastmod>");
                //sb.AppendLine("<changefreq>always</changefreq>");
                //sb.AppendLine("<priority>0.8</priority>");
                sb.AppendLine("</url>");
            }
            sb.AppendLine("</urlset>");

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"PredictionProcessingOfConcreteDir sb.ToString() = {sb.ToString()}");
#endif

            var newPath = Path.Combine(_generalSiteBuilderSettings.DestPath, "sitemap.xml");

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"PredictionProcessingOfConcreteDir newPath = {newPath}");
#endif

            using var tmpTextWriter = new StreamWriter(newPath, false, new UTF8Encoding(true));
            tmpTextWriter.Write(sb.ToString());
            tmpTextWriter.Flush();
        }

        private void ProcessSiteElementsList(List<SiteElementInfo> siteElementsList)
        {
            foreach(var siteElement in siteElementsList)
            {
#if DEBUG
                //_logger.Info($"siteElement = {siteElement}");
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
                            var pageProcessor = new PageProcessor(siteElement, _generalSiteBuilderSettings);
                            pageProcessor.Run();

                            _hrefsList.Add(siteElement.Href);
                        }
                        break;

                    case KindOfSiteElement.File:
                        if(siteElement.InitialFullFileName.EndsWith(".console"))
                        {
                            break;
                        }

                        File.Copy(siteElement.InitialFullFileName, siteElement.TargetFullFileName);
                        break;

                    case KindOfSiteElement.Less:
                        {
                            var lessContent = File.ReadAllText(siteElement.InitialFullFileName);

#if DEBUG
                            //_logger.Info($"lessContent = {lessContent}");
#endif

                            var cssContent = Less.Parse(lessContent);

#if DEBUG
                            //_logger.Info($"cssContent = {cssContent}");
#endif

                            File.WriteAllText(siteElement.TargetFullFileName, cssContent);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        private void ClearDir()
        {
#if DEBUG
            //_logger.Info($"GeneralSettings.DestPath = {GeneralSettings.DestPath}");
#endif

            var dirs = Directory.GetDirectories(_generalSiteBuilderSettings.DestPath);

            foreach (var subDir in dirs)
            {
                var tmpDirInfo = new DirectoryInfo(subDir);

                if (tmpDirInfo.Name == GeneralSiteBuilderSettings.IgnoreSourceDir)
                {
                    continue;
                }

                if (tmpDirInfo.Name == GeneralSiteBuilderSettings.IgnoreGitDir)
                {
                    continue;
                }

                if (tmpDirInfo.Name == GeneralSiteBuilderSettings.VSDir)
                {
                    continue;
                }

                tmpDirInfo.Delete(true);
            }

            var files = Directory.GetFiles(_generalSiteBuilderSettings.DestPath);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            if (!string.IsNullOrWhiteSpace(_generalSiteBuilderSettings.TempPath))
            {
                if (Directory.Exists(_generalSiteBuilderSettings.TempPath))
                {
                    files = Directory.GetFiles(_generalSiteBuilderSettings.TempPath);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
