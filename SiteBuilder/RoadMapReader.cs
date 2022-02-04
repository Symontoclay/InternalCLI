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
    public static class RoadMapReader
    {
        public static List<RoadMapItem> ReadAndPrepare(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                return new List<RoadMapItem>();
            }

            var jsonStr = File.ReadAllText(fileName);

            if(string.IsNullOrWhiteSpace(jsonStr))
            {
                return new List<RoadMapItem>();
            }

            var items = JsonConvert.DeserializeObject<List<RoadMapItem>>(jsonStr);

            return items;
        }
    }
}
