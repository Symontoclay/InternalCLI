using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteBuilder.HtmlPreprocessors.CodeHighlighting
{
    public class CodeExampleReader
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static List<CodeExample> Read(string pageFileName)
        {
#if DEBUG
            //_logger.Info($"pageFileName = {pageFileName}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(File.ReadAllText(pageFileName));

            var result = new List<CodeExample>();

            ProcessNodes(doc.DocumentNode, result);

            return result;
        }

        private static void ProcessNodes(HtmlNode rootNode, List<CodeExample> result)
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

            if(rootNode.Name == "code")
            {
                var exampleNameStr = rootNode.GetAttributeValue("example_name", null);

                if(string.IsNullOrWhiteSpace(exampleNameStr))
                {
                    return;
                }

                var item = new CodeExample();
                item.Name = exampleNameStr;
                item.Code = rootNode.GetDirectInnerText();

                result.Add(item);

                return;
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, result);
            }
        }
    }
}
