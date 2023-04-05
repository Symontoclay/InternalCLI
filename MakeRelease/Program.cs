using BaseDevPipeline;
using Deployment.Helpers;
using Deployment.ReleaseTasks.MakeRelease;
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

#if DEBUG
            _logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
            ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();
#endif

            var runMode = GetParseRunModeFromArgs(args);

#if DEBUG
            _logger.Info($"runMode = {JsonConvert.SerializeObject(runMode, Formatting.Indented)}");
#endif

            var version = FutureReleaseInfoReader.GetFutureVersion();

#if DEBUG
            _logger.Info($"version = {JsonConvert.SerializeObject(version, Formatting.Indented)}");
#endif

            switch(runMode)
            {
                case RunMode.TestFirstProdNext:
                    {
                        throw new NotImplementedException();
                    }
                    break;

                case RunMode.Test:
                    {
                        throw new NotImplementedException();
                    }
                    break;

                case RunMode.Prod:
                    {
                        throw new NotImplementedException();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(runMode), runMode, null);
            }

            Console.WriteLine($"This app will release future version {version}. Are you sure?");
            Console.WriteLine("Press 'y' or 'Y' for release or other else key for cancel release.");
            Console.WriteLine("After your choise press enter.");

            var key = Console.ReadLine();

            if(key != "y" && key != "Y")
            {
                Console.WriteLine("Release has been cancelled.");

                return;
            }

            Console.WriteLine("Release has been started.");

            //var task = new MakeReleaseReleaseTask();
            //task.Run();
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

            throw new NotImplementedException();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
