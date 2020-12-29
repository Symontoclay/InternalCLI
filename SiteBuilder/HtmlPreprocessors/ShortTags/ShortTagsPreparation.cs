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

            if(rootNode.Name == "items")
            {
                ProcessItems(rootNode, doc);
                return;
            }

            if(rootNode.Name == "a")
            {
                var href = rootNode.GetAttributeValue("href", string.Empty);

                if(href.StartsWith("T:") || href.StartsWith("P:") || href.StartsWith("F:") || href.StartsWith("M:") || href.StartsWith("E:"))
                {
                    ProcessTypeNameHref(rootNode, doc);
                    return;
                }
            }

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

            if(rootNode.Name == "complete_me")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.SetAttributeValue("style", "font-weight: bold; color: red;");
                newNode.InnerHtml = "COMPLETE ME!";

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
        
        private static void ProcessTypeNameHref(HtmlNode rootNode, HtmlDocument doc)
        {
            var href = rootNode.GetAttributeValue("href", string.Empty);

#if DEBUG
            //_logger.Info($"href = {href}");
#endif

            var codeDocument = GeneralSettings.CSharpUserApiXMLDocsCodeDocumentDict[href];

#if DEBUG
            //_logger.Info($"codeDocument = {codeDocument}");
#endif

            rootNode.SetAttributeValue("href", codeDocument.Href);

            rootNode.InnerHtml = codeDocument.DisplayedName;
        }

        private static void ProcessItems(HtmlNode rootNode, HtmlDocument doc)
        {
            var targetColsCount = rootNode.GetAttributeValue("cols", 0);

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"targetColsCount = '{targetColsCount}'");
#endif

            var tdWidth = 100 / targetColsCount;

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"tdWidth = {tdWidth}");
#endif

            var list = rootNode.ChildNodes.Where(p => p.Name == "item").Select(p => p.GetAttributeValue("value", string.Empty)).Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().OrderBy(p => p).ToList();

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"list = {JsonConvert.SerializeObject(list, Formatting.Indented)}");
#endif

            var countInCol = list.Count / targetColsCount;

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"list.Count = {list.Count}");
            //NLog.LogManager.GetCurrentClassLogger().Info($"countInCol = {countInCol}");
            //NLog.LogManager.GetCurrentClassLogger().Info($"list.Count % targetColsCount = {list.Count % targetColsCount}");
#endif
            if ((list.Count % targetColsCount) > 0)
            {
                countInCol++;
            }

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"countInCol (2) = {countInCol}");
#endif
            var counter = countInCol;

            var groupedDict = list.GroupBy(p => counter++ / countInCol).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"groupedDict = {JsonConvert.SerializeObject(groupedDict, Newtonsoft.Json.Formatting.Indented)}");
#endif
            var parentNode = rootNode.ParentNode;
            var tableNode = doc.CreateElement("table");
            parentNode.ReplaceChild(tableNode, rootNode);
            tableNode.AddClass("keywords-table");
            //tableNode.SetAttributeValue("border", "1");

            var tbodyNode = doc.CreateElement("tbody");
            tableNode.AppendChild(tbodyNode);

            for (var i = 0; i < countInCol; i++)
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"i = {i}");
#endif

                var trNode = doc.CreateElement("tr");
                tbodyNode.AppendChild(trNode);

                foreach (var groupedItem in groupedDict)
                {
                    var tdNode = doc.CreateElement("td");
                    trNode.AppendChild(tdNode);
                    tdNode.SetAttributeValue("style", $"width: {tdWidth}%;");

#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"groupedItem.Value.Count = {groupedItem.Value.Count}");
#endif
                    if (groupedItem.Value.Count > i)
                    {
                        var val = groupedItem.Value[i];
#if DEBUG
                        //NLog.LogManager.GetCurrentClassLogger().Info($"val = {val}");
#endif

                        var spanNode = doc.CreateElement("span");
                        tdNode.AppendChild(spanNode);

                        spanNode.InnerHtml = val;
                    }
                }
            }
        }

        private static void ProcessLicenseInfo(HtmlNode rootNode, HtmlDocument doc)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<p>");
            sb.AppendLine("SymOntoClay is released under <a href='http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html'>LGPL-2.1 License</a>.");
            sb.AppendLine("</p>");
            sb.AppendLine("<p>");
            sb.AppendLine("Please read the license before downloading and using!");
            sb.AppendLine("</p>");

            rootNode.InnerHtml = sb.ToString();
        }
    }
}
