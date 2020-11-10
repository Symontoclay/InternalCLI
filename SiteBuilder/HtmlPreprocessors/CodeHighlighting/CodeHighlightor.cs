using HtmlAgilityPack;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.CodeHighlighting
{
    public static class CodeHighlightor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private enum KindOfPosition
        {
            Unknown,
            SingleLineComment,
            BeginMultiLineComment,
            EndMultiLineComment
        }

        public static string Run(string initialContent)
        {
#if DEBUG
            _logger.Info($"initialContent = {initialContent}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            ProcessNodes(doc.DocumentNode, doc);

            return doc.ToHtmlString();
        }

        private static void ProcessNodes(HtmlNode rootNode, HtmlDocument doc)
        {
#if DEBUG
            if (rootNode.Name != "#document")
            {
                _logger.Info($"rootNode.Name = '{rootNode.Name}'");
                _logger.Info($"rootNode.OuterHtml = {rootNode.OuterHtml}");
                _logger.Info($"rootNode.InnerHtml = {rootNode.InnerHtml}");
                _logger.Info($"rootNode.InnerText = {rootNode.InnerText}");
            }
#endif

            if(rootNode.Name == "code")
            {
                var isProcessedStr = rootNode.GetAttributeValue("is_processed", null);

                if(!string.IsNullOrWhiteSpace(isProcessedStr) && bool.Parse(isProcessedStr) == true)
                {
                    return;
                }

                var language = rootNode.GetAttributeValue("data-language", null);

#if DEBUG
                _logger.Info($"language = {language}");
#endif

                switch(language)
                {
                    case "c#":
                    case "C#":
                    case "csharp":
                        ProcessCSharp(rootNode, doc);
                        return;

                    //default:
                        //throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            foreach (var node in rootNode.ChildNodes.ToList())
            {
                ProcessNodes(node, doc);
            }
        }

        private static void ProcessCSharp(HtmlNode rootNode, HtmlDocument doc)
        {
            var initialText = rootNode.GetDirectInnerText();

#if DEBUG
            _logger.Info($"initialText = {initialText}");
#endif

            var newCodeNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newCodeNode, rootNode);

            var codeList = ClearCode(initialText);

            var startInMultiLineComment = false;

            foreach(var codeLine in codeList)
            {
#if DEBUG
                _logger.Info($"codeLine = '{codeLine}'");
                _logger.Info($"startInMultiLineComment = {startInMultiLineComment}");
#endif

                var codeLineNode = doc.CreateElement("div");
                newCodeNode.AppendChild(codeLineNode);

                var textBeforeComment = string.Empty;
                var textInComment = string.Empty;

                var singleLinePos = codeLine.IndexOf("//");

#if DEBUG
                _logger.Info($"singleLinePos = {singleLinePos}");
#endif

                var beginMultiLinePos = codeLine.IndexOf("/*");

#if DEBUG
                _logger.Info($"beginMultiLinePos = {beginMultiLinePos}");
#endif

                var endMultiLinePos = codeLine.IndexOf(@"*\");

#if DEBUG
                _logger.Info($"endMultiLinePos = {endMultiLinePos}");
#endif

#if DEBUG
                _logger.Info($"textBeforeComment = '{textBeforeComment}'");
                _logger.Info($"textInComment = '{textInComment}'");
#endif

                codeLineNode.InnerHtml = codeLine;
            }

            //throw new NotImplementedException();
        }

        private static List<(KindOfPosition, int)> GetTargetPositionsList(string text)
        {
#if DEBUG
            _logger.Info($"text = {text}");
#endif
        }

        private static List<string> ClearCode(string initialContent)
        {
#if DEBUG
            //_logger.Info($"initialContent = {initialContent}");
#endif

            var result = new List<string>();

            var lineSb = new StringBuilder();

            foreach(var ch in initialContent)
            {
                var intCh = (int)ch;

#if DEBUG
                //_logger.Info($"ch = '{ch}'; intCh = {intCh}");
#endif

                switch(intCh)
                {
                    case 10:
                        continue;

                    case 13:
                        {
                            var codeLine = lineSb.ToString().Trim();

                            if (!string.IsNullOrWhiteSpace(codeLine))
                            {
                                result.Add(codeLine);
                            }
                        }
                        lineSb = new StringBuilder();
                        continue;

                    default:
                        lineSb.Append(ch);
                        break;
                }                
            }

            {
                var codeLine = lineSb.ToString().Trim();

                if(!string.IsNullOrWhiteSpace(codeLine))
                {
                    result.Add(codeLine);
                }                
            }

            return result;
        }
    }
}
