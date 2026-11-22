using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects;
using Deployment.DevTasks.TargetFrameworks.UpdateTargetFrameworkInAllCSharpProjects;
using Microsoft.Extensions.Configuration;
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

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var consoleWrapperSettings = configuration.GetSection("ConsoleWrapperSettings").Get<ConsoleWrapperSettings>() ?? new ConsoleWrapperSettings();

#if DEBUG
            //_logger.Info($"consoleWrapperSettings = {JsonConvert.SerializeObject(consoleWrapperSettings, Formatting.Indented)}");
#endif

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();

#if DEBUG
            //_logger.Info($"appSettings = {JsonConvert.SerializeObject(appSettings, Formatting.Indented)}");
#endif

#if DEBUG
            //_logger.Info($"args = {JsonConvert.SerializeObject(args, Formatting.Indented)}");
#endif

            if (consoleWrapperSettings.UseNLogLogger)
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
                ConsoleWrapper.WriteText(string.Empty);
            }

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

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

            var kindOfTargetCSharpFramework = (KindOfTargetCSharpFramework)result.Params["TargetFramework"];
            var targetVersion = (Version)result.Params["TargetVersion"];

            if (appSettings.ReadOnlyMode)
            {
                ConsoleWrapper.WriteText("It is just read only mode.");
                ConsoleWrapper.WriteText($"Checking possible chаnges version for all C# projects with target framework {kindOfTargetCSharpFramework} to {targetVersion}.");
                ConsoleWrapper.WriteText("Nothing will be updated.");
                ConsoleWrapper.WriteText(string.Empty);
            }
            else
            {
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
                ConsoleWrapper.WriteText(string.Empty);
            }

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateTargetFrameworkInAllCSharpProjectsDevTask(new UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions()
            {
                KindOfTargetCSharpFramework = kindOfTargetCSharpFramework,
                Version = targetVersion.ToString(),
                ReadOnlyMode = appSettings.ReadOnlyMode
            }));
            deploymentPipeline.Add(new CheckTargetFrameworksInAllCSharpProjectsDevTask());

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
