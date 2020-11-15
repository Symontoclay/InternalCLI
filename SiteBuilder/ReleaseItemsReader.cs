using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder
{
    public static class ReleaseItemsReader
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static List<ReleaseItem> Read(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return new List<ReleaseItem>();
            }

            var jsonStr = File.ReadAllText(fileName);

            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return new List<ReleaseItem>();
            }

            var result = JsonConvert.DeserializeObject<List<ReleaseItem>>(jsonStr).OrderBy(p => p.Date).ToList();

            PrepareItems(result);

            return result;
        }

        private static void PrepareItems(List<ReleaseItem> items)
        {
            var minDate = items.Min(p => p.Date);

#if DEBUG
            _logger.Info($"maxDate = {minDate}");
#endif

            var latestItem = items.SingleOrDefault(p => p.Date == minDate);

#if DEBUG
            _logger.Info($"latestItem = {latestItem}");
#endif

            latestItem.IsLatest = true;
        }
    }
}
