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

        public static List<ReleaseItem> Read(string fileName, string baseHref, string basePath, string sourceDir, string destDir)
        {
#if DEBUG
            //_logger.Info($"fileName = {fileName}");
            //_logger.Info($"baseHref = {baseHref}");
            //_logger.Info($"basePath = {basePath}");
            //_logger.Info($"sourceDir = {sourceDir}");
            //_logger.Info($"destDir = {destDir}");
#endif

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

            PrepareItems(result, baseHref, basePath, sourceDir, destDir);

            return result;
        }

        private static void PrepareItems(List<ReleaseItem> items, string baseHref, string basePath, string sourceDir, string destDir)
        {
            var maxDate = items.Max(p => p.Date);

#if DEBUG
            //_logger.Info($"maxDate = {maxDate}");
#endif

            var latestItem = items.SingleOrDefault(p => p.Date == maxDate);

#if DEBUG
            //_logger.Info($"latestItem = {latestItem}");
#endif

            latestItem.IsLatest = true;

            var relativePath = basePath.Replace(sourceDir, string.Empty);

#if DEBUG
            //_logger.Info($"relativePath = {relativePath}");
#endif

            foreach (var item in items)
            {
#if DEBUG
                //_logger.Info($"item = {item}");
#endif

                var relativeItemName = Path.Combine(relativePath, $"{item.Version}.html");

#if DEBUG
                //_logger.Info($"relativeItemName = {relativeItemName}");
#endif

                item.Href = Path.Combine(baseHref, relativeItemName).Replace("\\", "/");
                item.TargetFullFileName = Path.Combine(destDir, relativeItemName).Replace("\\", "/");

#if DEBUG
                //_logger.Info($"item (after) = {item}");
#endif
            }
        }
    }
}
