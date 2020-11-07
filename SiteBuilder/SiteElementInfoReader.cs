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
    public static class SiteElementInfoReader
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static SiteElementInfo Read(string sourceDir, string destDir, string siteHref, List<string> forbidenDirectoriesList, List<string> forbidenFileNamesList)
        {
#if DEBUG
            //_logger.Info($"sourceDir = {sourceDir}");
            _logger.Info($"siteHref = {siteHref}");
#endif

            var result = new SiteElementInfo() { Kind = KindOfSiteElement.Root };

            ProcessDirectory(sourceDir, result, sourceDir, destDir, siteHref, forbidenDirectoriesList, forbidenFileNamesList);

#if DEBUG
            //_logger.Info($" = {}");
#endif

            return result;
        }

        private static void ProcessDirectory(string directory, SiteElementInfo parent, string sourceDir, string destDir, string siteHref, List<string> forbidenDirectoriesList, List<string> forbidenFileNamesList)
        {
#if DEBUG
            //_logger.Info($"sourceDir = {sourceDir}");
            //_logger.Info($"destDir = {destDir}");
            //_logger.Info($"directory = {directory}");
            //_logger.Info($"siteName = {siteName}");
            //_logger.Info($"parent = {parent?.ToBriefString()}");
#endif

            var fileNamesList = Directory.EnumerateFiles(directory);

#if DEBUG
            _logger.Info($"fileNamesList = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif

            var indexSpFileName = fileNamesList.Where(p => p.EndsWith("index.sp")).SingleOrDefault();

#if DEBUG
            _logger.Info($"indexSpFileName = {indexSpFileName}");
#endif

            if(!string.IsNullOrWhiteSpace(indexSpFileName))
            {
                var indexTHtmlFileName = fileNamesList.Where(p => p.EndsWith("index.thtml")).Single();

#if DEBUG
                _logger.Info($"indexTHtmlFileName = {indexTHtmlFileName}");
#endif

                var cssFileName = fileNamesList.Where(p => p.EndsWith("index.css")).SingleOrDefault();

#if DEBUG
                _logger.Info($"cssFileName = {cssFileName}");
#endif

                if(!string.IsNullOrWhiteSpace(cssFileName))
                {
                    fileNamesList = fileNamesList.Except(new List<string> { cssFileName });
                }

                var lessFileName = fileNamesList.Where(p => p.EndsWith("index.less")).SingleOrDefault();

#if DEBUG
                _logger.Info($"lessFileName = {lessFileName}");
#endif

                if (!string.IsNullOrWhiteSpace(lessFileName))
                {
                    fileNamesList = fileNamesList.Except(new List<string> { lessFileName });
                }

                if (!string.IsNullOrWhiteSpace(cssFileName) && !string.IsNullOrWhiteSpace(lessFileName))
                {
                    throw new Exception($"Ambiguous resolving '{cssFileName}' and '{lessFileName}'.");
                }

                fileNamesList = fileNamesList.Except(new List<string> { indexSpFileName, indexTHtmlFileName });

#if DEBUG
                _logger.Info($"fileNamesList (2) = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif

                var oldParent = parent;
                parent = new SiteElementInfo() { Kind = KindOfSiteElement.Page, Parent = oldParent };
                oldParent.SubItemsList.Add(parent);

                parent.InitialFullFileName = indexSpFileName;
                parent.THtmlFullFileName = indexTHtmlFileName;

                var sitePageInfo = SitePageInfo.LoadFromFile(indexSpFileName);

                parent.SitePageInfo = sitePageInfo;

                if (!string.IsNullOrWhiteSpace(sitePageInfo.AdditionalMenu))
                {
                    parent.AdditionalMenu = MenuInfo.GetMenu(sitePageInfo.AdditionalMenu);
                }

                parent.BreadcrumbTitle = sitePageInfo.BreadcrumbTitle;

                if (string.IsNullOrWhiteSpace(parent.BreadcrumbTitle))
                {
                    parent.BreadcrumbTitle = sitePageInfo.Title;
                }

                var relativePath = indexSpFileName.Replace(sourceDir, string.Empty).Replace(".sp", ".html").Trim();

#if DEBUG
                _logger.Info($"relativePath = {relativePath}");
#endif

                var targetPath = Path.Combine(destDir, relativePath);

#if DEBUG
                _logger.Info($"targetPath = {targetPath}");
#endif

                parent.TargetFullFileName = targetPath;

                var fileTargetInfo = new FileInfo(targetPath);

                parent.DirectoryName = fileTargetInfo.DirectoryName;

                parent.Href = $"{siteHref}/{relativePath}";

                if(!string.IsNullOrWhiteSpace(cssFileName) || !string.IsNullOrWhiteSpace(lessFileName))
                {
                    if(!string.IsNullOrWhiteSpace(cssFileName))
                    {
                        parent.AddCssToPage = true;
                        parent.InitiallCssFullFileName = cssFileName;

                        var cssRelativePath = cssFileName.Replace(sourceDir, string.Empty).Trim();

                        parent.TargetCssFullFileName = Path.Combine(destDir, cssRelativePath);
                        parent.CssHrefForPage = $"{siteHref}/{cssRelativePath}";
                    }

                    if(!string.IsNullOrWhiteSpace(lessFileName))
                    {
                        parent.AddCssToPage = true;
                        parent.ProcessLessForPage = true;
                        parent.InitiallCssFullFileName = lessFileName;

                        var cssRelativePath = lessFileName.Replace(sourceDir, string.Empty).Replace(".less", ".css").Trim();

                        parent.TargetCssFullFileName = Path.Combine(destDir, cssRelativePath);
                        parent.CssHrefForPage = $"{siteHref}/{cssRelativePath}";
                    }
                }

#if DEBUG
                _logger.Info($"oldParent = {oldParent}");
                _logger.Info($"parent (after) = {parent}");
#endif
            }

            var spFileNamesList = fileNamesList.Where(p => p.EndsWith(".sp")).ToList();

#if DEBUG
            //_logger.Info($"spFileNamesList = {JsonConvert.SerializeObject(spFileNamesList, Formatting.Indented)}");
#endif

            if(spFileNamesList.Any())
            {
                var exceptFileNamesList = new List<string>();

                foreach(var spFileName in spFileNamesList)
                {
                    var fileInfo = new FileInfo(spFileName);

#if DEBUG
                    //_logger.Info($"fileInfo.Name = {fileInfo.Name}");
#endif

                    var tHtmlFileName = spFileName.Replace(fileInfo.Name, fileInfo.Name.Replace(".sp", ".thtml"));

#if DEBUG
                    //_logger.Info($"tHtmlFileName = {tHtmlFileName}");
#endif

                    if(!File.Exists(tHtmlFileName))
                    {
                        throw new FileNotFoundException(null, tHtmlFileName);
                    }

                    exceptFileNamesList.Add(spFileName);
                    exceptFileNamesList.Add(tHtmlFileName);

                    var item = new SiteElementInfo() { Kind = KindOfSiteElement.Page };
                    item.Parent = parent;
                    parent.SubItemsList.Add(item);

                    item.InitialFullFileName = spFileName;
                    item.THtmlFullFileName = tHtmlFileName;

                    var sitePageInfo = SitePageInfo.LoadFromFile(spFileName);

                    item.SitePageInfo = sitePageInfo;

                    if (!string.IsNullOrWhiteSpace(sitePageInfo.AdditionalMenu))
                    {
                        item.AdditionalMenu = MenuInfo.GetMenu(sitePageInfo.AdditionalMenu);
                    }

                    item.BreadcrumbTitle = sitePageInfo.BreadcrumbTitle;

                    if(string.IsNullOrWhiteSpace(item.BreadcrumbTitle))
                    {
                        item.BreadcrumbTitle = sitePageInfo.Title;
                    }

                    var relativePath = spFileName.Replace(sourceDir, string.Empty).Replace(".sp", ".html").Trim();

#if DEBUG
                    //_logger.Info($"relativePath = {relativePath}");
#endif

                    var targetPath = Path.Combine(destDir, relativePath);

#if DEBUG
                    //_logger.Info($"targetPath = {targetPath}");
#endif

                    item.TargetFullFileName = targetPath;

                    var fileTargetInfo = new FileInfo(targetPath);

                    item.DirectoryName = fileTargetInfo.DirectoryName;

                    item.Href = $"{siteHref}/{relativePath}";

                    var cssFileName = spFileName.Replace(fileInfo.Name, fileInfo.Name.Replace(".sp", ".css"));

                    if(fileNamesList.Contains(cssFileName))
                    {
                        exceptFileNamesList.Add(cssFileName);
                    }
                    else
                    {
                        cssFileName = null;
                    }

                    var lessFileName = spFileName.Replace(fileInfo.Name, fileInfo.Name.Replace(".sp", ".less"));

                    if (fileNamesList.Contains(lessFileName))
                    {
                        exceptFileNamesList.Add(lessFileName);
                    }
                    else
                    { 
                        lessFileName = null;
                    }

                    if (!string.IsNullOrWhiteSpace(cssFileName) && !string.IsNullOrWhiteSpace(lessFileName))
                    {
                        throw new Exception($"Ambiguous resolving '{cssFileName}' and '{lessFileName}'.");
                    }

                    if (!string.IsNullOrWhiteSpace(cssFileName) || !string.IsNullOrWhiteSpace(lessFileName))
                    {
                        if (!string.IsNullOrWhiteSpace(cssFileName))
                        {
                            item.AddCssToPage = true;
                            item.InitiallCssFullFileName = cssFileName;

                            var cssRelativePath = cssFileName.Replace(sourceDir, string.Empty).Trim();

                            item.TargetCssFullFileName = Path.Combine(destDir, cssRelativePath);
                            item.CssHrefForPage = $"{siteHref}/{cssRelativePath}";
                        }

                        if (!string.IsNullOrWhiteSpace(lessFileName))
                        {
                            item.AddCssToPage = true;
                            item.ProcessLessForPage = true;
                            item.InitiallCssFullFileName = lessFileName;

                            var cssRelativePath = lessFileName.Replace(sourceDir, string.Empty).Replace(".less", ".css").Trim();

                            item.TargetCssFullFileName = Path.Combine(destDir, cssRelativePath);
                            item.CssHrefForPage = $"{siteHref}/{cssRelativePath}";
                        }
                    }

#if DEBUG
                    _logger.Info($"item = {item}");
#endif
                }

                fileNamesList = fileNamesList.Except(exceptFileNamesList);

#if DEBUG
                //_logger.Info($"fileNamesList (3) = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif
            }

            foreach(var fileName in fileNamesList)
            {
                if(forbidenFileNamesList.Any(p => fileName.EndsWith(p)))
                {
                    continue;
                }

                var isLessFile = false;

                if (fileName.EndsWith(".less"))
                {
                    isLessFile = true;
                }

#if DEBUG
                _logger.Info($"fileName = {fileName}");
                _logger.Info($"isLessFile = {isLessFile}");
#endif

                var item = new SiteElementInfo();

                if(isLessFile)
                {
                    item.Kind = KindOfSiteElement.Less;
                }
                else
                {
                    item.Kind = KindOfSiteElement.File;
                }

                item.Parent = parent;
                parent.SubItemsList.Add(item);

                item.InitialFullFileName = fileName;

                var relativePath = fileName.Replace(sourceDir, string.Empty).Trim();

                if (isLessFile)
                {
                    relativePath = relativePath.Replace(".less", ".css");
                }

#if DEBUG
                _logger.Info($"relativePath = {relativePath}");
#endif

                var targetPath = Path.Combine(destDir, relativePath);

#if DEBUG
                _logger.Info($"targetPath = {targetPath}");
#endif

                item.TargetFullFileName = targetPath;

                var fileInfo = new FileInfo(targetPath);

                item.DirectoryName = fileInfo.DirectoryName;

                item.Href = $"{siteHref}/{relativePath}";

#if DEBUG
                _logger.Info($"item = {item}");
#endif
            }

            var subDirsList = Directory.EnumerateDirectories(directory).Where(p => !forbidenDirectoriesList.Contains(p)).ToList();

            foreach(var subDir in subDirsList)
            {
                ProcessDirectory(subDir, parent, sourceDir, destDir, siteHref, forbidenDirectoriesList, forbidenFileNamesList);
            }
        }
    }
}
