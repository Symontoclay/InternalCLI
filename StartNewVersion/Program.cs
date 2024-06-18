using CommonUtils.DeploymentTasks;
using Deployment.ReleaseTasks.StartNewVersion;
using Newtonsoft.Json;
using NLog;
using SymOntoClay.CLI.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace StartNewVersion
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            if (result.Errors.Count > 0)
            {
                foreach (var error in result.Errors)
                {
                    ConsoleWrapper.WriteError(error);
                }

                PrintHelp();

                ConsoleWrapper.WriteText("Press any key for exit");

                Console.ReadKey();

                return;
            }

            if (result.Params.Count == 0)
            {
                PrintHelp();

                ConsoleWrapper.WriteText("Press any key for exit");

                Console.ReadKey();

                return;
            }

            var targetVersion = (Version)result.Params["TargetVersion"];

            ConsoleWrapper.WriteText($"This app will start new version {targetVersion}. Are you sure?");
            ConsoleWrapper.WriteText("Press 'y' or 'Y' for release or other else key for cancel starting new version.");
            ConsoleWrapper.WriteText("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                ConsoleWrapper.WriteText("Starting new version has been cancelled.");

                return;
            }

            ConsoleWrapper.WriteText("Starting new version is being.");

            DeploymentPipeline.Run(new StartNewVersionReleaseTask(new StartNewVersionReleaseTaskOptions
            {
                Version = targetVersion.ToString()
            }), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = Directory.GetCurrentDirectory()
            });
        }

        private static void PrintHelp()
        {
            ConsoleWrapper.WriteText("You must specify new version as CLI argument!");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
