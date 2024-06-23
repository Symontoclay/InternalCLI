using CommonUtils.DeploymentTasks;
using Deployment.ReleaseTasks.CheckReadinessForRelease;
using NLog;
using SymOntoClay.CLI.Helpers;
using System.Configuration;

namespace CheckReadinessForRelease
{
    public class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            NLogSetupHelper.UseAppConfig();

            var writeOutputToTextFileAsParallel = bool.Parse(ConfigurationManager.AppSettings["ConsoleWrapper.WriteOutputToTextFileAsParallel"] ?? "false");
            var useNLogLogger = bool.Parse(ConfigurationManager.AppSettings["ConsoleWrapper.UseNLogLogger"] ?? "false");
            var writeCopyright = bool.Parse(ConfigurationManager.AppSettings["ConsoleWrapper.WriteCopyright"] ?? "false");

#if DEBUG
            //_logger.Info($"writeOutputToTextFileAsParallel = {writeOutputToTextFileAsParallel}");
            //_logger.Info($"useNLogLogger = {useNLogLogger}");
            //_logger.Info($"writeCopyright = {writeCopyright}");
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            if (useNLogLogger)
            {
                ConsoleWrapper.SetNLogLogger(_logger);
            }

            if (writeOutputToTextFileAsParallel)
            {
                ConsoleWrapper.WriteOutputToTextFileAsParallel = true;
            }

            if (writeCopyright)
            {
                ConsoleWrapper.WriteCopyright();
            }

            DeploymentPipeline.Run(new CheckReadinessForReleaseTask(new CheckReadinessForReleaseTaskOptions()
            {
                OutputFileName = "ReadinessForReleaseReport.log"
            }));
        }
    }
}
