using CommonUtils;
using NLog;
using System;
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

            EVPath.RegVar("APPDIR", Directory.GetCurrentDirectory());

            var siteBuilder = new SiteBuilder();
            siteBuilder.Run();

#if DEBUG
            _logger.Info("End");
#endif
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
