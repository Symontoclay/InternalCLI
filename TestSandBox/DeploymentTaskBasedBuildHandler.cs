using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class DeploymentTaskBasedBuildHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            Case2();
            //Case1();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void Case2()
        {
            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() { TargetDir = "a", SkipExistingFilesInTargetDir = false }));
            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions() {
                ProjectFileName = @"c:\Users\Acer\Documents\GitHub\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj",
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
                NoLogo = true
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case1()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() { TargetDir = "a", SkipExistingFilesInTargetDir = false }));
            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = @"c:\Users\Acer\source\repos\InternalCLI\CSharpUtils\",
                DestDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
                SaveSubDirs = false,
                OnlySubDirs = new List<string>() { "bin" },
                ExceptSubDirs = new List<string>() { "Debug/net5.0" },
                OnlyFileExts = new List<string>() { "dll", "pdb", "json", "cs" },
                ExceptFileExts = new List<string>() { "json" },
                FileNameShouldContain = new List<string>() { "CS" },
                FileNameShouldNotContain = new List<string>() { "utils" }
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }
    }
}
