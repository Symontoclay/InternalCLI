﻿using CommonUtils.DebugHelpers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.CodeHighlighting
{
    public static class CodeHighlighter
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static List<string> _cSharpKeyWordsList = new List<string>()
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal",
            "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object",
            "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "alias", "ascending", "async", "await", "by", "descending",
            "dynamic", "equals", "from", "get", "global", "group", "into", "join", "let", "nameof", "notnull", "on", "orderby", "partial", "remove",
            "select", "set", "unmanaged", "value", "var", "when", "where", "yield"
        };

        private static List<string> _symOntoClayKeyWordsList = new List<string>() 
        {
            "npc","class", "world", "host", "is", "on",  "select", "insert", "not", "use"//, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };

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
            //_logger.Info($"initialContent = {initialContent}");
#endif

            var doc = new HtmlDocument();
            doc.LoadHtml(initialContent);

            ProcessNodes(doc.DocumentNode, doc);

            return doc.ToHtmlString();
        }

        private static void ProcessNodes(HtmlNode rootNode, HtmlDocument doc)
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
                var isProcessedStr = rootNode.GetAttributeValue("is_processed", null);

                if(!string.IsNullOrWhiteSpace(isProcessedStr) && bool.Parse(isProcessedStr) == true)
                {
                    return;
                }

                var language = rootNode.GetAttributeValue("data-lng", null);

#if DEBUG
                //_logger.Info($"language = {language}");
