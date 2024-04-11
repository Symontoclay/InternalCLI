using CommonUtils.DeploymentTasks;
using Deployment.ReleaseTasks.StartNewVersion;
using Newtonsoft.Json;
using NLog;
using System;
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

            if (!args.Any())
            {
                Console.WriteLine("You should specify new version as CLI argument!");
                Console.WriteLine("Press any key for exit!");
                Console.ReadKey();
                return;
            }

            if(args.Length > 1)
            {
                Console.WriteLine($"The CLI should receive only one argument which specifies new version! But got {args.Length} arguments.");
                Console.WriteLine("Press any key for exit!");
                Console.ReadKey();
                return;
            }

            var newVersion = args.Single();

            try
            {
                var version = new Version(newVersion);
            }
            catch
            {
                Console.WriteLine($"The new version has incorrect format!");
                Console.WriteLine("Press any key for exit!");
                Console.ReadKey();
                return;
            }

            if(newVersion.Count(p => p == '.') != 2)
            {
                Console.WriteLine($"The new version has incorrect format!");
                Console.WriteLine("Press any key for exit!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"This app will start new version {newVersion}. Are you sure?");
            Console.WriteLine("Press 'y' or 'Y' for release or other else key for cancel starting new version.");
            Console.WriteLine("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                Console.WriteLine("Starting new version has been cancelled.");

                return;
            }

            Console.WriteLine("Starting new version is being.");

            DeploymentPipeline.Run(new StartNewVersionReleaseTask(new StartNewVersionReleaseTaskOptions
            {
                Version = newVersion
            }), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = Directory.GetCurrentDirectory()
            });
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
