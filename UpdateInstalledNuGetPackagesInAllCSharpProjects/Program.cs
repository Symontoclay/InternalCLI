using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects;
using Deployment.DevTasks.InstalledNuGetPackages.UpdateInstalledNuGetPackagesInAllCSharpProjects;
using Microsoft.Extensions.Configuration;
using NLog;
using SymOntoClay.CLI.Helpers;
using UpdateTargetFrameworkInAllCSharpProjects;

namespace UpdateInstalledNuGetPackagesInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var consoleWrapperSettings = configuration.GetSection("ConsoleWrapperSettings").Get<ConsoleWrapperSettings>() ?? new ConsoleWrapperSettings();

#if DEBUG
            //_logger.Info($"consoleWrapperSettings = {JsonConvert.SerializeObject(consoleWrapperSettings, Formatting.Indented)}");
#endif

#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            if(consoleWrapperSettings.UseNLogLogger)
            {
                ConsoleWrapper.SetNLogLogger(_logger);
            }

            if (consoleWrapperSettings.WriteOutputToTextFileAsParallel)
            {
                ConsoleWrapper.WriteOutputToTextFileAsParallel = true;
            }

            if (consoleWrapperSettings.WriteCopyright)
            {
                ConsoleWrapper.WriteCopyright();
            }

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

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

            var targetPackageId = (string)result.Params["TargetPackageId"];
            var targetVersion = (Version)result.Params["TargetVersion"];

#if DEBUG
            //_logger.Info($"targetPackageId = {targetPackageId}");
            //_logger.Info($"targetVersion = {targetVersion}");
#endif

            ConsoleWrapper.WriteText($"All C# projects with nuget package {targetPackageId} will be updated to {targetVersion}.");
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
                Version = targetVersion.ToString()
            }));

            deploymentPipeline.Add(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask());

            deploymentPipeline.Run();
        }

        private static void PrintHelp()
        {
            var reamdeFilePath = Path.Combine(AppContext.BaseDirectory, "README.md");

            var filelines = File.ReadAllLines(reamdeFilePath);

            foreach (var line in filelines)
            {
                ConsoleWrapper.WriteText(line);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