#endif

                switch(language)
                {
                    case "c#":
                    case "C#":
                    case "csharp":
                        ProcessLng(rootNode, doc, KindOfLng.CSharp);
                        return;

                    case "soc":
                    case "SymOntoClay":
                    case "symontoclay":
                        ProcessLng(rootNode, doc, KindOfLng.SymOntoClay);
                        return;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
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
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            var initialText = rootNode.GetDirectInnerText();

#if DEBUG
            //_logger.Info($"initialText = {initialText}");
#endif

            var codeList = ClearCode(initialText);

            var newCodeNode = doc.CreateElement("div");
            var parentNode = rootNode.ParentNode;
            parentNode.ReplaceChild(newCodeNode, rootNode);

            newCodeNode.AddClass("code-viewer");

            var buttonsBarNode = doc.CreateElement("div");
            newCodeNode.AppendChild(buttonsBarNode);

            var copyButtonNode = doc.CreateElement("button");
            buttonsBarNode.AppendChild(copyButtonNode);                 

            var copyButtonId = $"id{Guid.NewGuid().ToString("D").Replace("-", "_")}";

            copyButtonNode.InnerHtml = "Copy";
            copyButtonNode.AddClass("btn btn-outline-info code-viewer-btn");
            copyButtonNode.SetAttributeValue("title", "Copy to clipboard");
            copyButtonNode.Id = copyButtonId;

            var scriptNode = doc.CreateElement("script");

            var normalizedCode = NormalizeCodeForClipboard(codeList, kindOfLng);

#if DEBUG
            //_logger.Info($"normalizedCode = {normalizedCode}");
#endif

            var base64Array = Encoding.UTF8.GetBytes(normalizedCode);

            var base64Str = Convert.ToBase64String(base64Array);

            var scriptSb = new StringBuilder();

            scriptSb.Append($"$('#{copyButtonId}').click(function() {{navigator.clipboard.writeText(atob('{base64Str}'));}});");

            scriptNode.InnerHtml = scriptSb.ToString();

            newCodeNode.AppendChild(scriptNode);

            var exampleHref = rootNode.GetAttributeValue("example-href", null);

#if DEBUG
            //_logger.Info($"exampleHref = {exampleHref}");
#endif

            if(!string.IsNullOrWhiteSpace(exampleHref))
            {
                if (!exampleHref.StartsWith("https://") && !exampleHref.StartsWith("http://"))
                {
                    if (exampleHref.StartsWith("/"))
                    {
                        exampleHref = $"{GeneralSettings.SiteHref}{exampleHref}";
                    }
                    else
                    {
                        exampleHref = $"{GeneralSettings.SiteHref}/{exampleHref}";
                    }
                }

#if DEBUG
                //_logger.Info($"exampleHref (after) = {exampleHref}");
#endif

                var downloadButtonNode = doc.CreateElement("a");
                buttonsBarNode.AppendChild(downloadButtonNode);
                downloadButtonNode.SetAttributeValue("href", exampleHref);

                downloadButtonNode.AddClass("btn btn-outline-info code-viewer-btn");
                downloadButtonNode.InnerHtml = "Download";

                downloadButtonNode.SetAttributeValue("title", "Download example project");
            }

            CreateCodeLinesNodes(codeList, newCodeNode, doc, kindOfLng);
        }

        private static string NormalizeCodeForClipboard(List<string> codeList, KindOfLng kindOfLng)
        {
#if DEBUG
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            var startInMultiLineComment = false;

            uint n = 0;

            var resultSb = new StringBuilder();

            foreach (var codeLineItem in codeList)
            {
                var codeLine = codeLineItem;

#if DEBUG
                //_logger.Info($"codeLine = '{codeLine}'");
                //_logger.Info($"n = {n}");
                //_logger.Info($"startInMultiLineComment = {startInMultiLineComment}");
#endif

                var newN = n;

                var sb = new StringBuilder();

                var needRun = true;

                var startWithComment = startInMultiLineComment;

                while (needRun)
                {
                    var targetPositionsList = GetTargetPositionsList(codeLine);

#if DEBUG
                    //_logger.Info($"targetPositionsList = {JsonConvert.SerializeObject(targetPositionsList, Formatting.Indented)}");
#endif

                    if (!targetPositionsList.Any())
                    {
                        ProcessCodeLineChankInCodeNormalizing(sb, codeLine, startInMultiLineComment, kindOfLng, ref newN);
                        needRun = false;
                        break;
                    }

                    var minPos = targetPositionsList.Min(p => p.Item2);

#if DEBUG
                    //_logger.Info($"minPos = {minPos}");
#endif

                    var targetPosition = targetPositionsList.Single(p => p.Item2 == minPos);

#if DEBUG
                    //_logger.Info($"targetPosition = {targetPosition}");
#endif

                    var kindOfPosition = targetPosition.Item1;
                    var targetPos = targetPosition.Item2;

                    switch (kindOfPosition)
                    {
                        case KindOfPosition.SingleLineComment:
                            if (startInMultiLineComment)
                            {
                                ProcessCodeLineChankInCodeNormalizing(sb, "//", true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                //_logger.Info($"chank = {chank}");
#endif

                                if (string.IsNullOrWhiteSpace(chank))
                                {
                                    startWithComment = true;
                                }
                                else
                                {
                                    ProcessCodeLineChankInCodeNormalizing(sb, chank, false, kindOfLng, ref newN);
                                }

                                var comment = codeLine.Substring(targetPos);

#if DEBUG
                                //_logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChankInCodeNormalizing(sb, comment, true, kindOfLng, ref newN);

                                needRun = false;
                            }
                            break;

                        case KindOfPosition.BeginMultiLineComment:
                            if (startInMultiLineComment)
                            {
                                ProcessCodeLineChankInCodeNormalizing(sb, "/*", true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                //_logger.Info($"chank = {chank}");
#endif

                                if (string.IsNullOrWhiteSpace(chank))
                                {
                                    startWithComment = true;
                                }
                                else
                                {
                                    ProcessCodeLineChankInCodeNormalizing(sb, chank, false, kindOfLng, ref newN);
                                }

                                codeLine = codeLine.Substring(targetPos);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif

                                var endMultilineCommentPos = codeLine.IndexOf("*/");

#if DEBUG
                                //_logger.Info($"endMultilineCommentPos = {endMultilineCommentPos}");
#endif

                                if (endMultilineCommentPos == -1)
                                {
                                    startInMultiLineComment = true;

                                    ProcessCodeLineChankInCodeNormalizing(sb, codeLine, true, kindOfLng, ref newN);

                                    needRun = false;
                                }
                                else
                                {
                                    var comment = codeLine.Substring(0, endMultilineCommentPos + 2);

#if DEBUG
                                    //_logger.Info($"comment = {comment}");
#endif

                                    ProcessCodeLineChankInCodeNormalizing(sb, comment, true, kindOfLng, ref newN);

                                    codeLine = codeLine.Substring(endMultilineCommentPos + 2);

#if DEBUG
                                    //_logger.Info($"codeLine = {codeLine}");
#endif
                                }
                            }
                            break;

                        case KindOfPosition.EndMultiLineComment:
                            {
                                var comment = codeLine.Substring(0, targetPos + 2);

#if DEBUG
                                //_logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChankInCodeNormalizing(sb, comment, true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(targetPos + 2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif

                                startInMultiLineComment = false;
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(kindOfPosition), kindOfPosition, null);
                    }
                }

#if DEBUG
                //_logger.Info($"n = {n}");
                //_logger.Info($"newN = {newN}");
#endif

                int diff = (int)newN - (int)n;

#if DEBUG
                //_logger.Info($"diff = {diff}");
                //_logger.Info($"startWithComment = {startWithComment}");
#endif

                string spaces;

                if (codeLine.StartsWith("#") && !startWithComment && kindOfLng == KindOfLng.CSharp)
                {
                    spaces = string.Empty;
                }
                else
                {
                    if (diff > 0)
                    {
                        spaces = DisplayHelper.Spaces(4 * n);
                    }
                    else
                    {
                        if (diff == 0)
                        {
                            spaces = DisplayHelper.Spaces(4 * n);
                        }
                        else
                        {
                            spaces = DisplayHelper.Spaces(4 * newN);
                        }
                    }
                }

#if DEBUG
                //_logger.Info($"spaces = '{spaces}'");
                //_logger.Info($"'{spaces}{sb}'");
#endif

                resultSb.AppendLine($"{spaces}{sb}");

                n = newN;
            }

            return resultSb.ToString();
        }

        private static void ProcessCodeLineChankInCodeNormalizing(StringBuilder sb, string chank, bool isComment, KindOfLng kindOfLng, ref uint n)
        {
#if DEBUG
            //_logger.Info($"chank = {chank}");
            //_logger.Info($"isComment = {isComment}");
            //_logger.Info($"n = {n}");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            if (isComment)
            {
                sb.Append(chank);

                return;
            }

            if (chank.Contains("{"))
            {
                if (!chank.Contains("}"))
                {
                    n++;
                }
            }
            else
            {
                if (chank.Contains("}"))
                {
                    if (!chank.Contains("{"))
                    {
                        n--;
                    }
                }
            }

            var kindOfString = KindOfString.Symbol;

            StringBuilder strSb = null;

            foreach (var ch in chank)
            {
#if DEBUG
                //_logger.Info($"ch = '{ch}'; (int)ch = {(int)ch}; kindOfString = {kindOfString}; strSb = '{strSb}'");
#endif

                switch (kindOfString)
                {
                    case KindOfString.Symbol:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Word;

                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Mix;

                                break;
                            }

                            if (ch == '_')
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Mix;

                                break;
                            }

                            if (ch == '#')
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Preprocessor;

                                break;
                            }

                            sb.Append(ch);
                        }
                        break;

                    case KindOfString.Word:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            ProcessWordInCodeNormalizing(sb, strSb.ToString(), kindOfLng);
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    case KindOfString.Preprocessor:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            ProcessPreprocessorInCodeNormalizing(sb, strSb.ToString(), kindOfLng);
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    case KindOfString.Mix:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                break;
                            }

                            sb.Append(strSb.ToString());
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfString), kindOfString, null);
                }
            }

