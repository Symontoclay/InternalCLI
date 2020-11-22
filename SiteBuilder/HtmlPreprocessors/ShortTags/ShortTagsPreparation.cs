using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.ShortTags
{
    public static class ShortTagsPreparation
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static readonly List<string> mTargetTags = new List<string>() {
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6"
        };

        public static string Run(string initialContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            var hrefsDict = new Dictionary<string, string>();

            DiscoverHrefNodes(doc.DocumentNode, hrefsDict);
            DiscoverNodes(doc.DocumentNode, doc, hrefsDict);

            return doc.ToHtmlString();
        }

        private static void DiscoverHrefNodes(HtmlNode rootNode, Dictionary<string, string> hrefsDict)
        {
#if DEBUG
            //if(rootNode.Name != "#document")
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.Name = '{rootNode.Name}'");
            //    NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //    NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //    NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}
#endif

            if (mTargetTags.Contains(rootNode.Name))
            {
                var dataHref = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"dataHref = '{dataHref}'");
#endif

                if (!string.IsNullOrWhiteSpace(dataHref))
                {
                    var title = rootNode.InnerText.Trim();

#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"title = '{title}'");
#endif

                    hrefsDict[dataHref] = title;
                }
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverHrefNodes(node, hrefsDict);
            }
        }

        private static void DiscoverNodes(HtmlNode rootNode, HtmlDocument doc, Dictionary<string, string> hrefsDict)
        {
#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.Name = '{rootNode.Name}'");
            //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerText = {rootNode.InnerText}");
#endif
            if (rootNode.Name == "linktocontent")
            {
                var parentNode = rootNode.ParentNode;

                var linkToContentNodePlace = doc.CreateElement("p");
                parentNode.ReplaceChild(linkToContentNodePlace, rootNode);

                var linkToContentNode = doc.CreateElement("a");
                linkToContentNodePlace.ChildNodes.Add(linkToContentNode);
                linkToContentNode.SetAttributeValue("href", "#Contents");
                linkToContentNode.InnerHtml = "<i class='fas fa-long-arrow-alt-up'></i> back to top";
                return;
            }

            if(rootNode.Name == "license_info")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                ProcessLicenseInfo(newNode, doc);

                return;
            }

            if (rootNode.Name == "see")
            {
                var parentNode = rootNode.ParentNode;

                var dataHref = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"dataHref = '{dataHref}'");
#endif

                if (dataHref.Contains(".html"))
                {
                    throw new NotSupportedException();
                }
                else
                {
                    var title = hrefsDict[dataHref];

#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"title = '{title}'");
#endif

                    if (!dataHref.StartsWith("#"))
                    {
                        dataHref = $"#{dataHref}";
                    }

                    var rootSpanNode = doc.CreateElement("span");
                    parentNode.ReplaceChild(rootSpanNode, rootNode);

                    var textSpanNode = doc.CreateElement("span");
                    textSpanNode.InnerHtml = "See more for details in&nbsp;<i class='fas fa-link' style='font-size:12px;'></i>";

                    rootSpanNode.AppendChild(textSpanNode);

                    var linkNode = doc.CreateElement("a");

                    linkNode.SetAttributeValue("href", dataHref);
                    linkNode.InnerHtml = title;

                    rootSpanNode.AppendChild(linkNode);
                }

                return;
            }


            if (rootNode.Name == "ico")
            {
                var parentNode = rootNode.ParentNode;

                var targetValue = rootNode.GetAttributeValue("target", string.Empty);

                if(string.IsNullOrWhiteSpace(targetValue))
                {
                    targetValue = rootNode.GetAttributeValue("t", string.Empty);
                }
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"targetValue = '{targetValue}'");
#endif
                switch (targetValue)
                {
                    case "W":
                    case "w":
                    case "wikipedia":
                    case "Wikipedia":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-wikipedia-w");
                            wikiNode.SetAttributeValue("title", "Wikipedia");
                        }
                        break;

                    case "Facebook":
                    case "facebook":
                    case "Fb":
                    case "fb":
                    case "F":
                    case "f":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-facebook");
                            wikiNode.SetAttributeValue("title", "Facebook");
                        }
                        break;

                    default:
                        rootNode.Remove();
                        break;
                }

                return;
            }

            if (mTargetTags.Contains(rootNode.Name))
            {
                var dataHrefValue = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"dataHrefValue = '{dataHrefValue}'");
#endif
                if (!string.IsNullOrWhiteSpace(dataHrefValue))
                {
                    var href = string.Empty;
                    var name = string.Empty;

                    if (dataHrefValue.StartsWith("#"))
                    {
                        href = dataHrefValue;
                        name = dataHrefValue.Substring(1);
                    }
                    else
                    {
                        href = $"#{dataHrefValue}";
                        name = dataHrefValue;
                    }

#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"href = '{href}'");
                    //NLog.LogManager.GetCurrentClassLogger().Info($"name = '{name}'");
#endif
                    var hrefNode = doc.CreateElement("a");
                    rootNode.ChildNodes.Add(hrefNode);
                    hrefNode.SetAttributeValue("href", href);
                    hrefNode.SetAttributeValue("name", name);
                    hrefNode.SetAttributeValue("title", "The link to the section");
                    hrefNode.AddClass("permalink");
                    hrefNode.InnerHtml = "<i class='fas fa-link'></i>";

                    return;
                }
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverNodes(node, doc, hrefsDict);
            }
        }

        private static void ProcessLicenseInfo(HtmlNode rootNode, HtmlDocument doc)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<p>");
            sb.AppendLine("SymOntoClay is released under <a href='http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html'>LGPL-2.1 License</a>.");
            sb.AppendLine("</p>");
            sb.AppendLine("<p>");
            sb.AppendLine("Please study the license before downloading and using!");
            sb.AppendLine("</p>");

            rootNode.InnerHtml = sb.ToString();
        }
    }
}
