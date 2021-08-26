using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.CSharpUserApiContentsGeneration
{
    public static class CSharpUserApiContentsGenerator
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

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

            if(rootNode.Name == "csharp_user_api_contents")
            {
                ProcessCSharpUserApiContents(rootNode, doc, generalSiteBuilderSettings);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc, generalSiteBuilderSettings);
            }
        }

        private static void ProcessCSharpUserApiContents(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var newRootNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newRootNode, rootNode);

            var sb = new StringBuilder();

            foreach (var packageCard in generalSiteBuilderSettings.CSharpUserApiXMLDocsList)
            {
#if DEBUG
                //_logger.Info($"packageCard = {packageCard.ToBriefString()}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.InterfacesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.ClassesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.EnumsList.Count}");
                //_logger.Info($"packageCard.AssemblyName.Name = {packageCard.AssemblyName.Name}");
#endif

                sb.AppendLine($"<h2>{packageCard.AssemblyName.Name}</h2>");

                if (packageCard.InterfacesList.Any())
                {
                    sb.AppendLine("<h3>Interfaces</h3>");

                    foreach (var item in packageCard.InterfacesList)
                    {
#if DEBUG
                        //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                        //_logger.Info($"item.Href = {item.Href}");
                        //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                        sb.AppendLine($"<div><a href='{item.Href}'>{item.Name.DisplayedName}</a></div>");
                    }

                    sb.AppendLine("<div>&nbsp;</div>");
                }

                if (packageCard.ClassesList.Any())
                {
                    sb.AppendLine("<h3>Classes</h3>");

                    foreach (var item in packageCard.ClassesList)
                    {
#if DEBUG
                        //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                        //_logger.Info($"item.Href = {item.Href}");
                        //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                        sb.AppendLine($"<div><a href='{item.Href}'>{item.Name.DisplayedName}</a></div>");
                    }

                    sb.AppendLine("<div>&nbsp;</div>");
                }

                if (packageCard.EnumsList.Any())
                {
                    sb.AppendLine("<h3>Enums</h3>");

                    foreach (var item in packageCard.EnumsList)
                    {
#if DEBUG
                        //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                        //_logger.Info($"item.Href = {item.Href}");
                        //_logger.Info($"item.TargetFullFileName = {item.TargetFullFileName}");
#endif

                        sb.AppendLine($"<div><a href='{item.Href}'>{item.Name.DisplayedName}</a></div>");
                    }

                    sb.AppendLine("<div>&nbsp;</div>");
                }
            }

            newRootNode.InnerHtml = sb.ToString();
        }
    }
}