#if DEBUG
            //_logger.Info($"strSb = '{strSb}'");
            //_logger.Info($"kindOfString = {kindOfString}");
#endif

            if (strSb != null)
            {
                switch (kindOfString)
                {
                    case KindOfString.Word:
                        ProcessWordInCodeNormalizing(sb, strSb.ToString(), kindOfLng);
                        break;

                    case KindOfString.Preprocessor:
                        ProcessPreprocessorInCodeNormalizing(sb, strSb.ToString(), kindOfLng);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfString), kindOfString, null);
                }
            }
        }

        private static void ProcessWordInCodeNormalizing(StringBuilder sb, string word, KindOfLng kindOfLng)
        {
#if DEBUG
            //_logger.Info($"word = '{word}'");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            sb.Append(word);
        }

        private static void ProcessPreprocessorInCodeNormalizing(StringBuilder sb, string word, KindOfLng kindOfLng)
        {
#if DEBUG
            //_logger.Info($"word = '{word}'");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            sb.Append(word);
        }

        private static void CreateCodeLinesNodes(List<string> codeList, HtmlNode rootNode, HtmlDocument doc, KindOfLng kindOfLng)
        {
            var startInMultiLineComment = false;

            uint n = 0;

            foreach (var codeLineItem in codeList)
            {
                var codeLine = codeLineItem;

#if DEBUG
                //_logger.Info($"codeLine = '{codeLine}'");
                //_logger.Info($"n = {n}");
                //_logger.Info($"startInMultiLineComment = {startInMultiLineComment}");
#endif

                var newN = n;

                var codeLineNode = doc.CreateElement("div");
                rootNode.AppendChild(codeLineNode);
                codeLineNode.AddClass("code-viewer-line");

                var sb = new StringBuilder();

                var needRun = true;

                var startWithComment = startInMultiLineComment;

                while (needRun)
                {
                    var targetPositionsList = GetTargetPositionsList(codeLine);

#if DEBUG
                    //_logger.Info($"targetPositionsList = {JsonConvert.SerializeObject(targetPositionsList, Formatting.Indented)}");
#endif

                    if (!targetPositionsList.Any())
                    {
                        ProcessCodeLineChank(sb, codeLine, startInMultiLineComment, kindOfLng, ref newN);
                        needRun = false;
                        break;
                    }

                    var minPos = targetPositionsList.Min(p => p.Item2);

#if DEBUG
                    //_logger.Info($"minPos = {minPos}");
#endif

                    var targetPosition = targetPositionsList.Single(p => p.Item2 == minPos);

#if DEBUG
                    //_logger.Info($"targetPosition = {targetPosition}");
#endif

                    var kindOfPosition = targetPosition.Item1;
                    var targetPos = targetPosition.Item2;

                    switch (kindOfPosition)
                    {
                        case KindOfPosition.SingleLineComment:
                            if (startInMultiLineComment)
                            {
                                ProcessCodeLineChank(sb, "//", true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                //_logger.Info($"chank = {chank}");
#endif

                                if (string.IsNullOrWhiteSpace(chank))
                                {
                                    startWithComment = true;
                                }
                                else
                                {
                                    ProcessCodeLineChank(sb, chank, false, kindOfLng, ref newN);
                                }

                                var comment = codeLine.Substring(targetPos);

#if DEBUG
                                //_logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChank(sb, comment, true, kindOfLng, ref newN);

                                needRun = false;
                            }
                            break;

                        case KindOfPosition.BeginMultiLineComment:
                            if (startInMultiLineComment)
                            {
                                ProcessCodeLineChank(sb, "/*", true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif
                            }
                            else
                            {
                                var chank = codeLine.Substring(0, targetPos);

#if DEBUG
                                //_logger.Info($"chank = {chank}");
#endif

                                if (string.IsNullOrWhiteSpace(chank))
                                {
                                    startWithComment = true;
                                }
                                else
                                {
                                    ProcessCodeLineChank(sb, chank, false, kindOfLng, ref newN);
                                }

                                codeLine = codeLine.Substring(targetPos);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif

                                var endMultilineCommentPos = codeLine.IndexOf("*/");

#if DEBUG
                                //_logger.Info($"endMultilineCommentPos = {endMultilineCommentPos}");
#endif

                                if (endMultilineCommentPos == -1)
                                {
                                    startInMultiLineComment = true;

                                    ProcessCodeLineChank(sb, codeLine, true, kindOfLng, ref newN);

                                    needRun = false;
                                }
                                else
                                {
                                    var comment = codeLine.Substring(0, endMultilineCommentPos + 2);

#if DEBUG
                                    //_logger.Info($"comment = {comment}");
#endif

                                    ProcessCodeLineChank(sb, comment, true, kindOfLng, ref newN);

                                    codeLine = codeLine.Substring(endMultilineCommentPos + 2);

#if DEBUG
                                    //_logger.Info($"codeLine = {codeLine}");
#endif
                                }
                            }
                            break;

                        case KindOfPosition.EndMultiLineComment:
                            {
                                var comment = codeLine.Substring(0, targetPos + 2);

#if DEBUG
                                //_logger.Info($"comment = {comment}");
#endif

                                ProcessCodeLineChank(sb, comment, true, kindOfLng, ref newN);

                                codeLine = codeLine.Substring(targetPos + 2);

#if DEBUG
                                //_logger.Info($"codeLine = {codeLine}");
#endif

                                startInMultiLineComment = false;
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(kindOfPosition), kindOfPosition, null);
                    }
                }

#if DEBUG
                //_logger.Info($"n = {n}");
                //_logger.Info($"newN = {newN}");
#endif

                int diff = (int)newN - (int)n;

#if DEBUG
                //_logger.Info($"diff = {diff}");
                //_logger.Info($"startWithComment = {startWithComment}");
#endif

                string spaces;

                if (codeLine.StartsWith("#") && !startWithComment && kindOfLng == KindOfLng.CSharp)
                {
                    spaces = string.Empty;
                }
                else
                {
                    if (diff > 0)
                    {
                        spaces = Spaces(4 * n);
                    }
                    else
                    {
                        if (diff == 0)
                        {
                            spaces = Spaces(4 * n);
                        }
                        else
                        {
                            spaces = Spaces(4 * newN);
                        }
                    }
                }

#if DEBUG
                //_logger.Info($"spaces = '{spaces}'");
                //_logger.Info($"'{spaces}{sb}'");
#endif

                codeLineNode.InnerHtml = $"{spaces}{sb}";

                n = newN;
            }
        }

        public static string Spaces(uint n)
        {
            if (n == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < n; i++)
            {
                sb.Append("&nbsp;");
            }

            return sb.ToString();
        }

        private enum KindOfString
        {
            Symbol,
            Mix,
            Word,
            Preprocessor
        }

        private static void ProcessCodeLineChank(StringBuilder sb, string chank, bool isComment, KindOfLng kindOfLng, ref uint n)
        {
#if DEBUG
            //_logger.Info($"chank = {chank}");
            //_logger.Info($"isComment = {isComment}");
            //_logger.Info($"n = {n}");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            if(isComment)
            {
                sb.Append($"<span class='code-viewer-comment'>{chank}</span>");

                return;
            }

            if(chank.Contains("{"))
            {
                if(!chank.Contains("}"))
                {
                    n++;
                }                
            }
            else
            {
                if(chank.Contains("}"))
                {
                    if(!chank.Contains("{"))
                    {
                        n--;
                    }
                }
            }

            var kindOfString = KindOfString.Symbol;

            StringBuilder strSb = null;

            foreach (var ch in chank)
            {
#if DEBUG
                //_logger.Info($"ch = '{ch}'; (int)ch = {(int)ch}; kindOfString = {kindOfString}; strSb = '{strSb}'");
#endif

                switch (kindOfString)
                {
                    case KindOfString.Symbol:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Word;

                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Mix;

                                break;
                            }

                            if (ch == '_')
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Mix;

                                break;
                            }

                            if (ch == '#')
                            {
                                strSb = new StringBuilder();
                                strSb.Append(ch);

                                kindOfString = KindOfString.Preprocessor;

                                break;
                            }

                            sb.Append(ch);
                        }
                        break;

                    case KindOfString.Word:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            ProcessWord(sb, strSb.ToString(), kindOfLng);
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    case KindOfString.Preprocessor:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                kindOfString = KindOfString.Mix;
                                break;
                            }

                            ProcessPreprocessor(sb, strSb.ToString(), kindOfLng);
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    case KindOfString.Mix:
                        {
                            if (char.IsLetter(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (char.IsDigit(ch))
                            {
                                strSb.Append(ch);
                                break;
                            }

                            if (ch == '_')
                            {
                                strSb.Append(ch);
                                break;
                            }

                            sb.Append(strSb.ToString());
                            kindOfString = KindOfString.Symbol;
                            sb.Append(ch);
                            strSb = null;
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfString), kindOfString, null);
                }
            }

