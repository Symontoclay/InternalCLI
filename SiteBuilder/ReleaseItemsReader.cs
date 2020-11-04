using Newtonsoft.Json;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder
{
    public static class ReleaseItemsReader
    {
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

            return JsonConvert.DeserializeObject<List<ReleaseItem>>(jsonStr);
        }
    }
}
