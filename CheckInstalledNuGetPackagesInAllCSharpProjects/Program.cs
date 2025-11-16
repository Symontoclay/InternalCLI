using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects;
using NLog;
using SymOntoClay.CLI.Helpers;

namespace CheckInstalledNuGetPackagesInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var parser = new CheckInstalledNuGetPackagesInAllCSharpProjectsCommandLineParser(true);

            var parsingResult = parser.Parse(args);

            if (parsingResult.Errors.Count > 0)
            {
                foreach (var error in parsingResult.Errors)
                {
                    ConsoleWrapper.WriteError(error);
                }

                return;
            }

            var showOnlyOutdatedPackages = parsingResult.Params.TryGetValue("-ShowOnlyOutdated", out var value) && (bool)value;

            DeploymentPipeline.Run(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions
            {
                ShowOnlyOutdatedPackages = showOnlyOutdatedPackages
            }));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
