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
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        public static List<string> ParseGrammarBlock(string html)
        {
#if DEBUG
            //_logger.Info($"html = {html}");
#endif

            var result = new List<string>();

            var sb = new StringBuilder();

            foreach(var ch in html)
            {
                var intCh = (int)ch;

#if DEBUG
                //_logger.Info($"ch = '{ch}';(int)ch = {intCh}");
#endif

                switch(intCh)
                {
                    case 10:
                    case 13:
                        FillUpSb(result, sb);
                        sb = new StringBuilder();
                        break;

                    default:
                        sb.Append(ch);
                        break;
                }
            }

            FillUpSb(result, sb);

            return result;
        }

        private static void FillUpSb(List<string> result, StringBuilder sb)
        {
            if(sb == null)
            {
                return;
            }

            var str = sb.ToString().Trim();

#if DEBUG
            //_logger.Info($"str = '{str}'");
#endif

            if(string.IsNullOrWhiteSpace(str))
            {
                return;
            }

            if(!str.EndsWith("."))
            {
                str = $"{str}.";
            }

#if DEBUG
            //_logger.Info($"str (after) = '{str}'");
#endif

            result.Add(str);
        }
    }
}
