using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects;
using Deployment.DevTasks.TargetFrameworks.UpdateTargetFrameworkInAllCSharpProjects;
using Newtonsoft.Json;
using NLog;
using SymOntoClay.CLI.Helpers;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            ConsoleWrapper.WriteCopyright();

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
            Console.WriteLine("You shoud input kind of target framework and target version in command line.");
            Console.WriteLine("The first argument should be name of target framework, next target version after space.");
            Console.WriteLine("For example: 'Net 8.0'");
            Console.WriteLine("The kind of target framework is an option of enum KindOfTargetCSharpFramework.");
            Console.WriteLine("The target version should only contain digits and point, for example: '8.0', '3.1.2'");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
