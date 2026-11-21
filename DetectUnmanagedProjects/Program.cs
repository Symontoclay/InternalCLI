using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.DetectUnmanagedProjects;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;

namespace DetectUnmanagedProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();

#if DEBUG
            //_logger.Info($"appSettings.CheckedSolutions = {JsonConvert.SerializeObject(appSettings.CheckedSolutions, Formatting.Indented)}");
#endif

            var options = new DetectUnmanagedProjectsDevTaskOptions()
            {
                CheckedSolutions = appSettings.CheckedSolutions
            };

            DeploymentPipeline.Run(new DetectUnmanagedProjectsDevTask(options));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
