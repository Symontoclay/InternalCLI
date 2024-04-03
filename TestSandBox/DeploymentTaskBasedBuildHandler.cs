using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.CreateAndCommitReadmes;
using Deployment.DevTasks.CreateReadmes;
using Deployment.DevTasks.DevSiteBuild;
using Deployment.DevTasks.DevSiteFullBuild;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.ArchTasks.Zip;
using Deployment.Tasks.BuildReadme;
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
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInFolder;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionCopyright;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionVersion;
using Deployment.Tasks.ProjectsTasks.UpdateUnityPackageVersion;
using NLog;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils.DeploymentTasks;

namespace TestSandBox
{
    public class DeploymentTaskBasedBuildHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //SCase17();
            //Case16();
            Case15();
            //Case14();
            //Case13();
            //Case12();
            //Case11();
            //Case10();
            //Case9();
            //Case8();
            //Case7();
            //Case6();//SiteBuildTask
            //Case5();
            //Case4();
            //Case3();
            //Case2();
            //Case1();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void Case17()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateAndCommitReadmesDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case16()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateReadmesDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case15()
        {
            var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            //_logger.Info($"siteSolution = {siteSolution}");

            var commonBadgesFileName = Path.Combine(Directory.GetCurrentDirectory(), "__common_BADGES.md");

            _logger.Info($"commonBadgesFileName = {commonBadgesFileName}");

            var commonReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "__common_README.md");

            _logger.Info($"commonReadmeFileName = {commonReadmeFileName}");

            var repositorySpecificBadgesFileName = string.Empty;

            _logger.Info($"repositorySpecificBadgesFileName = {repositorySpecificBadgesFileName}");

            var repositorySpecificReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "__ReadmeSource.md");

            _logger.Info($"repositorySpecificReadmeFileName = {repositorySpecificReadmeFileName}");

            var targetReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "TargetReadme.md");

            _logger.Info($"targetReadmeFileName = {targetReadmeFileName}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildReadmeTask(new BuildReadmeTaskOptions() {
                SiteSourcePath = siteSolution.SourcePath,
                SiteDestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
                CommonBadgesFileName = commonBadgesFileName,
                CommonReadmeFileName = commonReadmeFileName,
                RepositorySpecificBadgesFileName = repositorySpecificBadgesFileName,
                RepositorySpecificReadmeFileName = repositorySpecificReadmeFileName,
                TargetReadmeFileName = targetReadmeFileName
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case14()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var version = "0.3.2";

            deploymentPipeline.Add(new UpdateUnityPackageVersionTask(new UpdateUnityPackageVersionTaskOptions() 
            { 
                PackageSourcePath = Directory.GetCurrentDirectory(),
                Version = version
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case13()
        {
            DeploymentPipeline.Run(new DevSiteFullBuildTask());
        }

        private void Case12()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new DevSiteBuildTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case11()
        {
            //var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            //_logger.Info($"siteSolution = {siteSolution}");

            //var coreAssetLibProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreAssetLib);

            //_logger.Info($"coreAssetLibProject = {coreAssetLibProject}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CoreToSiteSourceDevTask(/*new CoreToSiteSourceDevTaskOptions() { 
                CoreCProjPath = coreAssetLibProject.CsProjPath,
                SiteSourceDir = siteSolution.SourcePath
            }*/));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case10()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var coreSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.CoreSolution);

            _logger.Info($"coreSolution.SlnPath = {coreSolution.SlnPath}");

            deploymentPipeline.Add(new UpdateSolutionVersionTask(new UpdateSolutionVersionTaskOptions() { 
                SolutionFilePath = coreSolution.SlnPath,
                Version = "0.3.2"
            }));

            deploymentPipeline.Add(new UpdateSolutionCopyrightTask(new UpdateSolutionCopyrightTaskOptions() {
                SolutionFilePath = coreSolution.SlnPath,
                Copyright = "tst"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case9()
        {
            var deploymentPipeline = new DeploymentPipeline();

            var license = ProjectsDataSourceFactory.GetLicense("MIT");

            var coreProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreLib);

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

            var license = ProjectsDataSourceFactory.GetLicense("MIT");

            var unitySolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.Unity);

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

            var license = ProjectsDataSourceFactory.GetLicense("MIT");

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

                var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

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
            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateReleaseNotesTask(new UpdateReleaseNotesTaskOptions() { 
                FutureReleaseInfo = futureReleaseInfo,
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
