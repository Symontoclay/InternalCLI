using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.EBNF
{
    public static class EBNFHelpers
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        public static List<string> ParseGrammarBlock(string html)
        {
            var itemsStrList = html.Split(new string[1] { $".{Environment.NewLine}" }, StringSplitOptions.RemoveEmptyEntries).Where(p => !string.IsNullOrWhiteSpace(p));

            var result = new List<string>();
            var buffer = new List<string>();

            foreach (var item in itemsStrList)
            {
                result.Add($"{item.Replace(Environment.NewLine, "").Trim()} .");
            }

            return result;
        }
    }
}
