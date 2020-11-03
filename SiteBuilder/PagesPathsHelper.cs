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
        public static string PathToRelativeHref(string path)
        {
            var pos = path.IndexOf(GeneralSettings.SiteName);
            return path.Substring(pos).Replace(GeneralSettings.SiteName, string.Empty).ToLower();
        }

        public static string RelativeHrefToAbsolute(string relativeHref)
        {
            var domainHref = $"https://{GeneralSettings.SiteName}";

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
