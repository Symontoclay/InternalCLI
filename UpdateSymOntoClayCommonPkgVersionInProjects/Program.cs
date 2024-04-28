using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CommonPackages.UpdateSymOntoClayCommonPkgVersionInProgects;
using NLog;

namespace UpdateSymOntoClayCommonPkgVersionInProjects
{
    public class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DeploymentPipeline.Run(new UpdateSymOntoClayCommonPkgVersionInProjectsDevTask());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
