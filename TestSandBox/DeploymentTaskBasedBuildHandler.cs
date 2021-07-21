﻿using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.Tasks;
using Deployment.Tasks.ArchTasks.Zip;
using Deployment.Tasks.BuildTasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.BuildTasks.NuGetPack;
using Deployment.Tasks.BuildTasks.Publish;
using Deployment.Tasks.BuildTasks.Test;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.SiteTasks.SiteBuild;
using Deployment.Tasks.SiteTasks.UpdateReleaseNotes;
using Deployment.Tasks.UnityTasks.ExportPackage;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeaders;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeadersInFolder;
using NLog;
using SiteBuilder;
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

            Case9();
            //Case8();
            //Case7();
            //Case6();
            //Case5();
            //Case4();
            //Case3();
            //Case2();
            //Case1();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void Case9()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var license = ProjectsDataSource.GetLicense("MIT");

            var coreProject = ProjectsDataSource.GetProject(KindOfProject.CoreLib);

            _logger.Info($"coreProject.Path = {coreProject.Path}");

            deploymentPipeline.Add(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions()
            {
                Text = license.HeaderContent,
                SourceDir = coreProject.Path
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case8()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var license = ProjectsDataSource.GetLicense("MIT");

            var unitySolution = ProjectsDataSource.GetSolution(KindOfProject.Unity);

            deploymentPipeline.Add(new UpdateCopyrightInFileHeadersInFolderTask(new UpdateCopyrightInFileHeadersInFolderTaskOptions() {
                Text = license.HeaderContent,
                SourceDir = unitySolution.SourcePath
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case7()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var targetFilesList = new List<string>() 
            {
                @"%USERPROFILE%\source\repos\SymOntoClayAsset\Assets\SymOntoClay\World.cs",
                @"%USERPROFILE%\source\repos\SymOntoClayAsset\Assets\SymOntoClay\Thing.cs"
            };

            targetFilesList = targetFilesList.Select(p => PathsHelper.Normalize(p)).ToList();

            var license = ProjectsDataSource.GetLicense("MIT");

            deploymentPipeline.Add(new UpdateCopyrightInFileHeadersTask(new UpdateCopyrightInFileHeadersTaskOptions() { 
                Text = license.HeaderContent,
                TargetFiles = targetFilesList
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case6()
        {
            using (var tempDir = new TempDirectory())
            {
                _logger.Info($"tempDir.FullName = {tempDir.FullName}");

                var deploymentPipeline = new DeploymentPipeline();

                var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

                _logger.Info($"siteSolution = {siteSolution}");

                var destDir = Path.Combine(Directory.GetCurrentDirectory(), "s");

                deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
                {
                    TargetDir = destDir,
                    SkipExistingFilesInTargetDir = false
                }));

                deploymentPipeline.Add(new SiteBuildTask(new SiteBuildTaskOptions()
                {
                    KindOfTargetUrl = KindOfTargetUrl.Path,
                    SiteName = siteSolution.RepositoryName,
                    SourcePath = siteSolution.SourcePath,
                    DestPath = destDir,
                    TempPath = tempDir.FullName
                }));

                _logger.Info($"deploymentPipeline = {deploymentPipeline}");

                deploymentPipeline.Run();
            }
        }

        private void Case5()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateReleaseNotesTask(new UpdateReleaseNotesTaskOptions() { 
                FutureReleaseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "future_release.json"),
                ArtifactsForDeployment = new List<KindOfArtifact>() 
                {
                    KindOfArtifact.SourceArch,
                    KindOfArtifact.CLIArch,
                    KindOfArtifact.UnityPackage
                },
                ReleaseNotesFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ReleaseNotes.json"),
                BaseHref = "https://github.com/Symontoclay/SymOntoClay/"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case4()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "c",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new PublishTask(new PublishTaskOptions()
            {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCLI\SymOntoClayCLI.csproj"),
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "c"),
                NoLogo = true
            }));

            deploymentPipeline.Add(new ZipTask(new ZipTaskOptions() { 
                SourceDir = Path.Combine(Directory.GetCurrentDirectory(), "c"),
                OutputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CLI-0.3.6.zip")
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case3()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "d",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new ExportPackageTask(new ExportPackageTaskOptions() { 
                UnityExeFilePath = @"c:\Program Files\Unity\Hub\Editor\2020.2.3f1\Editor\Unity.exe",
                RootDir = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClayAsset"),
                SourceDir = @"Assets\SymOntoClay",
                OutputPackageName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\tmpSymOntoClay_2.unitypackage")
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case2()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() {
                TargetDir = "a", 
                SkipExistingFilesInTargetDir = false 
            }));

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions() {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj"),
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
                NoLogo = true
            }));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "b",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new NuGetPackTask(new NuGetPackTaskOptions() {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj"),
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
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCLI\SymOntoClayCLI.csproj"),
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "c"),
                NoLogo = true
            }));

            deploymentPipeline.Add(new TestTask(new TestTaskOptions()
            {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClay.sln")
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
                SourceDir = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\InternalCLI\CSharpUtils\"),
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
