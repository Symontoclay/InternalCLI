using HtmlAgilityPack;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteBuilder
{
    public static class HrefsNormalizer
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string FillAppDomainNameInHrefs(string content, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            return FillAppDomainNameInHrefs(content, null, generalSiteBuilderSettings);
        }

        public static string FillAppDomainNameInHrefs(string content, SiteElementInfo siteElement, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            FillAppDomainNameInHrefs(doc.DocumentNode, siteElement, generalSiteBuilderSettings);

            var strWriter = new StringWriter();
            doc.Save(strWriter);

            return strWriter.ToString();
        }

        private static void FillAppDomainNameInHrefs(HtmlNode rootNode, SiteElementInfo siteElement, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //_logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
#endif

            var href = rootNode.GetAttributeValue("href", null);

            if (!string.IsNullOrWhiteSpace(href))
            {
                if (!href.StartsWith("https://") && !href.StartsWith("http://") && !href.StartsWith("file://"))
                {
#if DEBUG
                    //_logger.Info($"href = {href}");
#endif

                    href = GetAbcoluteHref(href, siteElement, generalSiteBuilderSettings);

#if DEBUG
                    //_logger.Info($"href (after) = {href}");
#endif

                    rootNode.SetAttributeValue("href", href);
                }
            }

            var src = rootNode.GetAttributeValue("src", null);

            if (!string.IsNullOrWhiteSpace(src))
            {
                if (!src.StartsWith("https://") && !src.StartsWith("http://") && !src.StartsWith("file://"))
                {
#if DEBUG
                    //_logger.Info($"src = {src}");
#endif

                    src = GetAbcoluteHref(src, siteElement, generalSiteBuilderSettings);

#if DEBUG
                    //_logger.Info($"src (after) = {src}");
#endif

                    rootNode.SetAttributeValue("src", src);
                }
            }

            foreach (var child in rootNode.ChildNodes.ToList())
            {
                FillAppDomainNameInHrefs(child, siteElement, generalSiteBuilderSettings);
            }
        }

        private static string GetAbcoluteHref(string href, SiteElementInfo siteElement, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            if (href.StartsWith("#"))
            {
                if(string.IsNullOrWhiteSpace(siteElement.Href))
                {
                    return href;
                }

                return $"{siteElement.Href}{href}";
            }
            else
            {
                if (href.StartsWith("/"))
                {
                    return $"{generalSiteBuilderSettings.SiteHref}{href}";
                }
                else
                {
                    return $"{generalSiteBuilderSettings.SiteHref}/{href}";
                }
            }
        }
    }
}
