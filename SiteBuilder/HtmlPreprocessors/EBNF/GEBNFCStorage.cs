using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.EBNF
{
    public class GEBNFCStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, List<string>> _dict = new Dictionary<string, List<string>>();

        public void RegNode(HtmlNode node)
        {
            //_logger.Info($"node.OuterHtml = '{node.OuterHtml}'");

            var groups = node.GetAttributeValue("groups", string.Empty);

            //_logger.Info($"groups = '{groups}'");

            if (string.IsNullOrWhiteSpace(groups))
            {
                return;
            }

            var name = node.GetAttributeValue("name", string.Empty);

            //_logger.Info($"name = '{name}'");

            var groupsList = groups.Split(',', ';').Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()).Distinct();

            foreach (var groupVal in groupsList)
            {
                //_logger.Info($"groupVal = '{groupVal}'");

                if (_dict.ContainsKey(groupVal))
                {
                    _dict[groupVal].Add(name);
                }
                else
                {
                    _dict[groupVal] = new List<string>() { name };
                }
            }
        }

        public List<string> GetGroup(string group)
        {
            if (_dict.ContainsKey(group))
            {
                return _dict[group];
            }

            return new List<string>();
        }
    }
}
