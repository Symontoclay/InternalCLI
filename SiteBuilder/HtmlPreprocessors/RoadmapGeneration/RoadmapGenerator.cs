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

        public static string Run(string initialContent)
        {
#if DEBUG
            //_logger.Info($"initialContent = {initialContent}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            ProcessNodes(doc.DocumentNode, doc);

            return doc.ToHtmlString();
        }

        private static void ProcessNodes(HtmlNode rootNode, HtmlDocument doc)
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
                ProcessRoadMap(rootNode, doc);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc);
            }
        }

        private static void ProcessRoadMap(HtmlNode rootNode, HtmlDocument doc)
        {
            var newRoadMapNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRoadMapNode, rootNode);

#if DEBUG
            //_logger.Info($"GeneralSettings.RoadMapItemsList.Count = {GeneralSettings.RoadMapItemsList.Count}");
#endif

            foreach (var item in GeneralSettings.RoadMapItemsList)
            {
#if DEBUG
                //_logger.Info($"item = {item}");
#endif

                if(item.Kind != KindOfRoadMapItem.Step && item.Kind != KindOfRoadMapItem.Unplanned)
                {
                    throw new NotImplementedException();
                }

                if(item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Completed || item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Unknown)
                {
                    continue;
                }

#if DEBUG
                //_logger.Info($"item = {item}");
#endif

                var itemNode = doc.CreateElement("div");
                newRoadMapNode.AppendChild(itemNode);
                itemNode.SetAttributeValue("style", "margin-bottom: 20px;");

                var sb = new StringBuilder();

                if(item.Kind == KindOfRoadMapItem.Step)
                {
                    sb.AppendLine($"<h2>{item.Name}</h2>");

                    if (item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Developed)
                    {
                        sb.AppendLine($"<div><i class='fas fa-tasks'></i>&nbsp;&nbsp;In development</div>");
                    }
                    else
                    {
                        sb.AppendLine($"<div><i class='fas fa-hourglass-start'></i>&nbsp;&nbsp;Planned</div>");
                    }

                    sb.AppendLine($"<div><i class='far fa-calendar-alt'></i>&nbsp;&nbsp;<span style='font-weight: bold;'>Period</span>: {item.Start.Value.ToString("D", _targetCulture)} - {item.End.Value.ToString("D", _targetCulture)}</div>");

                    sb.AppendLine($"<div><span style='font-weight: bold;'>Version</span>: {item.Version}</div>");
                }
                else
                {
                    if(item.Kind == KindOfRoadMapItem.Unplanned)
                    {
                        sb.AppendLine("<h2><i class='fas fa-warehouse'></i>&nbsp;&nbsp;Unplanned backlog</h2>");
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                sb.AppendLine($"<div>{ContentPreprocessor.Run(item.Description, item.IsMarkdown)}</div>");

                itemNode.InnerHtml = sb.ToString();
            }
        }
    }
}
