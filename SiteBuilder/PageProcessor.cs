using CommonMark;
using NLog;
using SiteBuilder.HtmlPreprocessors.EBNF;
using SiteBuilder.HtmlPreprocessors.InThePageContentGen;
using SiteBuilder.HtmlPreprocessors.ShortTags;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder
{
    public class PageProcessor
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private readonly CultureInfo TargetCulture = new CultureInfo("en-GB");

        public PageProcessor(SiteElementInfo siteElement)
        {
            _siteElement = siteElement;
        }

        private readonly SiteElementInfo _siteElement;

        public void Run()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            var sitePageInfo = _siteElement.SitePageInfo;

            if (!sitePageInfo.IsReady)
            {
                return;
            }

            CreateFile();
        }

        private StringBuilder mResult;

        private void CreateFile()
        {
            mResult = new StringBuilder();
            GenerateText();

            using (var textWriter = new StreamWriter(_siteElement.TargetFullFileName, false, new UTF8Encoding(false)))
            {
                textWriter.Write(mResult.ToString());
                textWriter.Flush();
            }

#if DEBUG
            _logger.Info($"_siteElement.TargetFullFileName = {_siteElement.TargetFullFileName}");

            //if (_siteElement.TargetFullFileName.EndsWith("spec.html"))
            //{
            //    throw new NotImplementedException();
            //}
#endif
        }

        private void GenerateText()
        {
            var sitePageInfo = _siteElement.SitePageInfo;
            var siteMictodata = sitePageInfo.Microdata;

            AppendLine("<!DOCTYPE html>");
            AppendLine("<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>");
            AppendLine("<head>");
            AppendLine("<meta charset='utf-8' />");
            AppendLine("<meta name='generator' content='SymOntoClay/InternalCLI:SiteBuilder'>");
            AppendLine("<meta property='og:type' content='article' />");
            AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1'>");

            if (!string.IsNullOrWhiteSpace(siteMictodata.Title))
            {
                AppendLine($"<meta property='og:title' content='{siteMictodata.Title}' />");
                //AppendLine($"<meta itemprop='name' content='{MicrodataTitle}' />");
            }

            if (!string.IsNullOrWhiteSpace(siteMictodata.ImageUrl))
            {
                AppendLine($"<meta property='og:image' content='{PagesPathsHelper.RelativeHrefToAbsolute(siteMictodata.ImageUrl)}' />");
                //AppendLine($"<link rel='\"image_src\" href=\"{PagesPathsHelper.RelativeHrefToAbsolute(ImageUrl)}\" />");
                //AppendLine("<meta property='og:image:type' content='image/png'>");
                //AppendLine("<meta property='og:image:width' content='300'>");
                //AppendLine("<meta property='og:image:height' content='300'>");
                if (!string.IsNullOrWhiteSpace(siteMictodata.ImageAlt))
                {
                    //AppendLine($"<meta property='og:image:alt' content='{ImageAlt}' />");
                }
            }

            AppendLine($"<meta property='og:url' content='{_siteElement.Href}' />");

            if (!string.IsNullOrWhiteSpace(siteMictodata.Description))
            {
                Append("<meta name='description' content='");
                Append(siteMictodata.Description);
                AppendLine("'>");
            }

            var title = GetTitle();

            if (!string.IsNullOrWhiteSpace(title))
            {
                Append("<title>");
                Append(title);
                AppendLine("</title>");
            }

            if (GeneralSettings.SiteSettings.EnableFavicon)
            {
                //AppendLine("<link rel='shortcut icon' href='/favicon.ico' type='image/x-icon'>");
                AppendLine("<link rel='icon' href='/favicon.png' type='image/png'>");
            }

            AppendLine("<script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script>");
            AppendLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js' integrity='sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1' crossorigin='anonymous'></script>");
            AppendLine("<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js' integrity='sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM' crossorigin='anonymous'></script>");

            //AppendLine("<link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' integrity='sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u' crossorigin='anonymous'>");
            //AppendLine("<link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css' integrity='sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp' crossorigin='anonymous'>");
            AppendLine("<link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' integrity='sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T' crossorigin='anonymous'>");
            AppendLine("<link rel='stylesheet' href='/site.css'>");

            if (_siteElement.AdditionalMenu != null)
            {
                AppendLine("<link rel='stylesheet' href='/gnu-clay-menu.css'>");
            }

            AppendLine("<script src='https://code.jquery.com/jquery-3.2.1.js'></script>");
            AppendLine("<script src='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js' integrity='sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa' crossorigin='anonymous'></script>");
            AppendLine("<script src='https://kit.fontawesome.com/78db54b323.js'></script>");

            if (sitePageInfo.EnableMathML)
            {
                AppendLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.5.3/MathJax.js?config=TeX-AMS-MML_HTMLorMML'></script>");
            }

            //var tmpGAScript = new StringBuilder();

            //tmpGAScript.Append("<script>");
            //tmpGAScript.Append("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){");
            //tmpGAScript.Append("(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),");
            //tmpGAScript.Append("m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)");
            //tmpGAScript.Append("})(window,document,'script','//www.google-analytics.com/analytics.js','ga');");
            //tmpGAScript.Append("ga('create', 'UA-73880715-1', 'auto');");
            //tmpGAScript.Append("ga('send', 'pageview');");
            //tmpGAScript.Append("</script>");

            //AppendLine(tmpGAScript.ToString());

            AppendLine("</head>");
            AppendLine("<body>");
            AppendLine("<div class='container main-container'>");
            AppendLine("<div class='row justify-content-center'>");
            AppendLine("<div class='col col-md-10'>");
            AppendLine("<header role='banner'>");
            //GenerateMainWarning();
            GenerateHeader();
            AppendLine("</header>");
            AppendLine("<nav role='navigation'>");
            GenerateMainMenu();
            AppendLine("</nav>");
            AppendLine("<hr style='border-bottom-color: #e2e2e2;'>");
            AppendLine("<nav class='bread-crumb-nav'>");
            GenerateBreadcrumbs();
            AppendLine("</nav>");
            AppendLine("</div>");
            AppendLine("</div>");

            AppendLine("<div class='row justify-content-center'>");
            AppendLine("<div class='col col-md-10'>");

            if (_siteElement.AdditionalMenu == null)
            {
                GenerateArticle();
            }
            else
            {
                AppendLine("<div class='container-fluid'>");
                AppendLine("<div class='row'>");
                AppendLine("<div class='col col-md-3 my-menu-col'>");
                GenerateAdditionalMenu();
                AppendLine("</div>");
                AppendLine("<div class='col col-md-9'>");
                GenerateArticle();
                AppendLine("</div>");
                AppendLine("</div>");
                AppendLine("</div>");
            }

            AppendLine("</div>");
            AppendLine("</div>");
            AppendLine("</div>");

            AppendLine("<footer class='container' role='contentinfo'>");
            AppendLine("<div class='row justify-content-center'>");
            AppendLine("<div class='col col-md-10'>");
            AppendLine($"This page was last modified on {_siteElement.LastUpdateDate.ToString("dd MMMM yyyy", TargetCulture)}</br>");
            //Append(", at ");
            //Append(LastUpdateDate.ToString("HH:mm", tmpFormat));

            AppendLine($"&copy;&nbsp; <a href='https://github.com/metatypeman'>metatypeman</a> {GetCopyRightDate()}</br>");
            AppendLine("The text is available under the <a href='https://creativecommons.org/licenses/by-sa/3.0/'>Creative Commons Attribution-ShareAlike 3.0 Unported License</a>");
            AppendLine("</div>");
            AppendLine("</div>");
            AppendLine("</footer>");

            AppendLine("</body>");
            AppendLine("</html>");

#if DEBUG
            //_logger.Info($" = {}");
#endif

            //throw new NotImplementedException();
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

        protected void GenerateArticle()
        {
            var content = GetContent();

#if DEBUG
            _logger.Info($"content = {content}");
#endif

            AppendLine("<article>");
            AppendLine(content);
            AppendLine("</article>");
        }

        private void GenerateHeader()
        {
            Append("<p>");

            if (!string.IsNullOrWhiteSpace(GeneralSettings.SiteSettings.Logo))
            {
                Append("<a href = '/'>");
                Append("<img src='");
                Append(GeneralSettings.SiteSettings.Logo);
                Append("' style='margin-top: -12px;'>");
                Append("</a>");
                Append("&nbsp;");
            }

            Append("<span style='font-size: 30px; font-weight: bold;'>");
            Append("SymOntoClay");
            Append("</span>");
            Append("&nbsp;");
            Append("<span>");
            Append("is game AI experimental open source indie-framework with its own DSL.");
            Append("</span>");
            AppendLine("</p>");
        }

        private void GenerateMainWarning()
        {
            AppendLine("<div class='not-suitable-danger'>");
            AppendLine("<i class='fa fa-exclamation-triangle' aria-hidden='true' style='font-size:20px;'></i> <span class='not-suitable-danger-text'>This project is experimental. And it is not suitable for practical using.</span>");
            AppendLine("</div>");
        }

        private void GenerateMainMenu()
        {
            var tmpItems = new List<string>();

            foreach (var item in GeneralSettings.SiteSettings.Menu.Items)
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");
#endif

                var tmpSb = new StringBuilder();

                if (item.Items.Any())
                {
                    var tmpId = $"id{Guid.NewGuid().ToString("D")}";
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
            if (GeneralSettings.UseMinification)
            {
                val = val.Trim();
            }
            mResult.Append(val);
        }

        protected void AppendLine(string val)
        {
            if (GeneralSettings.UseMinification)
            {
                Append(val);
                return;
            }

            mResult.AppendLine(val);
        }

        private string GetContent()
        {
            var content = File.ReadAllText(_siteElement.THtmlFullFileName);

#if DEBUG
            _logger.Info($"content = {content}");
#endif

            if(_siteElement.SitePageInfo.UseMarkdown)
            {
                return CommonMarkConverter.Convert(content);
            }

            return PreprocessContent(content);
        }

        private string PreprocessContent(string initialContent)
        {
            var content = EBNFPreparation.Run(initialContent);


#if DEBUG
            _logger.Info($"content (1) = {content}");
#endif

            content = ShortTagsPreparation.Run(content);

#if DEBUG
            _logger.Info($"content (2) = {content}");
#endif

            return InThePageContentGenerator.Run(content);
        }

        private string GetTitle()
        {
            var sitePageInfo = _siteElement.SitePageInfo;

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(GeneralSettings.SiteSettings.MainTitle))
            {
                sb.Append(GeneralSettings.SiteSettings.MainTitle);

                if (!string.IsNullOrWhiteSpace(GeneralSettings.SiteSettings.TitlesDelimiter))
                {
                    sb.Append(GeneralSettings.SiteSettings.TitlesDelimiter);
                }
            }

            if (string.IsNullOrWhiteSpace(sitePageInfo.Title))
            {
                sb.Append(GeneralSettings.SiteSettings.Title);
            }
            else
            {
                sb.Append(sitePageInfo.Title);
            }

            return sb.ToString().Trim();
        }
    }
}
