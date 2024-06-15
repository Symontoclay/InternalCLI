using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects;
using Deployment.DevTasks.InstalledNuGetPackages.UpdateInstalledNuGetPackagesInAllCSharpProjects;
using NLog;
using SymOntoClay.CLI.Helpers;

namespace UpdateInstalledNuGetPackagesInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //
#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            ConsoleWrapper.WriteCopyright();

            if (args.Length == 0)
            {
                PrintHelp();

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

            ConsoleWrapper.WriteText($"All C# projects with nuget package {targetPackageId} will be updated to {targetVersionStr}.");
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

            deploymentPipeline.Add(new UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTask(new UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions()
            {
                PackageId = targetPackageId,
                Version = targetVersionStr
            }));

            deploymentPipeline.Add(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask());

            deploymentPipeline.Run();
        }

        private static void PrintHelp()
        {
            ConsoleWrapper.WriteText("You shoud input target nuget packageId and target version in command line.");
            ConsoleWrapper.WriteText("The first argument should be target nuget packageId, next target version after space.");
            ConsoleWrapper.WriteText("For example: 'NLog 5.1.4'");
            ConsoleWrapper.WriteText("The target version should only contain digits and point, for example: '5.1.4', '3.1.2'");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
