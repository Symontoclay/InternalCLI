using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.ReleaseTasks.MakeRelease;
using Deployment.TestDeploymentTasks.PrepareTestDeployment;
using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace MakeRelease
{
    static class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var runMode = GetParseRunModeFromArgs(args);

            var version = FutureReleaseInfoReader.GetFutureVersion();

            switch(runMode)
            {
                case RunMode.TestFirstProdNext:
                    {
                        Console.WriteLine($"This app will release future version {version}.");
                        Console.WriteLine("First this deployment of the release will be tested on test repositories.");
                        Console.WriteLine("Next the release will be deployed into PROD.");

                        if(!StartReleaseQuestion())
                        {
                            return;
                        }

                        MakeReleaseOnTest();

                        Console.WriteLine($"Version {version} has been successfully released into test repositories.");
                        Console.WriteLine("Release into prod repositories has been started.");

                        MakeReleaseOnProd();
                    }
                    break;

                case RunMode.Test:
                    {
                        Console.WriteLine($"This app will release future version {version} on test repositories.");

                        if (!StartReleaseQuestion())
                        {
                            return;
                        }

                        MakeReleaseOnTest();
                    }
                    break;

                case RunMode.Prod:
                    {
                        Console.WriteLine($"This app will release future version {version} directly into PROD. Are you sure?");

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

            //Console.WriteLine($"This app will release future version {version}. Are you sure?");
            //Console.WriteLine("Press 'y' or 'Y' for release or other else key for cancel release.");
            //Console.WriteLine("After your choise press enter.");

            //var key = Console.ReadLine();

            //if(key != "y" && key != "Y")
            //{
            //    Console.WriteLine("Release has been cancelled.");

            //    return;
            //}

            //Console.WriteLine("Release has been started.");

            //var task = new MakeReleaseReleaseTask();
            //task.Run();
        }

        private static void MakeReleaseOnTest()
        {
            _logger.Info("Begin release into test repositories.");

            DeploymentPipeline.Run(new PrepareTestDeploymentTask());

            ProjectsDataSourceFactory.Mode = ProjectsDataSourceMode.Test;

            DeploymentPipeline.Run(new MakeReleaseReleaseTask());

            _logger.Info("Finish release into test repositories.");
        }

        private static void MakeReleaseOnProd()
        {
            _logger.Info("Begin release into PROD repositories.");

            ProjectsDataSourceFactory.Mode = ProjectsDataSourceMode.Prod;

            DeploymentPipeline.Run(new MakeReleaseReleaseTask());

            _logger.Info("Finish release into PROD repositories.");
        }

        private static bool StartReleaseQuestion()
        {
            Console.WriteLine("Press 'y' or 'Y' for release or other else key for cancel release.");
            Console.WriteLine("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                Console.WriteLine("Release has been cancelled.");

                return false;
            }

            Console.WriteLine("Release has been started.");

            return true;
        }

        private enum RunMode
        {
            TestFirstProdNext,
            Test,
            Prod
        }

        private const RunMode DEFAULT_RUN_MODE = RunMode.TestFirstProdNext;

        private static RunMode GetParseRunModeFromArgs(string[] args)
        {
            if(!args.Any())
            {
                return DEFAULT_RUN_MODE;
            }

            if(args.Contains("test") && args.Contains("prod"))
            {
                throw new Exception("Option 'test' can not be used with option 'prod'.");
            }

            if(args.Any(p => p == "test"))
            {
                return RunMode.Test;
            }

            if(args.Any(p => p == "prod"))
            {
                return RunMode.Prod;
            }

            throw new NotImplementedException();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
