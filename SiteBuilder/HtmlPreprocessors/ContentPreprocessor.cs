using CommonMark;
using NLog;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using SiteBuilder.HtmlPreprocessors.CSharpUserApiContentsGeneration;
using SiteBuilder.HtmlPreprocessors.EBNF;
using SiteBuilder.HtmlPreprocessors.InThePageContentGen;
using SiteBuilder.HtmlPreprocessors.ReleaseInfoGeneration;
using SiteBuilder.HtmlPreprocessors.RoadmapGeneration;
using SiteBuilder.HtmlPreprocessors.ShortTags;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors
{
    public static class ContentPreprocessor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string Run(string initialContent, bool useMarkdown, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            return Run(initialContent, useMarkdown? MarkdownStrategy.ConvertMarkdownToHtml : MarkdownStrategy.None, generalSiteBuilderSettings);
        }

        public static string Run(string initialContent, MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            if (markdownStrategy == MarkdownStrategy.ConvertMarkdownToHtml)
            {
                initialContent = CommonMarkConverter.Convert(initialContent);
            }

            var newContent = string.Empty;
            var oldContent = initialContent;

            var n = 0;

            while ((newContent = PreprocessContent(oldContent, markdownStrategy, generalSiteBuilderSettings)) != oldContent)
            {
#if DEBUG
                //_logger.Info($"oldContent = '{oldContent}'");
                //_logger.Info($"newContent = '{newContent}'");
                //_logger.Info($"n = {n}");
                //_logger.Info($"oldContent.Length = {oldContent.Length}");
                //_logger.Info($"newContent.Length = {newContent.Length}");
                //_logger.Info($"oldContent == newContent = {oldContent == newContent}");
#endif

                oldContent = newContent;

                n++;

                if (n > 100)
                {
                    throw new NotSupportedException($"Too much iterations!!!");
                }
            }

            return newContent;
        }

        private static string PreprocessContent(string initialContent, MarkdownStrategy markdownStrategy, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var content = EBNFPreparation.Run(initialContent);

#if DEBUG
            //_logger.Info($"content (1) = {content}");
#endif

            content = ShortTagsPreparation.Run(content, markdownStrategy, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"content (2) = {content}");
#endif

            content = CodeHighlighter.Run(content, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"content (3) = {content}");
#endif

            content = RoadmapGenerator.Run(content, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"content (4) = {content}");
#endif

            content = ReleaseInfoGenerator.Run(content, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"content (5) = {content}");
#endif

            content = CSharpUserApiContentsGenerator.Run(content, generalSiteBuilderSettings);

#if DEBUG
            //_logger.Info($"content (6) = {content}");
#endif

            return InThePageContentGenerator.Run(content);
        }
    }
}
