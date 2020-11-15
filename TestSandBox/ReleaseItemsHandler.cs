using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSandBox
{
    public class ReleaseItemsHandler
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public void Run()
        {
            _logger.Info("Begin");

            var item = new ReleaseItem()
            {
                Date = new DateTime(2020, 8, 29),
                Version = "0.2.0",
                AssetsList = new List<ReleaseAssetItem>()
                {
                    new ReleaseAssetItem()
                    {
                        Kind = KindOfReleaseAssetItem.SourceCodeZip,
                        Title = "Source code (.zip)",
                        Href = "https://github.com/Symontoclay/SymOntoClay/archive/0.2.0.0.zip"
                    }
                },
                Description = @"* Host endpoints
* @@host system variable
* Synchronous calling host method in SymOntoClay script
* Asynchronous calling host method in SymOntoClay script",
                IsMarkdown = true
            };

            var jsonStr = JsonConvert.SerializeObject(item, Formatting.Indented);

#if DEBUG
            _logger.Info($"jsonStr = {jsonStr}");
#endif

            var itemsList = new List<ReleaseItem>()
            {
                new ReleaseItem()
                {
                    Date = new DateTime(2020, 8, 29),
                    Version = "",
                    AssetsList = new List<ReleaseAssetItem>()
                    {
                        new ReleaseAssetItem()
                        {
                            Kind = KindOfReleaseAssetItem.SourceCodeZip,
                            Title = "",
                            Href = "https://github.com/Symontoclay/SymOntoClay/archive/0.2.0.0.zip"
                        }
                    },
                    Description = "",
                    IsMarkdown = true
                },
                new ReleaseItem()
                {
                    Date = new DateTime(2020, 5, 10),
                    Version = "",
                    AssetsList = new List<ReleaseAssetItem>()
                    {
                        new ReleaseAssetItem()
                        {
                            Kind = KindOfReleaseAssetItem.SourceCodeZip,
                            Title = "",
                            Href = "https://github.com/Symontoclay/SymOntoClay/archive/0.2.0.0.zip"
                        }
                    },
                    Description = "",
                    IsMarkdown = true
                }
            };

            jsonStr = JsonConvert.SerializeObject(itemsList, Formatting.Indented);

#if DEBUG
            _logger.Info($"jsonStr = {jsonStr}");
#endif

            _logger.Info("Begin");
        }
    }
}
