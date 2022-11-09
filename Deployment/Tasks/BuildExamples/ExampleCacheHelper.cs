using CommonUtils;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildExamples
{
    public static class ExampleCacheHelper
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string GetFileName(string lngExamplesPage, string exampleName)
        {
            return $"{MD5Helper.GetHash(lngExamplesPage)}_{exampleName}.json";
        }

        public static bool IsNeedToBuild(CodeExample codeExample, string lngExamplesPage, string cacheDir)
        {
#if DEBUG
            //_logger.Info($"codeExample = {codeExample}");
            //_logger.Info($"lngExamplesPage = '{lngExamplesPage}'");
            //_logger.Info($"cacheDir = '{cacheDir}'");
#endif

            var fileName = GetFileName(lngExamplesPage, codeExample.Name);

#if DEBUG
            //_logger.Info($"fileName = '{fileName}'");
#endif

            var fullFileName = Path.Combine(cacheDir, fileName);

#if DEBUG
            //_logger.Info($"fullFileName = '{fullFileName}'");
#endif

            if(!File.Exists(fullFileName))
            {
                return true;
            }

            var item = JsonConvert.DeserializeObject<ExampleCacheItem>(File.ReadAllText(fullFileName));

#if DEBUG
            //_logger.Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");
#endif

            if(item.Code != Base64Helper.GetBase64String(codeExample.Code))
            {
                return true;
            }

            return false;
        }

        public static void SaveToCache(CodeExample codeExample, string lngExamplesPage, string cacheDir)
        {
#if DEBUG
            //_logger.Info($"codeExample = {codeExample}");
            //_logger.Info($"lngExamplesPage = '{lngExamplesPage}'");
            //_logger.Info($"cacheDir = '{cacheDir}'");
#endif

            var fileName = GetFileName(lngExamplesPage, codeExample.Name);

#if DEBUG
            //_logger.Info($"fileName = '{fileName}'");
#endif

            var fullFileName = Path.Combine(cacheDir, fileName);

#if DEBUG
            //_logger.Info($"fullFileName = '{fullFileName}'");
#endif

            var item = new ExampleCacheItem();
            item.PageName = lngExamplesPage;
            item.Name = codeExample.Name;
            item.Code = Base64Helper.GetBase64String(codeExample.Code);

#if DEBUG
            //_logger.Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");
#endif

            File.WriteAllText(fullFileName, JsonConvert.SerializeObject(item, Formatting.Indented));
        }
    }
}
