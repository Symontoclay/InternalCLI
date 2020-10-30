using CommonUtils;
using Deployment.Building;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestSandBox
{
    public class BuildHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var solutionDir = "%USERPROFILE%/Documents/GitHub/SymOntoClay";

            _logger.Info($"solutionDir = {solutionDir}");

            solutionDir = EVPath.Normalize(solutionDir);

            _logger.Info($"solutionDir (2) = {solutionDir}");

            var targetNugetDir = Path.Combine(Directory.GetCurrentDirectory(), "TargetNugets");

            _logger.Info($"targetNugetDir = {targetNugetDir}");

            if(Directory.Exists(targetNugetDir))
            {
                Directory.Delete(targetNugetDir, true);
            }

            Directory.CreateDirectory(targetNugetDir);

            var targetLibraryArchDir = Path.Combine(Directory.GetCurrentDirectory(), "LibraryArch");

            _logger.Info($"targetLibraryArchDir = {targetLibraryArchDir}");

            if (Directory.Exists(targetLibraryArchDir))
            {
                Directory.Delete(targetLibraryArchDir, true);
            }

            Directory.CreateDirectory(targetLibraryArchDir);

            var targetLibraryFolderDir = Path.Combine(Directory.GetCurrentDirectory(), "LibraryFolder");

            _logger.Info($"targetLibraryFolderDir = {targetLibraryFolderDir}");

            if (Directory.Exists(targetLibraryFolderDir))
            {
                Directory.Delete(targetLibraryFolderDir, true);
            }

            Directory.CreateDirectory(targetLibraryFolderDir);

            var targetUnity3DLibArchDir = Path.Combine(Directory.GetCurrentDirectory(), "Unity3DLibArch");

            _logger.Info($"targetUnity3DLibArchDir = {targetUnity3DLibArchDir}");

            if (Directory.Exists(targetUnity3DLibArchDir))
            {
                Directory.Delete(targetUnity3DLibArchDir, true);
            }

            Directory.CreateDirectory(targetUnity3DLibArchDir);

            var targetUnity3DLibFolderDir = Path.Combine(Directory.GetCurrentDirectory(), "Unity3DLibFolder");

            _logger.Info($"targetUnity3DLibFolderDir = {targetUnity3DLibFolderDir}");

            if (Directory.Exists(targetUnity3DLibFolderDir))
            {
                Directory.Delete(targetUnity3DLibFolderDir, true);
            }

            Directory.CreateDirectory(targetUnity3DLibFolderDir);

            var targetCLIArchDir = Path.Combine(Directory.GetCurrentDirectory(), "CLIArch");

            _logger.Info($"targetCLIArchDir = {targetCLIArchDir}");

            if (Directory.Exists(targetCLIArchDir))
            {
                Directory.Delete(targetCLIArchDir, true);
            }

            Directory.CreateDirectory(targetCLIArchDir);

            var targetCLIFolderDir = Path.Combine(Directory.GetCurrentDirectory(), "CLIFolder");

            _logger.Info($"targetCLIFolderDir = {targetCLIFolderDir}");

            if (Directory.Exists(targetCLIFolderDir))
            {
                Directory.Delete(targetCLIFolderDir, true);
            }

            Directory.CreateDirectory(targetCLIFolderDir);

            var options = new BuildOptions();

            var solutionOption = new BuildSourceSolutionOptions();
            solutionOption.SolutionDir = solutionDir;

            options.SolutionsOptions.Add(solutionOption);

            var projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCoreHelper";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCore";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayUnityAssetCore";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCLI";
            projectOption.Kind = KindOfSourceProject.CLI;

            //var target = new BuildTargetOptions();
            //target.TargetDir = targetNugetDir;
            //target.Kind = KindOfBuildTarget.NuGet;

            //options.TargetsOptions.Add(target);

            var target = new BuildTargetOptions();
            target.TargetDir = targetLibraryFolderDir;
            target.Kind = KindOfBuildTarget.LibraryFolder;

            options.TargetsOptions.Add(target);

            //_logger.Info($"options = {options}");

            BuildPipelines.Run(options);

            //_logger.Info($" = {}");

            _logger.Info("End");
        }
    }
}
