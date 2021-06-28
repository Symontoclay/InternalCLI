using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.BuildTasks.Pack;
using Deployment.Tasks.BuildTasks.Publish;
using Deployment.Tasks.BuildTasks.Test;
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

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() {
                TargetDir = "a", 
                SkipExistingFilesInTargetDir = false 
            }));

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions() {
                ProjectOrSoutionFileName = @"c:\Users\Acer\Documents\GitHub\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj",
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
                NoLogo = true
            }));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "b",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new PackTask(new PackTaskOptions() {
                ProjectOrSoutionFileName = @"c:\Users\Acer\Documents\GitHub\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj",
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "b"),
                NoLogo = true,
                IncludeSource = true,
                IncludeSymbols = true
            }));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "c",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new PublishTask(new PublishTaskOptions() {
                ProjectOrSoutionFileName = @"c:\Users\Acer\Documents\GitHub\SymOntoClay\SymOntoClayCLI\SymOntoClayCLI.csproj",
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "c"),
                NoLogo = true
            }));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "d",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new TestTask(new TestTaskOptions()
            {
                ProjectOrSoutionFileName = @"c:\Users\Acer\Documents\GitHub\SymOntoClay\SymOntoClay.sln"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case1()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() {
                TargetDir = "a",
                SkipExistingFilesInTargetDir = false 
            }));

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