#if DEBUG
            //_logger.Info($"strSb = '{strSb}'");
            //_logger.Info($"kindOfString = {kindOfString}");
#endif

            if (strSb != null)
            {
                switch (kindOfString)
                {
                    case KindOfString.Word:
                        ProcessWord(sb, strSb.ToString(), kindOfLng);
                        break;

                    case KindOfString.Preprocessor:
                        ProcessPreprocessor(sb, strSb.ToString(), kindOfLng);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfString), kindOfString, null);
                }
            }
        }

        private static void ProcessWord(StringBuilder sb, string word, KindOfLng kindOfLng)
        {
#if DEBUG
            //_logger.Info($"word = '{word}'");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            if(IsKeyWord(word, kindOfLng))
            {
                sb.Append($"<span class='code-viewer-keyword'>{word}</span>");

                return;
            }

            sb.Append(word);
        }

        private static bool IsKeyWord(string word, KindOfLng kindOfLng)
        {
            if(kindOfLng == KindOfLng.CSharp)
            {
                return _cSharpKeyWordsList.Any(p => p == word);
            }

            return _symOntoClayKeyWordsList.Any(p => p == word);
        }

        private static void ProcessPreprocessor(StringBuilder sb, string word, KindOfLng kindOfLng)
        {
#if DEBUG
            //_logger.Info($"word = '{word}'");
            //_logger.Info($"kindOfLng = {kindOfLng}");
#endif

            if(kindOfLng == KindOfLng.CSharp)
            {
                sb.Append($"<span class='code-viewer-preprocessor-statement'>{word}</span>");

                return;
            }

            sb.Append(word);
        }

        private static List<(KindOfPosition, int)> GetTargetPositionsList(string text)
        {
#if DEBUG
            //_logger.Info($"text = {text}");
#endif

            var result = new List<(KindOfPosition, int)>();

            var singleLinePos = text.IndexOf("//");

#if DEBUG
            //_logger.Info($"singleLinePos = {singleLinePos}");
#endif

            if(singleLinePos != -1)
            {
                result.Add((KindOfPosition.SingleLineComment, singleLinePos));
            }

            var beginMultiLinePos = text.IndexOf("/*");

#if DEBUG
            //_logger.Info($"beginMultiLinePos = {beginMultiLinePos}");
#endif

            if (beginMultiLinePos != -1)
            {
                result.Add((KindOfPosition.BeginMultiLineComment, beginMultiLinePos));
            }

            var endMultiLinePos = text.IndexOf("*/");

#if DEBUG
            //_logger.Info($"endMultiLinePos = {endMultiLinePos}");
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