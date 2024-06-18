using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.ReleaseTasks.MakeRelease;
using Deployment.TestDeploymentTasks.PrepareTestDeployment;
using NLog;
using SymOntoClay.CLI.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MakeRelease
{
    static class Program
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

            var runModeResult = GetParseRunModeFromArgs(args);

            if(runModeResult.HasErrors)
            {
                return;
            }

            var runMode = runModeResult.RunMode;

            var version = FutureReleaseInfoReader.GetFutureVersion();

            switch(runMode)
            {
                case RunMode.TestFirstProdNext:
                    {
                        ConsoleWrapper.WriteText($"This app will release future version {version}.");
                        ConsoleWrapper.WriteText("First this deployment of the release will be tested on test repositories.");
                        ConsoleWrapper.WriteText("Next the release will be deployed into PROD.");

                        if(!StartReleaseQuestion())
                        {
                            return;
                        }

                        MakeReleaseOnTest();

                        ConsoleWrapper.WriteText($"Version {version} has been successfully released into test repositories.");
                        ConsoleWrapper.WriteText("Release into prod repositories has been started.");

                        MakeReleaseOnProd();
                    }
                    break;

                case RunMode.Test:
                    {
                        ConsoleWrapper.WriteText($"This app will release future version {version} on test repositories.");

                        if (!StartReleaseQuestion())
                        {
                            return;
                        }

                        MakeReleaseOnTest();
                    }
                    break;

                case RunMode.Prod:
                    {
                        ConsoleWrapper.WriteText($"This app will release future version {version} directly into PROD. Are you sure?");

                        if (!StartReleaseQuestion())
                        {
                            return;
                        }

                        MakeReleaseOnProd();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(runMode), runMode, null);
            }
        }

        private static void MakeReleaseOnTest()
        {
            _logger.Info("Begin release into test repositories.");

            DeploymentPipeline.Run(new PrepareTestDeploymentTask());

            ProjectsDataSourceFactory.Mode = ProjectsDataSourceMode.Test;

            DeploymentPipeline.Run(new MakeReleaseReleaseTask(), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = Directory.GetCurrentDirectory()
            });

            _logger.Info("Finish release into test repositories.");
        }

        private static void MakeReleaseOnProd()
        {
            _logger.Info("Begin release into PROD repositories.");

            ProjectsDataSourceFactory.Mode = ProjectsDataSourceMode.Prod;

            DeploymentPipeline.Run(new MakeReleaseReleaseTask(), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = Directory.GetCurrentDirectory()
            });

            _logger.Info("Finish release into PROD repositories.");
        }

        private static bool StartReleaseQuestion()
        {
            ConsoleWrapper.WriteText("Press 'y' or 'Y' for release or other else key for cancel release.");
            ConsoleWrapper.WriteText("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                ConsoleWrapper.WriteText("Release has been cancelled.");

                return false;
            }

            ConsoleWrapper.WriteText("Release has been started.");

            return true;
        }

        private const RunMode DEFAULT_RUN_MODE = RunMode.TestFirstProdNext;

        private static (RunMode RunMode, bool HasErrors) GetParseRunModeFromArgs(string[] args)
        {
            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            if(result.Errors.Count > 0)
            {
                foreach (var error in result.Errors)
                {
                    ConsoleWrapper.WriteError(error);
                }

                return (RunMode.Test, true);
            }

            if (result.Params.Count == 0)
            {
                return (DEFAULT_RUN_MODE, false);
            }

            var runMode = (RunMode)result.Params["RunMode"];

            return (runMode, false);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
