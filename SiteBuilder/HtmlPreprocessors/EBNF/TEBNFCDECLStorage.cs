using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.EBNF
{
    public class TEBNFCDECLStorage
    {
        private class TEBNFCDECLStorageItem
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public Dictionary<string, string> Params { get; set; }
        }

#if DEBUG
        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        private readonly Dictionary<string, TEBNFCDECLStorageItem> _dic = new Dictionary<string, TEBNFCDECLStorageItem>();

        public void RegNode(HtmlNode node)
        {
            //_logger.Info($"node.OuterHtml = '{node.OuterHtml}'");

            var content = node.InnerHtml;

            //_logger.Info($"content = '{content}'");

            var contentList = content.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

            content = string.Join(" ", contentList).Trim();

            //_logger.Info($"content (2) = '{content}'");

            var name = node.GetAttributeValue("name", string.Empty);

            //_logger.Info($"name = '{name}'");

            var paramsList = node.Attributes.Select(p => new KeyValuePair<string, string>(p.Name, p.Value)).Where(p => p.Key.StartsWith("param"));

            //_logger.Info($"paramsList = {JsonConvert.SerializeObject(paramsList, Formatting.Indented)}");

            var item = new TEBNFCDECLStorageItem()
            {
                Name = name,
                Content = content,
                Params = paramsList.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => p.Select(x => x.Value).FirstOrDefault())
            };

            //_logger.Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");

            _dic[name] = item;
        }

        public void ProcessNode(HtmlNode node, HtmlDocument doc)
        {
#if DEBUG
            //_logger.Info($"ProcessNode node.OuterHtml = '{node.OuterHtml}'");
#endif
            var name = node.GetAttributeValue("name", string.Empty);

#if DEBUG
            //_logger.Info($"ProcessNode name = '{name}'");
#endif
            if (!_dic.ContainsKey(name))
            {
                throw new NotSupportedException($"Template `{name}` is not supported.");
            }

            var paramsList = node.Attributes.Select(p => new KeyValuePair<string, string>(p.Name, p.Value)).Where(p => p.Key.StartsWith("param"));

            //_logger.Info($"ProcessNode paramsList = {JsonConvert.SerializeObject(paramsList, Formatting.Indented)}");

            var paramsDict = paramsList.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => p.Select(x => x.Value).FirstOrDefault());

            //_logger.Info($"ProcessNode paramsDict = {JsonConvert.SerializeObject(paramsDict, Formatting.Indented)}");

            var item = _dic[name];

            //_logger.Info($"ProcessNode item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");

            var content = item.Content;

            //_logger.Info($"content = '{content}'");

            foreach (var param in paramsDict)
            {
                if (!item.Params.ContainsKey(param.Key))
                {
                    continue;
                }

                content = content.Replace(item.Params[param.Key], param.Value);
            }

            //_logger.Info($"ProcessNode content (2) = '{content}'");

            var textNode = doc.CreateTextNode(content.Trim());

            //_logger.Info($"ProcessNode textNode.OuterHtml = {textNode.OuterHtml}");

            var parentNode = node.ParentNode;
            parentNode.ReplaceChild(textNode, node);

            //_logger.Info($"ProcessNode parentNode.OuterHtml = {parentNode.OuterHtml}");
        }
    }
}
