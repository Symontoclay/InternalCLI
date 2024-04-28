using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CommonPackages.IncreaseSymOntoClayCommonPkgVersion;
using NLog;

namespace IncreaseSymOntoClayCommonPkgVersion
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DeploymentPipeline.Run(new IncreaseSymOntoClayCommonPkgVersionDevTask());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
