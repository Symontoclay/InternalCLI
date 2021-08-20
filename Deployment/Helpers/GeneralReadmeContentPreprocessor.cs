using HtmlAgilityPack;
using NLog;
using SiteBuilder.HtmlPreprocessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class GeneralReadmeContentPreprocessor
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string Run(string initialContent, string commonBadgesFileName, string repositorySpecificBadgesFileName, string repositorySpecificReadmeFileName)
        {
#if DEBUG
            //_logger.Info($"commonBadgesFileName = {commonBadgesFileName}");
            //_logger.Info($"repositorySpecificBadgesFileName = {repositorySpecificBadgesFileName}");
            //_logger.Info($"repositorySpecificReadmeFileName = {repositorySpecificReadmeFileName}");
            //_logger.Info($"initialContent = '{initialContent}'");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            DiscoverNodes(doc.DocumentNode, doc, commonBadgesFileName, repositorySpecificBadgesFileName, repositorySpecificReadmeFileName);

            return doc.ToHtmlString();
        }

        private static void DiscoverNodes(HtmlNode rootNode, HtmlDocument doc, string commonBadgesFileName, string repositorySpecificBadgesFileName, string repositorySpecificReadmeFileName)
        {
//#if DEBUG
            //if (rootNode.Name != "#document")
            //{
            //    _logger.Info($"rootNode.Name = '{rootNode.Name}'");
            //    _logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //    _logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //    _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}
//#endif

            if(rootNode.Name == "spec_readme")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                if (!string.IsNullOrWhiteSpace(repositorySpecificReadmeFileName))
                {
                    var content = File.ReadAllText(repositorySpecificReadmeFileName);

                    newNode.InnerHtml = content;
                }

                return;
            }

            if(rootNode.Name == "gen_badges")
            {
                var newNode = doc.CreateElement("span");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                if (!string.IsNullOrWhiteSpace(commonBadgesFileName))
                {
                    var content = File.ReadAllText(commonBadgesFileName);

                    newNode.InnerHtml = content;
                }

                return;
            }

            if (rootNode.Name == "spec_badges")
            {
                var newNode = doc.CreateElement("span");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                if (!string.IsNullOrWhiteSpace(repositorySpecificBadgesFileName))
                {
                    var content = File.ReadAllText(repositorySpecificBadgesFileName);

                    newNode.InnerHtml = content;
                }

                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverNodes(node, doc, commonBadgesFileName, repositorySpecificBadgesFileName, repositorySpecificReadmeFileName);
            }
        }
    }
}
