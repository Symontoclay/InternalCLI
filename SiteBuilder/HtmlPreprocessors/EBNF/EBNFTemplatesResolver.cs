using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

                var intermediateContent = modifiedDoc.Text;

                modifiedDoc = new HtmlDocument();
                modifiedDoc.LoadHtml(intermediateContent);
            };

            //_logger.Info("End");

            return modifiedDoc.Text;
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

                //_logger.Info($"ExploreTemplateNodeskind = '{kind}'");

                var itemsList = gEBNFCStorage.GetGroup(name);

                //_logger.Info($"ExploreTemplateNodesitemsList = {JsonConvert.SerializeObject(itemsList, Formatting.Indented)}");

                if (!itemsList.Any())
                {
                    rootNode.Remove();

                    return true;
                }

                var resultList = new List<string>();

                foreach (var item in itemsList)
                {
                    //_logger.Info($"item = '{item}'");

                    if (kind == "op_and")
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

                if (kind == "or")
                {
                    separator = " | ";
                }

                var resultStr = string.Join(separator, resultList);

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
