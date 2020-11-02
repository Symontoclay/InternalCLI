using CommonMark;
using NLog;
using SiteBuilder.HtmlPreprocessors.EBNF;
using SiteBuilder.HtmlPreprocessors.InThePageContentGen;
using SiteBuilder.HtmlPreprocessors.ShortTags;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder
{
    public class PageProcessor
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public PageProcessor(SiteElementInfo siteElement)
        {
            _siteElement = siteElement;
        }

        private readonly SiteElementInfo _siteElement;

        public void Run()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            var sitePageInfo = _siteElement.SitePageInfo;

            if (!sitePageInfo.IsReady)
            {
                return;
            }

            CreateFile();
        }

        private StringBuilder mResult;

        private void CreateFile()
        {
            mResult = new StringBuilder();
            GenerateText();

            using (var tmpTextWriter = new StreamWriter(_siteElement.TargetFullFileName, false, new UTF8Encoding(false)))
            {
                tmpTextWriter.Write(mResult.ToString());
                tmpTextWriter.Flush();
            }
        }

        private void GenerateText()
        {
#if DEBUG
            //_logger.Info($" = {}");
#endif

            //throw new NotImplementedException();
        }

        private string GetContent()
        {
            var content = File.ReadAllText(_siteElement.THtmlFullFileName);

#if DEBUG
            _logger.Info($"content = {content}");
#endif

            if(_siteElement.SitePageInfo.UseMarkdown)
            {
                return CommonMarkConverter.Convert(content);
            }

            return PreprocessContent(content);
        }

        private string PreprocessContent(string initialContent)
        {
            var content = EBNFPreparation.Run(initialContent);


#if DEBUG
            _logger.Info($"content (1) = {content}");
#endif

            content = ShortTagsPreparation.Run(content);

#if DEBUG
            _logger.Info($"content (2) = {content}");
#endif

            return InThePageContentGenerator.Run(content);
        }

        private string GetTitle()
        {
            var sitePageInfo = _siteElement.SitePageInfo;

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(GeneralSettings.SiteSettings.MainTitle))
            {
                sb.Append(GeneralSettings.SiteSettings.MainTitle);

                if (!string.IsNullOrWhiteSpace(GeneralSettings.SiteSettings.TitlesDelimiter))
                {
                    sb.Append(GeneralSettings.SiteSettings.TitlesDelimiter);
                }
            }

            if (string.IsNullOrWhiteSpace(sitePageInfo.Title))
            {
                sb.Append(GeneralSettings.SiteSettings.Title);
            }
            else
            {
                sb.Append(sitePageInfo.Title);
            }

            return sb.ToString().Trim();
        }
    }
}
