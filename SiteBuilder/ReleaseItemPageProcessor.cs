using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder
{
    public class ReleaseItemPageProcessor: PageProcessor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static SiteElementInfo ConvertReleaseItemToSiteElementInfo(ReleaseItem releaseItem)
        {
            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = releaseItem.Href;
            result.TargetFullFileName = releaseItem.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;

            var title = $"Release notes: Version {releaseItem.Version}";

            result.BreadcrumbTitle = title;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = title;

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            throw new NotImplementedException();

            return result;
        }

        public ReleaseItemPageProcessor(ReleaseItem releaseItem)
            : base(ConvertReleaseItemToSiteElementInfo(releaseItem))
        {
        }

        protected override string GetInitialContent()
        {
            return ":)";
        }
    }
}
