using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.ReleaseInfoGeneration
{
    public class ReleaseInfoGenerator
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

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
                throw new NotImplementedException();
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc);
            }
        }
    }
}
