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
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static SiteElementInfo ConvertReleaseItemToSiteElementInfo(ReleaseItem releaseItem)
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

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            var latestMark = string.Empty;

            if (releaseItem.IsLatest)
            {
                latestMark = " (latest)";
            }

            microdata.Title = title;
            microdata.Description = $"Download SymOntoClay version {releaseItem.Version}{latestMark}";

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            return result;
        }

        public ReleaseItemPageProcessor(ReleaseItem releaseItem, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertReleaseItemToSiteElementInfo(releaseItem), generalSiteBuilderSettings)
        {
            _releaseItem = releaseItem;
        }

        private readonly ReleaseItem _releaseItem;

        protected override string GetInitialContent()
        {
            return $"<release version='{_releaseItem.Version}'/>";
        }
    }
}
