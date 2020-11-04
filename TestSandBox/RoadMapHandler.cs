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

            foreach (var item in items)
            {
                _logger.Info($"item.Name = {item.Name}");
                _logger.Info($"item.KindOfCompeltion = {item.KindOfCompeltion}");
                _logger.Info($"item.Start = {item.Start}");
                _logger.Info($"item.End = {item.End}");
                _logger.Info($"item.ExpectedDuration = {item.ExpectedDuration}");
                _logger.Info($"item.KindOfExpectedDuration = {item.KindOfExpectedDuration}");
            }

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private List<RoadMapItem> CreateItemsList()
        {
            var items = new List<RoadMapItem>()
            {
                new RoadMapItem()
                {
                    Name = "Step 7",
                    //Start = new DateTime(2020, 3, 1)
                    Start = new DateTime(2020, 10, 5)
                },
                new RoadMapItem()
                {
                    Name = "Step 8"
                },
                new RoadMapItem()
                {
                    Name = "Step 9"
                },
                new RoadMapItem()
                {
                    Name = "Step 10"
                },
                new RoadMapItem()
                {
                    Name = "Step 11"
                },
                new RoadMapItem()
                {
                    Name = "Step 12"
                },
                new RoadMapItem()
                {
                    Name = "Step 13"
                },
                new RoadMapItem()
                {
                    Name = "Step 14"
                }
            };

            return items;
        }
    }
}
