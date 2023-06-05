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

        public static string Run(string initialContent, MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            var hrefsDict = new Dictionary<string, string>();

            DiscoverHrefNodes(doc.DocumentNode, hrefsDict);
            DiscoverNodes(doc.DocumentNode, doc, hrefsDict, generalSiteBuilderSettings, markdownStrategy);

            return doc.ToHtmlString();
        }

        private static void DiscoverHrefNodes(HtmlNode rootNode, Dictionary<string, string> hrefsDict)
        {
#if DEBUG
            //if(rootNode.Name != "#document")
            //{
            //    _logger.Info($"rootNode.Name = '{rootNode.Name}'");
            //    _logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //    _logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //    _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}
#endif

            if (mTargetTags.Contains(rootNode.Name))
            {
                var dataHref = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //_logger.Info($"dataHref = '{dataHref}'");
#endif

                if (!string.IsNullOrWhiteSpace(dataHref))
                {
                    var title = rootNode.InnerText.Trim();

#if DEBUG
                    //_logger.Info($"title = '{title}'");
#endif

                    hrefsDict[dataHref] = title;
                }
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverHrefNodes(node, hrefsDict);
            }
        }

        private static void DiscoverNodes(HtmlNode rootNode, HtmlDocument doc, Dictionary<string, string> hrefsDict, GeneralSiteBuilderSettings generalSiteBuilderSettings, MarkdownStrategy markdownStrategy)
        {
#if DEBUG
            //_logger.Info($"rootNode.Name = '{rootNode.Name}'");
            //_logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //_logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //_logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
#endif

            if (rootNode.Name == "items")
            {
                ProcessItems(rootNode, doc);
                return;
            }

            if(rootNode.Name == "a")
            {
                var href = rootNode.GetAttributeValue("href", string.Empty);

                if(href.StartsWith("T:") || href.StartsWith("P:") || href.StartsWith("F:") || href.StartsWith("M:") || href.StartsWith("E:"))
                {
                    ProcessTypeNameHref(rootNode, doc, generalSiteBuilderSettings);
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

                newNode.InnerHtml = "<include path='CommonFragments/license.thtml'/>";

                return;
            }

            if(rootNode.Name == "general_description_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/general_description_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "name_meaning_and_pronunciation_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/name_meaning_and_pronunciation_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "project_reason_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/project_reason_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "aim_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/aim_content.thtml'/>";

                return;
            }

            if(rootNode.Name == "disclaimer")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = GetDisclaimerHtml();

                return;
            }

            if (rootNode.Name == "dsl_preview_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/dsl_preview_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "project_status_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/project_status_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "contributing_preview_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/contributing_preview_content.thtml'/>";

                return;
            }

            //if(rootNode.Name == "")
            //{
            //    var newNode = doc.CreateElement("div");
            //    var parentNode = rootNode.ParentNode;

            //    parentNode.ReplaceChild(newNode, rootNode);

            //    newNode.InnerHtml = "<include path='CommonFragments/.thtml'/>";

            //    return;
            //}

            if (rootNode.Name == "key_features_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                ProcessKeyFeaturesContent(newNode, doc);

                return;
            }

            if(rootNode.Name == "key_features_preview")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                ProcessKeyFeaturesPreview(newNode, doc, markdownStrategy, generalSiteBuilderSettings);

                return;
            }

            if (rootNode.Name == "include")
            {
                var path = rootNode.GetAttributeValue("path", string.Empty);

#if DEBUG
                //_logger.Info($"path = '{path}'");
#endif

                if(string.IsNullOrWhiteSpace(path))
                {
                    throw new NullReferenceException("Attribute 'path' of <include/> can not be null ore empty.");
                }

                if(path.StartsWith("/") || path.StartsWith("\\") || generalSiteBuilderSettings.SourcePath.StartsWith("/") || generalSiteBuilderSettings.SourcePath.StartsWith("\\"))
                {
                    path = $"{generalSiteBuilderSettings.SourcePath}{path}";
                }
                else
                {
                    path = $"{generalSiteBuilderSettings.SourcePath}/{path}";
                }

#if DEBUG
                //_logger.Info($"path (after) = '{path}'");
#endif

                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = File.ReadAllText(path);

                return;
            }

            if (rootNode.Name == "see")
            {
                var parentNode = rootNode.ParentNode;

                var dataHref = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //_logger.Info($"dataHref = '{dataHref}'");
#endif

                if (dataHref.Contains(".html"))
                {
                    throw new NotSupportedException();
                }
                else
                {
                    var title = hrefsDict[dataHref];

#if DEBUG
                    //_logger.Info($"title = '{title}'");
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

            if(rootNode.Name == "work_in_progress")
            {
                var parentNode = rootNode.ParentNode;

                var node = doc.CreateElement("p");
                parentNode.ReplaceChild(node, rootNode);

                var imgNode = doc.CreateElement("img");
                node.AppendChild(imgNode);

                imgNode.SetAttributeValue("src", "/assets/work-in-progress.png");
                imgNode.SetAttributeValue("alt", "Work in progress");

                return;
            }

            if(rootNode.Name == "curr_year")
            {
                var parentNode = rootNode.ParentNode;

                var rootSpanNode = doc.CreateTextNode(DateTime.Now.Year.ToString());
                parentNode.ReplaceChild(rootSpanNode, rootNode);

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
                //_logger.Info($"targetValue = '{targetValue}'");
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

                    case "GitHub":
                    case "Github":
                    case "github":
                    case "git":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-github");
                            wikiNode.SetAttributeValue("title", "GitHub");
                        }
                        break;

                    case "LinkedIn":
                    case "Linkedin":
                    case "linkedin":
                    case "In":
                    case "in":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-linkedin");
                            wikiNode.SetAttributeValue("title", "Facebook");
                        }
                        break;

                    case "Unity":
                    case "unity":
                    case "U":
                    case "u":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-unity");
                            wikiNode.SetAttributeValue("title", "Unity");
                        }
                        break;

                    case "YouTube":
                    case "youtube":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("fab fa-youtube");
                            wikiNode.SetAttributeValue("title", "YouTube");
                        }
                        break;

                    case "Discussions":
                        {
                            var wikiNode = doc.CreateElement("i");
                            parentNode.ReplaceChild(wikiNode, rootNode);
                            wikiNode.AddClass("far fa-comments");
                            wikiNode.SetAttributeValue("title", "Discussions");
                        }
                        break;

                    default:
                        rootNode.Remove();
                        break;
                }

                return;
            }

            if(rootNode.Name == "gist")
            {
#if DEBUG
                //_logger.Info($"rootNode.InnerHtml = '{rootNode.InnerHtml}'");
#endif

                var parentNode = rootNode.ParentNode;

                var node = doc.CreateElement("b");
                parentNode.ReplaceChild(node, rootNode);

                node.SetAttributeValue("orig", "gist");

                node.InnerHtml = rootNode.InnerHtml;

                return;
            }

            if(rootNode.Name == "cli_requirements")
            {
                var parentNode = rootNode.ParentNode;

                var node = doc.CreateElement("ul");
                parentNode.ReplaceChild(node, rootNode);

                var sb = new StringBuilder();

                sb.AppendLine($"<li>{_netVersion}</li>");

                node.InnerHtml = sb.ToString();

                return;
            }

            if (rootNode.Name == "unity_requirements")
            {
                var parentNode = rootNode.ParentNode;

                var node = doc.CreateElement("ul");
                parentNode.ReplaceChild(node, rootNode);

                var sb = new StringBuilder();

                sb.AppendLine($"<li>Unity {_unityVersion}</li>");
                sb.AppendLine($"<li>{_netVersion}</li>");

                node.InnerHtml = sb.ToString();

                return;
            }

            if (rootNode.Name == "cli_requirements_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/cli_requirements_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "unity_requirements_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/unity_requirements_content.thtml'/>";

                return;
            }

            if (rootNode.Name == "requirements_content")
            {
                var newNode = doc.CreateElement("div");
                var parentNode = rootNode.ParentNode;

                parentNode.ReplaceChild(newNode, rootNode);

                newNode.InnerHtml = "<include path='CommonFragments/requirements_content.thtml'/>";

                return;
            }

            if (mTargetTags.Contains(rootNode.Name))
            {
                var isProcessedStr = rootNode.GetAttributeValue("is-processed", string.Empty);

                if(string.IsNullOrWhiteSpace(isProcessedStr))
                {
                    rootNode.SetAttributeValue("is-processed", "true");
                }
                else
                {
                    return;
                }

                var dataHrefValue = rootNode.GetAttributeValue("data-href", string.Empty);

#if DEBUG
                //_logger.Info($"dataHrefValue = '{dataHrefValue}'");
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
                    //_logger.Info($"href = '{href}'");
                    //_logger.Info($"name = '{name}'");
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
                DiscoverNodes(node, doc, hrefsDict, generalSiteBuilderSettings, markdownStrategy);
            }
        }

        private const string _netVersion = "NET 7.0";
        private const string _unityVersion = "2022.2.8f1";

        private static void ProcessTypeNameHref(HtmlNode rootNode, HtmlDocument doc, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var href = rootNode.GetAttributeValue("href", string.Empty);

#if DEBUG
            //_logger.Info($"href = {href}");
#endif

            var codeDocument = generalSiteBuilderSettings.CSharpUserApiXMLDocsCodeDocumentDict[href];

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
            //_logger.Info($"targetColsCount = '{targetColsCount}'");
#endif

            var tdWidth = 100 / targetColsCount;

#if DEBUG
            //_logger.Info($"tdWidth = {tdWidth}");
#endif

            var list = rootNode.ChildNodes.Where(p => p.Name == "item").Select(p => p.GetAttributeValue("value", string.Empty)).Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().OrderBy(p => p).ToList();

#if DEBUG
            //_logger.Info($"list = {JsonConvert.SerializeObject(list, Formatting.Indented)}");
#endif

            var countInCol = list.Count / targetColsCount;

#if DEBUG
            //_logger.Info($"list.Count = {list.Count}");
            //_logger.Info($"countInCol = {countInCol}");
            //_logger.Info($"list.Count % targetColsCount = {list.Count % targetColsCount}");
#endif
            if ((list.Count % targetColsCount) > 0)
            {
                countInCol++;
            }

#if DEBUG
            //_logger.Info($"countInCol (2) = {countInCol}");
#endif
            var counter = countInCol;

            var groupedDict = list.GroupBy(p => counter++ / countInCol).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            //_logger.Info($"groupedDict = {JsonConvert.SerializeObject(groupedDict, Newtonsoft.Json.Formatting.Indented)}");
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
                //_logger.Info($"i = {i}");
#endif

                var trNode = doc.CreateElement("tr");
                tbodyNode.AppendChild(trNode);

                foreach (var groupedItem in groupedDict)
                {
                    var tdNode = doc.CreateElement("td");
                    trNode.AppendChild(tdNode);
                    tdNode.SetAttributeValue("style", $"width: {tdWidth}%;");

#if DEBUG
                    //_logger.Info($"groupedItem.Value.Count = {groupedItem.Value.Count}");
#endif
                    if (groupedItem.Value.Count > i)
                    {
                        var val = groupedItem.Value[i];
#if DEBUG
                        //_logger.Info($"val = {val}");
#endif

                        var spanNode = doc.CreateElement("span");
                        tdNode.AppendChild(spanNode);

                        spanNode.InnerHtml = val;
                    }
                }
            }
        }

        private static void ProcessKeyFeaturesContent(HtmlNode rootNode, HtmlDocument doc)
        {
            rootNode.InnerHtml = GetKeyFeaturesContent();
        }

        private static void ProcessKeyFeaturesPreview(HtmlNode rootNode, HtmlDocument doc, MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<ul>");

            var keyFeatureNames = GetKeyFeatureNames(markdownStrategy, generalSiteBuilderSettings);

            foreach(var featureName in keyFeatureNames)
            {
                sb.AppendLine("<li>");
                sb.AppendLine($"<b>{featureName}</b>");
                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");

            sb.AppendLine("<p>");
            sb.AppendLine($"Learn key features in details <a href='{generalSiteBuilderSettings.SiteSettings.DestKeyFeaturesPath}'>here</a>.");
            sb.AppendLine("</p>");

            rootNode.InnerHtml = sb.ToString();
        }

        private static List<string> GetKeyFeatureNames(MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            return ExtractGistsContent(GetKeyFeaturesContent(), markdownStrategy, generalSiteBuilderSettings);
        }

        private static string GetKeyFeaturesContent()
        {
            return "<include path='CommonFragments/key-features.thtml'/>";
        }

        private static List<string> ExtractGistsContent(string html, MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //_logger.Info($"html = '{html}'");
#endif

            html = ContentPreprocessor.Run(html, markdownStrategy, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"html (after) = '{html}'");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var result = new List<string>();

            DiscoverGistNodes(doc.DocumentNode, result);

            return result;
        }

        private static void DiscoverGistNodes(HtmlNode rootNode, List<string> result)
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
            
            if(rootNode.Name == "gist")
            {
                result.Add(rootNode.InnerHtml);
                return;
            }

            if(rootNode.Name == "b" && rootNode.GetAttributeValue("orig", string.Empty) == "gist")
            {
                result.Add(rootNode.InnerHtml);
                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverGistNodes(node, result);
            }
        }

        public static string GetDisclaimerHtml()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div style='background-color: #FFF9F3;'>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='background-color: #FEBE0A;'>");
            sb.AppendLine("&nbsp;");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine("<img src='icons8-warning-48.png' width='48px' height='48px' alt='Warning logo'/>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine("<b>Purely experimental and very unstable project developed by only one person</b></br>");
            sb.AppendLine("Please read the <a href='you-need-to-know.html'>page</a> before starting");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}
