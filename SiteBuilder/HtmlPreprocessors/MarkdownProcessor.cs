using Markdig;
using NLog;

namespace SiteBuilder.HtmlPreprocessors
{
    public static class MarkdownProcessor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        static MarkdownProcessor()
        {
            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
        }

        private static MarkdownPipeline _pipeline;

        public static string MarkdownToHtml(string markdown)
        {
#if DEBUG
            _logger.Info($"markdown = {markdown}");
#endif

            if(string.IsNullOrWhiteSpace(markdown))
            {
                return string.Empty;
            }

            return Markdown.ToHtml(markdown, _pipeline);
        }
    }
}
