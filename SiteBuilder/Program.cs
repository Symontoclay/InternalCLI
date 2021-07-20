using CommonUtils;
using NLog;
using System;
using System.Configuration;
using System.IO;

namespace SiteBuilder
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var options = new SiteBuilderOptions();

            var kindOfTargetUrlStr = ConfigurationManager.AppSettings["kindOfTargetUrl"];

#if DEBUG
            //_logger.Info($"GeneralSettings() kindOfTargetUrlStr = {kindOfTargetUrlStr}");
#endif

            options.KindOfTargetUrl = ParseKindOfTargetUrl(kindOfTargetUrlStr);

            options.SiteName = ConfigurationManager.AppSettings["siteName"];

            options.SourcePath = EVPath.Normalize(ConfigurationManager.AppSettings["sourcePath"]);

            options.DestPath = EVPath.Normalize(ConfigurationManager.AppSettings["destPath"]);

            options.TempPath = ConfigurationManager.AppSettings["tempPath"];

            var siteBuilder = new SiteBuilder(options);
            siteBuilder.Run();

#if DEBUG
            _logger.Info("End");
#endif
        }

        private static KindOfTargetUrl ParseKindOfTargetUrl(string value)
        {
#if DEBUG
            //_logger.Info($"value = {value}");
#endif

            if (string.IsNullOrWhiteSpace(value))
            {
                return KindOfTargetUrl.Domain;
            }

            var firstChar = value[0];

            if (char.IsLower(firstChar))
            {
                value = value.Remove(0, 1).Insert(0, char.ToUpper(firstChar).ToString());
            }

            return (KindOfTargetUrl)Enum.Parse(typeof(KindOfTargetUrl), value);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
