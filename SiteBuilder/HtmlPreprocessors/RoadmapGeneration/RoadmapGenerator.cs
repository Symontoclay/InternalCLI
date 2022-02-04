using HtmlAgilityPack;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.RoadmapGeneration
{
    public class RoadmapGenerator
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

            if (rootNode.Name == "roadmap")
            {
                ProcessRoadMap(rootNode, doc, generalSiteBuilderSettings);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc, generalSiteBuilderSettings);
            }
        }

        private static void ProcessRoadMap(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var newRoadMapNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRoadMapNode, rootNode);

            newRoadMapNode.AddClass("roadmap-container");

            var roadMapStartNode = doc.CreateElement("div");
            newRoadMapNode.AppendChild(roadMapStartNode);
            roadMapStartNode.AddClass("roadmap-start");

#if DEBUG
            //_logger.Info($"generalSiteBuilderSettings.RoadMapItemsList.Count = {generalSiteBuilderSettings.RoadMapItemsList.Count}");
#endif

            var odd = true;

            foreach (var item in generalSiteBuilderSettings.RoadMapItemsList)
            {
#if DEBUG
                //_logger.Info($"item = {item}");
#endif

                var itemNode = doc.CreateElement("div");
                roadMapStartNode.AppendChild(itemNode);
                itemNode.AddClass("roadmap-start-block");

                var itemContentNode = doc.CreateElement("div");
                itemNode.AppendChild(itemContentNode);
                itemContentNode.AddClass("roadmap-start-content");

                var itemLRColumn = doc.CreateElement("div");
                itemContentNode.AppendChild(itemLRColumn);

                if(odd)
                {
                    itemLRColumn.AddClass("roadmap-left-column");

                    odd = false;
                }
                else
                {
                    itemLRColumn.AddClass("roadmap-right-column");

                    odd = true;
                }

                var sb = new StringBuilder();

                sb.AppendLine($"<h4>{item.Title}</h4>");

                sb.AppendLine($"<p><b>{item.PeriodOfCompletion}</b></p>");

                sb.AppendLine($"<p>{ContentPreprocessor.Run(item.Description, item.IsMarkDown, generalSiteBuilderSettings)}</p>");

                if(!string.IsNullOrWhiteSpace(item.HrefWithDetailedInfomation))
                {
                    sb.AppendLine($"<p><a href='{item.HrefWithDetailedInfomation}'>Read more</a></p>");
                }

                itemLRColumn.InnerHtml = sb.ToString();
            }
        }
    }
}
