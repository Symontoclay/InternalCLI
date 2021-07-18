using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder
{
    public static class PagesPathsHelper
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        public static string PathToRelativeHref(string path, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var pos = path.IndexOf(generalSiteBuilderSettings.SiteName);
            return path.Substring(pos).Replace(generalSiteBuilderSettings.SiteName, string.Empty).ToLower();
        }

        public static string RelativeHrefToAbsolute(string relativeHref, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            var domainHref = generalSiteBuilderSettings.SiteHref;

#if DEBUG
            //_logger.Info($"RelativeHrefToAbsolute relativeHref = {relativeHref}");
            //_logger.Info($"RelativeHrefToAbsolute domainHref = {domainHref}");
#endif

            if (relativeHref.StartsWith(domainHref))
            {
                return domainHref.Replace("\\", "/");
            }

            return $"{domainHref}{relativeHref}".Replace("\\", "/");
        }
    }
}
