using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects;
using NLog;

namespace CheckInstalledNuGetPackagesInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DeploymentPipeline.Run(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
