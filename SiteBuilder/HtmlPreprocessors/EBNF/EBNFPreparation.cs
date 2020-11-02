using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.EBNF
{
    public static class EBNFPreparation
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string Run(string initialContent)
        {
            var content = EBNFTemplatesResolver.Run(initialContent);

#if DEBUG
            _logger.Info($"content = {content}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            DiscoverNodes(doc.DocumentNode, doc);

            return doc.Text;
        }

        private static void DiscoverNodes(HtmlNode rootNode, HtmlDocument doc)
        {
            //#if DEBUG
            //            if (rootNode.Name != "#document")
            //            {
            //                NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.Name = '{rootNode.Name}'");
            //                NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //                NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //                NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerText = {rootNode.InnerText}");
            //            }
            //#endif

            if (rootNode.Name == "keywords")
            {
                var targetColsCount = rootNode.GetAttributeValue("cols", 0);

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"targetColsCount = '{targetColsCount}'");
#endif

                var tdWidth = 100 / targetColsCount;

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"tdWidth = {tdWidth}");
#endif

                var list = rootNode.ChildNodes.Where(p => p.Name == "kw").Select(p => p.GetAttributeValue("name", string.Empty)).Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().OrderBy(p => p).ToList();

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
                            spanNode.AddClass("keyword");
                            spanNode.SetAttributeValue("ebnf-type", "keyword");
                            spanNode.SetAttributeValue("ebnf-kind", "decl");
                            spanNode.InnerHtml = val;
                        }
                    }
                }

                //throw new NotImplementedException();

                return;
            }

            if (rootNode.Name == "ebnfcdecl")
            {
                var parentNode = rootNode.ParentNode;

                var name = rootNode.GetAttributeValue("name", string.Empty);
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"name = '{name}'");
#endif
                var linkNode = doc.CreateElement("a");
                parentNode.ReplaceChild(linkNode, rootNode);

                linkNode.SetAttributeValue("ebnf-type", name);
                linkNode.SetAttributeValue("ebnf-kind", "decl");
                linkNode.SetAttributeValue("href", $"#{name}");
                linkNode.SetAttributeValue("name", name);
                linkNode.InnerHtml = name;

                return;
            }

            if (rootNode.Name == "ebnfc")
            {
                var parentNode = rootNode.ParentNode;

                var name = rootNode.GetAttributeValue("name", string.Empty);
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"name = '{name}'");
#endif


                var linkNode = doc.CreateElement("a");
                parentNode.ReplaceChild(linkNode, rootNode);

                linkNode.SetAttributeValue("ebnf-type", name);
                linkNode.SetAttributeValue("ebnf-kind", "use");
                linkNode.SetAttributeValue("href", $"#{name}");
                linkNode.InnerHtml = name;

                return;
            }

            if (rootNode.Name == "gr")
            {
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.Name = '{rootNode.Name}'");
                //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
                //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
                //NLog.LogManager.GetCurrentClassLogger().Info($"rootNode.InnerText = {rootNode.InnerText}");
#endif
                var itemsList = EBNFHelpers.ParseGrammarBlock(rootNode.InnerHtml);
#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"itemsList = {JsonConvert.SerializeObject(itemsList, Formatting.Indented)}");
#endif
                var newNode = doc.CreateElement("div");

                var parentNode = rootNode.ParentNode;
                parentNode.ReplaceChild(newNode, rootNode);

                newNode.AddClass("ebnf-code");

                foreach (var item in itemsList)
                {
#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"item = '{item}'");
#endif

                    var itemNode = doc.CreateElement("div");
                    newNode.AppendChild(itemNode);
                    itemNode.AddClass("ebnf-code-item");

                    var tmpDoc = new HtmlDocument();
                    tmpDoc.LoadHtml(item);

                    DiscoverNodes(tmpDoc.DocumentNode, tmpDoc);

#if DEBUG
                    //NLog.LogManager.GetCurrentClassLogger().Info($"tmpDoc.DocumentNode.OuterHtml = '{tmpDoc.DocumentNode.OuterHtml}'");
                    //NLog.LogManager.GetCurrentClassLogger().Info($"tmpDoc.DocumentNode.InnerHtml = '{tmpDoc.DocumentNode.InnerHtml}'");
#endif

                    itemNode.InnerHtml = tmpDoc.DocumentNode.InnerHtml;
                }

                return;
            }

            if (rootNode.Name == "a")
            {
                var name = rootNode.GetAttributeValue("name", string.Empty);
                var href = rootNode.GetAttributeValue("href", string.Empty);

#if DEBUG
                //NLog.LogManager.GetCurrentClassLogger().Info($"name = '{name}'");
                //NLog.LogManager.GetCurrentClassLogger().Info($"href = '{href}'");
#endif
                if (string.IsNullOrWhiteSpace(href) && !string.IsNullOrWhiteSpace(name))
                {
                    if (name.StartsWith("#"))
                    {
                        href = name;
                    }
                    else
                    {
                        href = $"#{name}";
                    }

                    rootNode.SetAttributeValue("href", href);
                }

                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                DiscoverNodes(node, doc);
            }
        }
    }
}
