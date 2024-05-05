using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects;
using Deployment.DevTasks.TargetFrameworks.UpdateTargetFrameworkInAllCSharpProjects;
using Newtonsoft.Json;
using NLog;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if(args.Length == 0)
            {
                Console.WriteLine("You shoud input kind of target framework and target version in command line.");
                Console.WriteLine("The first argument should be name of target framework, next target version after space.");
                Console.WriteLine("For example: 'Net 8.0'");
                Console.WriteLine("The kind of target framework is an option of enum KindOfTargetCSharpFramework.");
                Console.WriteLine("The target version should only contain digits and point, for example: '8.0', '3.1.2'");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            if(args.Length < 2)
            {
                Console.WriteLine("Not enough arguments! So target version argument is absent.");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            if(args.Length > 2)
            {
                Console.WriteLine("Too many arguments!");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            var kindOfTargetCSharpFrameworkStr = args[0];
            var targetVersionStr = args[1];

#if DEBUG
            //_logger.Info($"kindOfTargetCSharpFrameworkStr = {kindOfTargetCSharpFrameworkStr}");
            //_logger.Info($"targetVersionStr = {targetVersionStr}");
#endif

            if(!Enum.TryParse<KindOfTargetCSharpFramework>(kindOfTargetCSharpFrameworkStr, out var kindOfTargetCSharpFramework))
            {
                Console.WriteLine($"Unknown target framework '{kindOfTargetCSharpFrameworkStr}'");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

#if DEBUG
            //_logger.Info($"kindOfTargetCSharpFramework = {kindOfTargetCSharpFramework}");
#endif

            if(!Version.TryParse(targetVersionStr, out var targetVersion))
            {
                Console.WriteLine($"Bad version format '{targetVersionStr}'");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            Console.WriteLine($"Version of all C# projects with target framework {kindOfTargetCSharpFramework} will be changed to {targetVersionStr}.");
            Console.WriteLine("Are you sure?");
            Console.WriteLine("Press 'y' or 'Y' for continue or other else key for cancel.");
            Console.WriteLine("After your choise press enter.");

            var key = Console.ReadLine();

            if (key != "y" && key != "Y")
            {
                Console.WriteLine("Updating has been cancelled.");

                return;
            }

            Console.WriteLine("Updating has been started.");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateTargetFrameworkInAllCSharpProjectsDevTask(new UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions()
            {
                KindOfTargetCSharpFramework = kindOfTargetCSharpFramework,
                Version = targetVersionStr
            }));
            deploymentPipeline.Add(new CheckTargetFrameworksInAllCSharpProjectsDevTask());

            deploymentPipeline.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
