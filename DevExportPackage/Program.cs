using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.DevExportPackage;
using NLog;
using System;

namespace DevExportPackage
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DeploymentPipeline.Run(new DevExportPackageDevTask());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
