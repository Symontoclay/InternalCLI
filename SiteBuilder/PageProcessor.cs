using CommonMark;
using dotless.Core;
using dotless.Core.Parser.Tree;
using HtmlAgilityPack;
using NLog;
using SiteBuilder.HtmlPreprocessors;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using SiteBuilder.HtmlPreprocessors.EBNF;
using SiteBuilder.HtmlPreprocessors.InThePageContentGen;
using SiteBuilder.HtmlPreprocessors.RoadmapGeneration;
using SiteBuilder.HtmlPreprocessors.ShortTags;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder
{
    public class PageProcessor
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static readonly List<string> _commonCssLinksList = new List<string>();
        private static readonly List<string> _commonJsLinksList = new List<string>();

        private static readonly CultureInfo _targetCulture = new CultureInfo("en-GB");

        static PageProcessor()
        {
#if DEBUG
            //_logger.Info("Begin");
#endif

            SetUpFontAwesome();
        }

        private static void SetUpFontAwesome()
        {
            _commonJsLinksList.Add("<script src='/assets/fontawesome_5.15.1/all.js'></script>");
        }

        public PageProcessor(SiteElementInfo siteElement, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            _siteElement = siteElement;
            _generalSiteBuilderSettings = generalSiteBuilderSettings;
        }

        private readonly SiteElementInfo _siteElement;
        private readonly GeneralSiteBuilderSettings _generalSiteBuilderSettings;

        public SiteElementInfo SiteElementInfo => _siteElement;

        public void Run()
        {
#if DEBUG
            //_logger.Info("Begin");
#endif

            var sitePageInfo = _siteElement.SitePageInfo;

            if (!sitePageInfo.IsReady)
            {
                return;
            }

            ProcessCssOrLess();

            CreateFile();

#if DEBUG
            //_logger.Info($" = {}");
            //_logger.Info("End");
#endif
        }

        private StringBuilder mResult;
        private bool _addOwnCss;

        private void ProcessCssOrLess()
        {
            if(!_siteElement.AddCssToPage)
            {
                return;
            }

            var targetFile = _siteElement.InitiallCssFullFileName;

#if DEBUG
            //_logger.Info($"targetFile = {targetFile}");
#endif

            var content = File.ReadAllText(targetFile);

            if(string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            _addOwnCss = true;

            if (_siteElement.ProcessLessForPage)
            {
                content = Less.Parse(content);
            }

            File.WriteAllText(_siteElement.TargetCssFullFileName, content);
        }

        private void CreateFile()
        {
            mResult = new StringBuilder();
            GenerateText();
            var content = HrefsNormalizer.FillAppDomainNameInHrefs(mResult.ToString(), _siteElement, _generalSiteBuilderSettings);

            using var textWriter = new StreamWriter(_siteElement.TargetFullFileName, false, new UTF8Encoding(false));
            textWriter.Write(content);
            textWriter.Flush();

#if DEBUG
            //_logger.Info($"_siteElement.TargetFullFileName = {_siteElement.TargetFullFileName}");
#endif
        }

        private void GenerateText()
        {
            var sitePageInfo = _siteElement.SitePageInfo;
            
            AppendLine("<!DOCTYPE html>");
            AppendLine("<html lang='en' xmlns='http://www.w3.org/1999/xhtml' prefix='og: https://ogp.me/ns#'>");
            AppendLine("<head>");
            AppendLine("<meta charset='utf-8' />");
            AppendLine("<meta name='generator' content='SymOntoClay/InternalCLI:SiteBuilder'>");
            AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1'>");

            GenerateMicrodata();

            var title = GetTitle();

            if (!string.IsNullOrWhiteSpace(title))
            {
                Append("<title>");
                Append(title);
                AppendLine("</title>");
            }

            if (_generalSiteBuilderSettings.SiteSettings.EnableFavicon)
            {
                //AppendLine("<link rel='shortcut icon' href='/favicon.ico' type='image/x-icon'>");
                AppendLine("<link rel='icon' href='/favicon.png' type='image/png'>");
            }

            GenerateCommonLinks();
   
            AppendLine("<link rel='stylesheet' href='/site.css'>");

            if (_siteElement.AdditionalMenu != null)
            {
                AppendLine("<link rel='stylesheet' href='/sym-onto-clay-menu.css'>");
            }

            if(_addOwnCss)
            {
                AppendLine($"<link rel='stylesheet' href='{_siteElement.CssHrefForPage}'>");
            }

            if (sitePageInfo.EnableMathML)
            {
                AppendLine("<script src='https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js'></script>");
            }

            //GenerateGoogleAnalytics();

            AppendLine("</head>");
            AppendLine("<body>");

            AppendLine("<header role='banner'>");
            //GenerateMainWarning();
            GenerateHeader();
            AppendLine("</header>");
            AppendLine("<nav role='navigation'>");
            GenerateMainMenu();
            AppendLine("</nav>");
            GenerateMainSeparatorLine();
            
            AppendLine("<nav role='navigation'>");
            GenerateBreadcrumbs();
            AppendLine("</nav>");

            GenerateDisclaimer();

            if (_siteElement.AdditionalMenu == null)
            {
                GenerateArticle();
            }
            else
            {
                //AppendLine("<div class='container-fluid'>");
                //AppendLine("<div class='row'>");
                //AppendLine("<div class='col col-md-3 my-menu-col'>");
                GenerateAdditionalMenu();
                //AppendLine("</div>");
                //AppendLine("<div class='col col-md-9'>");
                GenerateArticle();
            }

            GenerateMainSeparatorLine();
            GenerateFooter();

            AppendLine("</body>");
            AppendLine("</html>");

#if DEBUG
            //_logger.Info($" = {}");
#endif

            //throw new NotImplementedException();
        }

        private void GenerateDisclaimer()
        {
            if (_siteElement.SitePageInfo.ShowDisclaimer ?? true)
            {
                AppendLine(ShortTagsPreparation.GetDisclaimerHtml());
            }            

            //<a target="_blank" href="https://icons8.com/icon/EggHJUeUuU6C/general-warning-sign">Warning</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
        }
        //AppendLine("<>");
        private void GenerateFooter()
        {
            AppendLine("</br>");
            AppendLine("<footer class='container' role='contentinfo'>");
            AppendLine("<span><a href='https://github.com/Symontoclay'><i class='fab fa-github' title='SymOntoClay on GitHub'></i></a></span>");
            //AppendLine("<span><a href='https://www.youtube.com/channel/UCgw9QmyKGZQXQyugbzCstZA'><i class='fab fa-youtube' title='SymOntoClay on Youtube'></i></a></span>");
            AppendLine("<span><a href='https://github.com/Symontoclay/SymOntoClay/discussions'><i class='far fa-comments' title='Discussions'></i></a></span>");
            AppendLine("</br>");
            AppendLine("</br>");
            AppendLine("<a href='you-need-to-know.html'>You need to know</a>");
            AppendLine("</br>");
            AppendLine("<a href='privacy.html'>Privacy Policy</a>");
            AppendLine("</br>");
            AppendLine("</br>");
            AppendLine($"This page was last modified on {_siteElement.LastUpdateDate.ToString("dd MMMM yyyy", _targetCulture)}</br>");
            //Append(", at ");
            //Append(LastUpdateDate.ToString("HH:mm", tmpFormat));

            AppendLine($"&copy;&nbsp; <a href='https://github.com/metatypeman'>Sergiy Tolkachov</a> {GetCopyRightDate()}</br>");
            AppendLine("The text is available under the <a href='https://creativecommons.org/licenses/by-sa/4.0/'>Creative Commons Attribution-ShareAlike 4.0 International License By Sa</a>&nbsp;<i class='fab fa-creative-commons'></i><i class='fab fa-creative-commons-by'></i><i class='fab fa-creative-commons-sa'></i>");
            AppendLine("</br>&nbsp;");
            AppendLine("</footer>");
        }

        private void GenerateMainSeparatorLine()
        {
            AppendLine("<hr style='border-bottom-color: #e2e2e2;'>");
        }

        private void GenerateCommonLinks()
        {
            foreach (var commonCssLink in _commonCssLinksList)
            {
                AppendLine(commonCssLink);
            }

            foreach (var commonJsLink in _commonJsLinksList)
            {
                AppendLine(commonJsLink);
            }
        }

        private string GetCopyRightDate()
        {
            var initYear = 2020;
            var currentYear = DateTime.Today.Year;

            if (currentYear == initYear)
            {
                return initYear.ToString();
            }

            return $"{initYear} - {currentYear}";
        }

        private void GenerateMicrodata()
        {
            var globalMictodata = _generalSiteBuilderSettings.SiteSettings.Microdata;

            var sitePageInfo = _siteElement.SitePageInfo;
            var siteMictodata = sitePageInfo.Microdata;

            AppendLine("<meta property='og:type' content='article' />");

            if (!string.IsNullOrWhiteSpace(siteMictodata?.Title))
            {
                AppendLine($"<meta property='og:title' content='{siteMictodata.Title}' />");
                //AppendLine($"<meta itemprop='name' content='{MicrodataTitle}' />");
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(globalMictodata?.Title))
                {
                    AppendLine($"<meta property='og:title' content='{globalMictodata.Title}' />");
                }
            }

            var needAdditionalImageSettings = false;

            if (!string.IsNullOrWhiteSpace(siteMictodata?.ImageUrl))
            {
                var imageUrl = PagesPathsHelper.RelativeHrefToAbsolute(siteMictodata.ImageUrl, _generalSiteBuilderSettings);

                AppendLine($"<meta property='og:image' content='{imageUrl}' />");
                AppendLine($"<link rel='image_src' href='{imageUrl}' />");
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(globalMictodata?.ImageUrl))
                {
                    var imageUrl = PagesPathsHelper.RelativeHrefToAbsolute(globalMictodata.ImageUrl, _generalSiteBuilderSettings);

                    AppendLine($"<meta property='og:image' content='{imageUrl}' />");
                    AppendLine($"<link rel='image_src' href='{imageUrl}' />");
                }
            }

            if(needAdditionalImageSettings)
            {
                //AppendLine("<meta property='og:image:type' content='image/png'>");
                //AppendLine("<meta property='og:image:width' content='300'>");
                //AppendLine("<meta property='og:image:height' content='300'>");
            }

            if (!string.IsNullOrWhiteSpace(siteMictodata?.ImageAlt))
            {
                AppendLine($"<meta property='og:image:alt' content='{siteMictodata.ImageAlt}' />");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(globalMictodata?.ImageAlt))
                {
                    AppendLine($"<meta property='og:image:alt' content='{globalMictodata.ImageAlt}' />");
                }
            }

            AppendLine($"<meta property='og:url' content='{_siteElement.Href}' />");

            if (!string.IsNullOrWhiteSpace(siteMictodata?.Description))
            {
                AppendLine($"<meta name='description' content='{siteMictodata.Description}'/>");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(globalMictodata?.Description))
                {
                    AppendLine($"<meta name='description' content='{globalMictodata.Description}'/>");
                }
            }
        }

        private void GenerateGoogleAnalytics()
        {
            var tmpGAScript = new StringBuilder();
            tmpGAScript.AppendLine("<!-- Global site tag (gtag.js) - Google Analytics -->");
            tmpGAScript.AppendLine("<script async src='https://www.googletagmanager.com/gtag/js?id=G-4T85EVM2WV'></script>");
            tmpGAScript.AppendLine("<script>");
            tmpGAScript.AppendLine("  window.dataLayer = window.dataLayer || [];");
            tmpGAScript.AppendLine("  function gtag(){dataLayer.push(arguments);}");
            tmpGAScript.AppendLine("  gtag('js', new Date());");
            tmpGAScript.AppendLine("  gtag('config', 'G-4T85EVM2WV');");
            tmpGAScript.AppendLine("</script>");

            AppendLine(tmpGAScript.ToString());
        }

        protected void GenerateArticle()
        {
            var content = GetContent();

#if DEBUG
            //_logger.Info($"content = {content}");
#endif

            AppendLine("<article>");
            AppendLine(content);
            AppendLine("</article>");
            AppendLine("<p>");
            AppendLine("&nbsp;");
            AppendLine("</p>");
        }

        private void GenerateHeader()
        {
            //PrintUkrainianFlag();

            Append("<p>");

            if (!string.IsNullOrWhiteSpace(_generalSiteBuilderSettings.SiteSettings.Logo))
            {
                Append("<a href = '/'>");
                Append("<img src='");
                Append(_generalSiteBuilderSettings.SiteSettings.Logo);
                Append("' style='margin-top: -12px;'>");
                Append("</a>");
                Append("&nbsp;");
            }

            Append("<span style='font-size: 30px; font-weight: bold;'>");
            Append("SymOntoClay");
            Append("</span>");
            Append("&nbsp;");
            Append("<span>");
            Append("is game AI experimental open source engine.");
            Append("</span>");
            AppendLine("</p>");
        }

        private void PrintUkrainianFlag()
        {
            Append("<div style='background-color: #005BBB;'>");
            Append("&nbsp;");
            AppendLine("</div>");
            Append("<div style='background-color: #FFD500;'>");
            Append("&nbsp;");
            AppendLine("</div>");
        }

        private void GenerateMainMenu()
        {
            var tmpItems = new List<string>();

            foreach (var item in _generalSiteBuilderSettings.SiteSettings.Menu.Items)
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");
#endif

                var tmpSb = new StringBuilder();

                if (item.Items.Any())
                {
                    var tmpId = $"id{Guid.NewGuid():D}";
                    tmpSb.Append("<div class='dropdown' style='display:inline;'>");
                    tmpSb.Append($"<button class='btn dropdown-toggle' type='button' id='{tmpId}' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>");
                    tmpSb.Append(item.Label);
                    tmpSb.Append("</button>");
                    tmpSb.Append($"<div class='dropdown-menu' aria-labelledby='{tmpId}'>");

                    foreach (var subItem in item.Items)
                    {
                        tmpSb.Append($"<a class='dropdown-item a_ddi' href='{subItem.Href}' style='color: #007bff;'>{subItem.Label}</a>");
                    }

                    tmpSb.Append("</div>");
                    tmpSb.Append("</div>");
                    /*
                    
  
    Dropdown
  
  
    
    <button class="dropdown-item" type="button">Another action</button>
    <button class="dropdown-item" type="button">Something else here</button>
  

                    */
                }
                else
                {
                    tmpSb.Append("<a href ='");
                    tmpSb.Append(item.Href);
                    tmpSb.Append("'>");
                    tmpSb.Append(item.Label);
                    tmpSb.Append("</a>");
                }

                tmpItems.Add(tmpSb.ToString());
                tmpItems.Add("&nbsp;|&nbsp;");
            }

            tmpItems.RemoveAt(tmpItems.Count - 1);

            foreach (var item in tmpItems)
            {
                Append(item);
            }
        }

        private void GenerateAdditionalMenu()
        {
            GenerateAdditionalMenuRunItems(_siteElement.AdditionalMenu.Items, false);
        }

        private int mN = 0;

        private void GenerateAdditionalMenuRunItems(List<MenuItem> items, bool isChild)
        {
            foreach (var item in items)
            {
                if (item.Items == null || item.Items.Count == 0)
                {
                    if (isChild)
                    {
                        AppendLine($"<ul class='my-second-menu-item'><li><a href='{item.Href}'>{item.Label}</a></li></ul>");
                    }
                    else
                    {
                        AppendLine($"<p class='my-root-menu-item'><a href='{item.Href}'>{item.Label}</a></p>");
                    }
                }
                else
                {
                    if (isChild)
                    {
                        AppendLine("<ul class='my-second-menu-item'><li>");
                        AppendLine($"<p class='my-second-menu-label'>{item.Label}</p>");
                        GenerateAdditionalMenuRunItems(item.Items, true);
                        AppendLine("</li></ul>");
                    }
                    else
                    {
                        mN++;

                        AppendLine($"<div class='panel-group myslim-panel-group' id='accordion_{mN}' role='tablist' aria-multiselectable='true'>");
                        AppendLine("<div class='panel panel-default' style='margin-bottom: 5px;'>");
                        AppendLine($"<div class='panel-heading myslim-panel-heading' role='tab' id='headingOne_{mN}'>");
                        AppendLine($"<a class='myslim-panel-button' role='button' data-toggle='collapse' data-parent='#accordion_{mN}' href='#collapseOne_{mN}' aria-expanded='false' aria-controls='collapseOne_{mN}'>");
                        AppendLine($"&#9660;&nbsp;<span style='font-weight: bold;'>{item.Label}</span>");
                        AppendLine("</a>");
                        AppendLine("</div>");
                        AppendLine($"<div id='collapseOne_{mN}' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingOne_{mN}'>");
                        AppendLine("<div class='panel-body myslim-panel-body'>");
                        GenerateAdditionalMenuRunItems(item.Items, true);
                        AppendLine("</div>");
                        AppendLine("</div>");
                        AppendLine("</div>");
                        AppendLine("</div>");
                    }
                }
            }
        }

        private void GenerateBreadcrumbs()
        {
#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"GenerateBreadcrumbs SourceName = {SourceName}");
#endif
            var breadcrumbsItem = _siteElement;

            var isFirst = true;

            var itemsList = new List<BreadcrumbInThePage>();

            do
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"GenerateBreadcrumbs isFirst = {isFirst} breadcrumbsItem = {breadcrumbsItem}");
#endif

                var item = new BreadcrumbInThePage();
                item.Title = breadcrumbsItem.BreadcrumbTitle;

                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    item.Href = breadcrumbsItem.Href;
                }

                itemsList.Add(item);
            }
            while ((breadcrumbsItem = breadcrumbsItem.Parent) != null);

            itemsList.Reverse();

            var n = 0;

            foreach (var item in itemsList)
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"GenerateBreadcrumbs item = {item}");
#endif
                n++;
                Append("<a");
                if (!string.IsNullOrWhiteSpace(item.Href))
                {
                    Append($" href = '{item.Href}'");
                }
                Append(" style='color: #C0C0C0;'>");
                Append(item.Title);
                Append("</a>");

                if (n < itemsList.Count)
                {
                    Append("&nbsp;/&nbsp;");
                }
            }
        }

        protected void Append(string val)
        {
            if (_generalSiteBuilderSettings.UseMinification)
            {
                val = val.Trim();
            }
            mResult.Append(val);
        }

        protected void AppendLine(string val)
        {
            if (_generalSiteBuilderSettings.UseMinification)
            {
                Append(val);
                return;
            }

            mResult.AppendLine(val);
        }

        protected virtual string GetInitialContent()
        {
            return File.ReadAllText(_siteElement.THtmlFullFileName);
        }

        private string GetContent()
        {
            var content = GetInitialContent();

#if DEBUG
            //_logger.Info($"content = {content}");
#endif

            return ContentPreprocessor.Run(content, _siteElement.SitePageInfo.UseMarkdown, _generalSiteBuilderSettings);
        }

        private string GetTitle()
        {
            var sitePageInfo = _siteElement.SitePageInfo;

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_generalSiteBuilderSettings.SiteSettings.MainTitle))
            {
                sb.Append(_generalSiteBuilderSettings.SiteSettings.MainTitle);

                if (!string.IsNullOrWhiteSpace(_generalSiteBuilderSettings.SiteSettings.TitlesDelimiter))
                {
                    sb.Append(_generalSiteBuilderSettings.SiteSettings.TitlesDelimiter);
                }
            }

            if (string.IsNullOrWhiteSpace(sitePageInfo.Title))
            {
                sb.Append(_generalSiteBuilderSettings.SiteSettings.Title);
            }
            else
            {
                sb.Append(sitePageInfo.Title);
            }

            return sb.ToString().Trim();
        }
    }
}
