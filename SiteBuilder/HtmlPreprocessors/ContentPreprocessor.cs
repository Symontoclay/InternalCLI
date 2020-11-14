using CommonMark;
using NLog;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using SiteBuilder.HtmlPreprocessors.EBNF;
using SiteBuilder.HtmlPreprocessors.InThePageContentGen;
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

        public static string Run(string initialContent, bool useMarkdown)
        {
            if(useMarkdown)
            {
                return CommonMarkConverter.Convert(initialContent);
            }

            return PreprocessContent(initialContent);
        }

        private static string PreprocessContent(string initialContent)
        {
            var content = EBNFPreparation.Run(initialContent);

#if DEBUG
            _logger.Info($"content (1) = {content}");
#endif

            content = ShortTagsPreparation.Run(content);

#if DEBUG
            _logger.Info($"content (2) = {content}");
#endif

            content = CodeHighlighter.Run(content);

#if DEBUG
            _logger.Info($"content (3) = {content}");
#endif

            content = RoadmapGenerator.Run(content);

#if DEBUG
            _logger.Info($"content (4) = {content}");
#endif

            return InThePageContentGenerator.Run(content);
        }
    }
}
