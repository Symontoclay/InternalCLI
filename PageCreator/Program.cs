using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.IO;
using System.Linq;

namespace PageCreator
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if(!args.Any())
            {
                throw new Exception("Page name must be defined.");
            }

            var pageName = args[0];

            _logger.Info($"pageName = {pageName}");

            if(pageName.Contains("."))
            {
                throw new Exception($"Page name can not contain `.`. But I have got `{pageName}`.");
            }

            var targetDir = Directory.GetCurrentDirectory();

            _logger.Info($"targetDir = {targetDir}");

            var spFullFileName = Path.Combine(targetDir, $"{pageName}.sp");

            _logger.Info($"spFullFileName = {spFullFileName}");

            var pageInfo = new SitePageInfo();
            pageInfo.IsReady = true;
            pageInfo.Microdata = new MicroDataInfo();

            var jsonStr = JsonConvert.SerializeObject(pageInfo, Formatting.Indented);

            _logger.Info($"jsonStr = {jsonStr}");

            File.WriteAllText(spFullFileName, jsonStr);

            var thtmlFullFileName = Path.Combine(targetDir, $"{pageName}.thtml");

            _logger.Info($"thtmlFullFileName = {thtmlFullFileName}");

            File.WriteAllText(thtmlFullFileName, string.Empty);

            var lessFullFileName = Path.Combine(targetDir, $"{pageName}.less");

            _logger.Info($"lessFullFileName = {lessFullFileName}");

            File.WriteAllText(lessFullFileName, string.Empty);

            //_logger.Info($" = {}");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
