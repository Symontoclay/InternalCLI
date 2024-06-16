using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects;
using Deployment.DevTasks.TargetFrameworks.UpdateTargetFrameworkInAllCSharpProjects;
using Newtonsoft.Json;
using NLog;
using SymOntoClay.CLI.Helpers;
using System.Configuration;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    internal class Program
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

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            if(result.Errors.Count > 1)
            {
                foreach(var error in result.Errors)
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

            var kindOfTargetCSharpFramework = (KindOfTargetCSharpFramework)result.Params["TargetFramework"];
            var targetVersion = (Version)result.Params["TargetVersion"];

            ConsoleWrapper.WriteText($"Version of all C# projects with target framework {kindOfTargetCSharpFramework} will be changed to {targetVersion}.");
            ConsoleWrapper.WriteText("Are you sure?");
            ConsoleWrapper.WriteText("Press 'y' or 'Y' for continue or other else key for cancel.");
            ConsoleWrapper.WriteText("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                ConsoleWrapper.WriteText("Updating has been cancelled.");

                return;
            }

            ConsoleWrapper.WriteText("Updating has been started.");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateTargetFrameworkInAllCSharpProjectsDevTask(new UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions()
            {
                KindOfTargetCSharpFramework = kindOfTargetCSharpFramework,
                Version = targetVersion.ToString()
            }));
            deploymentPipeline.Add(new CheckTargetFrameworksInAllCSharpProjectsDevTask());

            deploymentPipeline.Run();
        }

        private static void PrintHelp()
        {
            ConsoleWrapper.WriteText("You shoud input kind of target framework and target version in command line.");
            ConsoleWrapper.WriteText("The first argument should be name of target framework, next target version after space.");
            ConsoleWrapper.WriteText("For example: 'Net 8.0'");
            ConsoleWrapper.WriteText("The kind of target framework is an option of enum KindOfTargetCSharpFramework.");
            ConsoleWrapper.WriteText("The target version should only contain digits and point, for example: '8.0', '3.1.2'");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
