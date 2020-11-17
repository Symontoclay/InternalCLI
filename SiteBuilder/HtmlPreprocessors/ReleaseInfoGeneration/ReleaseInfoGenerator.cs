using HtmlAgilityPack;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static string Run(string initialContent)
        {
#if DEBUG
            _logger.Info($"initialContent = {initialContent}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            ProcessNodes(doc.DocumentNode, doc);

            return doc.ToHtmlString();
        }

        private static void ProcessNodes(HtmlNode rootNode, HtmlDocument doc)
        {
#if DEBUG
            if (rootNode.Name != "#document")
            {
                _logger.Info($"rootNode.Name = '{rootNode.Name}'");
                _logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
                _logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
                _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            }
#endif

            if (rootNode.Name == "release")
            {
                ProcessReleaseInfo(rootNode, doc);
                return;
            }

            if (rootNode.Name == "prev_releases")
            {
                ProcessPrevReleasesList(rootNode, doc);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc);
            }
        }

        private static void ProcessPrevReleasesList(HtmlNode rootNode, HtmlDocument doc)
        {
            var newRootNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRootNode, rootNode);

            var prevReleasesList = GeneralSettings.ReleaseItemsList.Where(p => !p.IsLatest).OrderBy(p => p.Date).ToList();

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

            //throw new NotImplementedException();

            newRootNode.InnerHtml = sb.ToString();
        }

        private static void ProcessReleaseInfo(HtmlNode rootNode, HtmlDocument doc)
        {
            var version = rootNode.GetAttributeValue("version", string.Empty);

            if(string.IsNullOrWhiteSpace(version))
            {
                version = "latest";
            }

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif

            var targetItem = GetTargetReleaseItem(version);

#if DEBUG
            _logger.Info($"targetItem = {targetItem}");
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
                sb.AppendLine(GetAssetTitle(asset.Kind));
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine($"<a href='{asset.Href}'>{asset.Title}</a>");
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            sb.AppendLine($"<h2>Release notes</h2>");

            sb.AppendLine($"<div>{ContentPreprocessor.Run(targetItem.Description, targetItem.IsMarkdown)}</div>");

            newRootNode.InnerHtml = sb.ToString();
        }

        private static string GetAssetTitle(KindOfReleaseAssetItem kind)
        {
            switch(kind)
            {
                case KindOfReleaseAssetItem.SourceCodeZip:
                    return "Source code (.zip)";

                case KindOfReleaseAssetItem.NuGet:
                    return "Nuget package";

                case KindOfReleaseAssetItem.LibraryArch:
                    return "Engine (.zip)";

                case KindOfReleaseAssetItem.LibraryFolder:
                    return "Engine (folder)";

                case KindOfReleaseAssetItem.CLIArch:
                    return "CLI (.zip)";

                case KindOfReleaseAssetItem.CLIFolder:
                    return "CLI (folder)";

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static ReleaseItem GetTargetReleaseItem(string version)
        {
            if(version == "latest")
            {
                return GeneralSettings.ReleaseItemsList.Single(p => p.IsLatest);
            }

            return GeneralSettings.ReleaseItemsList.Single(p => p.Version == version);
        }
    }
}
