using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects;
using Deployment.DevTasks.InstalledNuGetPackages.UpdateInstalledNuGetPackagesInAllCSharpProjects;
using NLog;

namespace UpdateInstalledNuGetPackagesInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args.Length == 0)
            {
                Console.WriteLine("You shoud input target nuget packageId and target version in command line.");
                Console.WriteLine("The first argument should be target nuget packageId, next target version after space.");
                Console.WriteLine("For example: 'NLog 5.1.4'");
                Console.WriteLine("The target version should only contain digits and point, for example: '5.1.4', '3.1.2'");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments! So target version argument is absent.");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            if (args.Length > 2)
            {
                Console.WriteLine("Too many arguments!");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            var targetPackageId = args[0];
            var targetVersionStr = args[1];

#if DEBUG
            //_logger.Info($"targetPackageId = {targetPackageId}");
            //_logger.Info($"targetVersionStr = {targetVersionStr}");
#endif

            if (!Version.TryParse(targetVersionStr, out var targetVersion))
            {
                Console.WriteLine($"Bad version format '{targetVersionStr}'");
                Console.WriteLine("Press any key for exit");

                Console.ReadKey();

                return;
            }

            Console.WriteLine($"All C# projects with nuget package {targetPackageId} will be updated to {targetVersionStr}.");
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

            deploymentPipeline.Add(new UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTask(new UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions()
            {
                PackageId = targetPackageId,
                Version = targetVersionStr
            }));

            deploymentPipeline.Add(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask());

            deploymentPipeline.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
