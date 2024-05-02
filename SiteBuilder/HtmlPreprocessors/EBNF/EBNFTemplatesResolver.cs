using HtmlAgilityPack;
using NLog;
using SymOntoClay.Common.CollectionsHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiteBuilder.HtmlPreprocessors.EBNF
{
    public static class EBNFTemplatesResolver
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string Run(string initialContent)
        {
            //_logger.Info("Begin");

            var n = 0;

            var modifiedDoc = new HtmlDocument();
            modifiedDoc.LoadHtml(initialContent);

            var tEBNFCDECLStorage = new TEBNFCDECLStorage();

            while (RunIteration(modifiedDoc, tEBNFCDECLStorage))
            {
                n++;

                //_logger.Info($"n = {n}");

                if (n > 100)
                {
                    throw new NotSupportedException($"Too much iterations!!!");
                }

                var intermediateContent = modifiedDoc.ToHtmlString();

#if DEBUG
                //_logger.Info($"intermediateContent = {intermediateContent}");
#endif

                modifiedDoc = new HtmlDocument();
                modifiedDoc.LoadHtml(intermediateContent);
            };

            //_logger.Info("End");

            var resultStrWriter = new StringWriter();
            modifiedDoc.Save(resultStrWriter);

            return resultStrWriter.ToString();
        }

        private static bool RunIteration(HtmlDocument doc, TEBNFCDECLStorage tEBNFCDECLStorage)
        {
            var gEBNFCStorage = new GEBNFCStorage();

            var result1 = DiscoverTemplateNodes(doc.DocumentNode, gEBNFCStorage, tEBNFCDECLStorage);
            var result2 = ExploreTemplateNodes(doc.DocumentNode, doc, gEBNFCStorage, tEBNFCDECLStorage);

            return result1 || result2;
        }

        private static bool DiscoverTemplateNodes(HtmlNode rootNode, GEBNFCStorage gEBNFCStorage, TEBNFCDECLStorage tEBNFCDECLStorage)
        {
            //if (rootNode.Name != "#document")
            //{
            //_logger.Info($"rootNode.Name = '{rootNode.Name}'");
            //_logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
            //_logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
            //_logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}

            if (rootNode.Name == "ebnfcdecl")
            {
                gEBNFCStorage.RegNode(rootNode);

                return false;
            }

            if (rootNode.Name == "tebnfcdecl")
            {
                tEBNFCDECLStorage.RegNode(rootNode);
                rootNode.Remove();

                return true;
            }

            var result = false;

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                if (DiscoverTemplateNodes(node, gEBNFCStorage, tEBNFCDECLStorage))
                {
                    result = true;
                }
            }

            return result;
        }

        private static bool ExploreTemplateNodes(HtmlNode rootNode, HtmlDocument doc, GEBNFCStorage gEBNFCStorage, TEBNFCDECLStorage tEBNFCDECLStorage)
        {
            //if (rootNode.Name != "#document")
            //{
            //    _logger.Info($"ExploreTemplateNodes rootNode.Name = '{rootNode.Name}'");
            //    _logger.Info($"ExploreTemplateNodes rootNode.OuterHtml = {rootNode.OuterHtml}");
            //    _logger.Info($"ExploreTemplateNodes rootNode.InnerHtml = {rootNode.InnerHtml}");
            //    _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            //}

            if (rootNode.Name == "gebnfc")
            {
                var name = rootNode.GetAttributeValue("name", string.Empty);

                //_logger.Info($"ExploreTemplateNodes name = '{name}'");

                var kind = rootNode.GetAttributeValue("kind", string.Empty);

                //_logger.Info($"ExploreTemplateNodes kind = '{kind}'");

                var itemsList = gEBNFCStorage.GetGroup(name);

                //_logger.Info($"ExploreTemplateNodes itemsList = {JsonConvert.SerializeObject(itemsList, Formatting.Indented)}");

                var include = rootNode.GetAttributeValue("include", string.Empty);

                //_logger.Info($"ExploreTemplateNodes include = '{include}'");

                if (!string.IsNullOrWhiteSpace(include))
                {
                    var includeList = include.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).SelectMany(p => p.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

                    foreach(var includeName in includeList)
                    {
                        //_logger.Info($"ExploreTemplateNodes includeName = '{includeName}'");

                        var includeItemsList = gEBNFCStorage.GetGroup(includeName);

                        //_logger.Info($"ExploreTemplateNodes v = {JsonConvert.SerializeObject(includeItemsList, Formatting.Indented)}");

                        if(includeItemsList.IsNullOrEmpty())
                        {
                            continue;
                        }

                        itemsList.AddRange(includeItemsList);
                    }
                }

                if (!itemsList.Any())
                {
                    rootNode.Remove();

                    return true;
                }

                itemsList = itemsList.Distinct().OrderBy(p => p).ToList();

                var resultList = new List<string>();

                foreach (var item in itemsList)
                {
                    //_logger.Info($"item = '{item}'");

                    if (kind == "op_and" || kind == "op_declset")
                    {
                        resultList.Add($"[ <EBNFC name='{item}' /> ]");
                    }
                    else
                    {
                        resultList.Add($"<EBNFC name='{item}' />");
                    }
                }

                //_logger.Info($"ExploreTemplateNodes resultList = {JsonConvert.SerializeObject(resultList, Formatting.Indented)}");

                var separator = " ";

                switch(kind)
                {
                    case "or":
                        separator = " | ";
                        break;

                    case "op_declset":
                        separator = "<br/>";
                        break;

                    default:
                        break;
                }

                var resultStr = string.Join(separator, resultList);

                if(kind == "op_declset")
                {
                    resultStr = $"[ {{ {resultStr} }} ]";
                }

                //_logger.Info($"ExploreTemplateNodes resultStr = '{resultStr}'");

                var textNode = doc.CreateTextNode(resultStr);

                var parentNode = rootNode.ParentNode;
                parentNode.ReplaceChild(textNode, rootNode);

                return true;
            }

            if (rootNode.Name == "tebnfc")
            {
                tEBNFCDECLStorage.ProcessNode(rootNode, doc);

                return true;
            }

            var result = false;

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                if (ExploreTemplateNodes(node, doc, gEBNFCStorage, tEBNFCDECLStorage))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
