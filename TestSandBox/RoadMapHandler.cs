using NLog;
using SiteBuilder;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestSandBox
{
    public class RoadMapHandler
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public void Run()
        {
            _logger.Info("Begin");

            var fullFileName = Path.Combine(Directory.GetCurrentDirectory(), "roadmap.json");

            _logger.Info($"fullFileName = {fullFileName}");

            var items = RoadMapReader.ReadAndPrepare(fullFileName);

            //var items = CreateItemsList();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }
    }
}
