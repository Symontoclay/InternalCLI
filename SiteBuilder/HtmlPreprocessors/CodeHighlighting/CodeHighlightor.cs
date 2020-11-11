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

        private enum KindOfLng
        {
            CSharp,
            SymOntoClay
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
                        ProcessLng(rootNode, doc, KindOfLng.CSharp);
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

        private static void ProcessLng(HtmlNode rootNode, HtmlDocument doc, KindOfLng kindOfLng)
        {
#if DEBUG
            _logger.Info($"kindOfLng = {kindOfLng}");
#endif

            var initialText = rootNode.GetDirectInnerText();

#if DEBUG
            _logger.Info($"initialText = {initialText}");
#endif

            var newCodeNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newCodeNode, rootNode);

            var codeList = ClearCode(initialText);

            var startInMultiLineComment = false;

            foreach(var codeLineItem in codeList)
            {
                var codeLine = codeLineItem;

#if DEBUG
                _logger.Info($"codeLine = '{codeLine}'");
                _logger.Info($"startInMultiLineComment = {startInMultiLineComment}");
#endif

                var codeLineNode = doc.CreateElement("div");
                newCodeNode.AppendChild(codeLineNode);

                var sb = new StringBuilder();

                var needRun = true;

                while(needRun)
                {
                    var targetPositionsList = GetTargetPositionsList(codeLine);

#if DEBUG
                    _logger.Info($"targetPositionsList = {JsonConvert.SerializeObject(targetPositionsList, Formatting.Indented)}");
#endif

                    if(!targetPositionsList.Any())
                    {
                        ProcessCodeLineChank(codeLineNode, sb, codeLine, startInMultiLineComment, kindOfLng);
                        needRun = false;
                        break;
                    }

                    var minPos = targetPositionsList.Min(p => p.Item2);

#if DEBUG
                    _logger.Info($"minPos = {minPos}");
#endif

                    var targetPosition = targetPositionsList.Single(p => p.Item2 == minPos);

#if DEBUG
                    _logger.Info($"targetPosition = {targetPosition}");
#endif

                    var kindOfPosition = targetPosition.Item1;
                    var targetPos = targetPosition.Item2;

                    switch (kindOfPosition)
                    {
                        case KindOfPosition.SingleLineComment:
                            if(startInMultiLineComment)
                            {
                                ProcessCodeLineChank(codeLineNode, sb, "//", true, kindOfLng);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                _logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                _logger.Info($"chank = {chank}");
#endif

                                if(!string.IsNullOrWhiteSpace(chank))
                                {
                                    ProcessCodeLineChank(codeLineNode, sb, chank, false, kindOfLng);
                                }

                                var comment = codeLine.Substring(targetPos);

#if DEBUG
                                _logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChank(codeLineNode, sb, comment, true, kindOfLng);

                                needRun = false;
                            }
                            break;

                        case KindOfPosition.BeginMultiLineComment:
                            if (startInMultiLineComment)
                            {
                                ProcessCodeLineChank(codeLineNode, sb, "/*", true, kindOfLng);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                _logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                _logger.Info($"chank = {chank}");
#endif

                                if (!string.IsNullOrWhiteSpace(chank))
                                {
                                    ProcessCodeLineChank(codeLineNode, sb, chank, false, kindOfLng);
                                }

                                codeLine = codeLine.Substring(targetPos);

#if DEBUG
                                _logger.Info($"codeLine = {codeLine}");
#endif

                                var endMultilineCommentPos = codeLine.IndexOf("*/");

#if DEBUG
                                _logger.Info($"endMultilineCommentPos = {endMultilineCommentPos}");
#endif

                                if(endMultilineCommentPos == -1)
                                {
                                    startInMultiLineComment = true;

                                    ProcessCodeLineChank(codeLineNode, sb, codeLine, true, kindOfLng);

                                    needRun = false;
                                }
                                else
                                {
                                    var comment = codeLine.Substring(0, endMultilineCommentPos + 2);

#if DEBUG
                                    _logger.Info($"comment = {comment}");
#endif

                                    ProcessCodeLineChank(codeLineNode, sb, comment, true, kindOfLng);

                                    codeLine = codeLine.Substring(endMultilineCommentPos + 2);

#if DEBUG
                                    _logger.Info($"codeLine = {codeLine}");
#endif
                                }                                
                            }
                            break;

                        case KindOfPosition.EndMultiLineComment:
                            {
                                var comment = codeLine.Substring(0, targetPos + 2);

#if DEBUG
                                _logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChank(codeLineNode, sb, comment, true, kindOfLng);

                                codeLine = codeLine.Substring(targetPos + 2);

#if DEBUG
                                _logger.Info($"codeLine = {codeLine}");
#endif

                                startInMultiLineComment = false;
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(kindOfPosition), kindOfPosition, null);
                    }
                }

                codeLineNode.InnerHtml = sb.ToString();
            }
        }

        private static void ProcessCodeLineChank(HtmlNode rootNode, StringBuilder sb, string chank, bool isComment, KindOfLng kindOfLng)
        {
#if DEBUG
            _logger.Info($"chank = {chank}");
            _logger.Info($"isComment = {isComment}");
            _logger.Info($"kindOfLng = {kindOfLng}");
#endif

            if(isComment)
            {
                sb.Append($"<span style='color:lime;'>{chank}</span>");
            }
            else
            {
                sb.Append(chank);
                //throw new NotImplementedException();
            }
        }

        private static List<(KindOfPosition, int)> GetTargetPositionsList(string text)
        {
#if DEBUG
            _logger.Info($"text = {text}");
#endif

            var result = new List<(KindOfPosition, int)>();

            var singleLinePos = text.IndexOf("//");

#if DEBUG
            _logger.Info($"singleLinePos = {singleLinePos}");
#endif

            if(singleLinePos != -1)
            {
                result.Add((KindOfPosition.SingleLineComment, singleLinePos));
            }

            var beginMultiLinePos = text.IndexOf("/*");

#if DEBUG
            _logger.Info($"beginMultiLinePos = {beginMultiLinePos}");
#endif

            if (beginMultiLinePos != -1)
            {
                result.Add((KindOfPosition.BeginMultiLineComment, beginMultiLinePos));
            }

            var endMultiLinePos = text.IndexOf("*/");

#if DEBUG
            _logger.Info($"endMultiLinePos = {endMultiLinePos}");
#endif

            if (endMultiLinePos != -1)
            {
                result.Add((KindOfPosition.EndMultiLineComment, endMultiLinePos));
            }

            return result;
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
