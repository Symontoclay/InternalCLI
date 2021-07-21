using HtmlAgilityPack;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.ReleaseInfoGeneration
{
    public class ReleaseInfoGenerator
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static readonly CultureInfo _targetCulture = new CultureInfo("en-GB");

        public static string Run(string initialContent, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //_logger.Info($"initialContent = {initialContent}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            ProcessNodes(doc.DocumentNode, doc, generalSiteBuilderSettings);

            return doc.ToHtmlString();
        }

        private static void ProcessNodes(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //if (rootNode.Name != "#document")
            //{
            //    _logger.Info($"rootNode.Name = '{rootNode.Name}'");
            //    _logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //    _logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //    _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}
#endif

            if (rootNode.Name == "release")
            {
                ProcessReleaseInfo(rootNode, doc, generalSiteBuilderSettings);
                return;
            }

            if (rootNode.Name == "prev_releases")
            {
                ProcessPrevReleasesList(rootNode, doc, generalSiteBuilderSettings);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc, generalSiteBuilderSettings);
            }
        }

        private static void ProcessPrevReleasesList(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var newRootNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRootNode, rootNode);

            var prevReleasesList = generalSiteBuilderSettings.ReleaseItemsList.Where(p => !p.IsLatest).OrderByDescending(p => p.Date).ToList();

            if(!prevReleasesList.Any())
            {
                return;
            }

            var sb = new StringBuilder();

            sb.AppendLine($"<h2>Previous versions:</h2>");

            foreach(var releaseInfo in prevReleasesList)
            {
                sb.AppendLine($"<div><a href='{releaseInfo.Href}'>{releaseInfo.Version}</a>&nbsp;&nbsp;<i class='far fa-calendar-alt'></i>&nbsp;&nbsp;{releaseInfo.Date.Value.ToString("D", _targetCulture)}</div>");
            }

            newRootNode.InnerHtml = sb.ToString();
        }

        private static void ProcessReleaseInfo(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var version = rootNode.GetAttributeValue("version", string.Empty);

            if(string.IsNullOrWhiteSpace(version))
            {
                version = "latest";
            }

#if DEBUG
            //_logger.Info($"version = '{version}'");
#endif

            var targetItem = GetTargetReleaseItem(version, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"targetItem = {targetItem}");
#endif

            var newRootNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRootNode, rootNode);

            var sb = new StringBuilder();

            var latestMark = string.Empty;

            if(targetItem.IsLatest)
            {
                latestMark = " (latest)";
            }

            sb.AppendLine($"<h1>Version {targetItem.Version}{latestMark}</h1>");
            sb.AppendLine($"<div><i class='far fa-calendar-alt'></i>&nbsp;&nbsp;{targetItem.Date.Value.ToString("D", _targetCulture)}</div>");

            sb.AppendLine("<table class='downloads-block'>");

            foreach(var asset in targetItem.AssetsList)
            {
                sb.AppendLine("<tr style='border-bottom: solid 1px #e2e2e2;'>");
                sb.AppendLine("<td style='width: 180px;'>");
                sb.AppendLine(KindOfReleaseAssetItemHelper.GetAssetTitle(asset.Kind, asset.Href));
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine($"<a href='{asset.Href}'>{KindOfReleaseAssetItemHelper.GetAssetTitle(asset.Kind, asset.Href)}</a>");
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            sb.AppendLine($"<h2>Release notes</h2>");

            sb.AppendLine($"<div>{ContentPreprocessor.Run(targetItem.Description, targetItem.IsMarkdown, generalSiteBuilderSettings)}</div>");

            newRootNode.InnerHtml = sb.ToString();
        }

        private static ReleaseItem GetTargetReleaseItem(string version, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            if(version == "latest")
            {
                return generalSiteBuilderSettings.ReleaseItemsList.Single(p => p.IsLatest);
            }

            return generalSiteBuilderSettings.ReleaseItemsList.Single(p => p.Version == version);
        }
    }
}
