using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.CopyAndPublishVSProjectOrSolution;
using Deployment.Helpers;
using Deployment.Tasks.ArchTasks.Zip;
using Deployment.Tasks.GitHubTasks.GitHubRelease;
using Deployment.Tasks.UnityTasks.ExportPackage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.ReleaseTasks.GitHubRelease
{
    public class GitHubReleaseReleaseTask : BaseDeploymentTask
    {
        private static GitHubReleaseReleaseTaskOptions CreateOptions()
        {
            var options = new GitHubReleaseReleaseTaskOptions();

            var repositories = new List<GitHubRepositoryInfo>();
            options.Repositories = repositories;

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            foreach(var targetSolution in targetSolutions)
            {
                var item = new GitHubRepositoryInfo();

                repositories.Add(item);

                item.RepositoryName = targetSolution.RepositoryName;
                item.RepositoryOwner = targetSolution.OwnerName;
            }

            return options;
        }

        public GitHubReleaseReleaseTask(GitHubReleaseReleaseTaskOptions options)
            : this(options, null)
        {
        }

        public GitHubReleaseReleaseTask()
            : this(parentTask: null)
        {
        }

        public GitHubReleaseReleaseTask(IDeploymentTask parentTask)
            : this(CreateOptions(), parentTask)
        {
        }

        public GitHubReleaseReleaseTask(GitHubReleaseReleaseTaskOptions options, IDeploymentTask parentTask)
            : base("5157C4AC-A3CF-4D2B-A443-DBD76A67BDC3", true, options, parentTask)
        {
            _options = options;
        }

        private readonly GitHubReleaseReleaseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);

            ValidateList(nameof(_options.Repositories), _options.Repositories);
            _options.Repositories.ForEach(item => 
            { 
                ValidateStringValueAsNonNullOrEmpty(nameof(item.RepositoryName), item.RepositoryName);
                ValidateStringValueAsNonNullOrEmpty(nameof(item.RepositoryOwner), item.RepositoryOwner);
            });
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            using var unityTempDir = new TempDirectory();
            using var cliTempDir = new TempDirectory();
            using var cliArchTempDir = new TempDirectory();

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var unitySolution = settings.GetSolution(KindOfProject.Unity);

            var targetUnityVersion = UnityHelper.GetTargetUnityVersion(unitySolution.Path);

            var unityExePath = settings.UtityExeInstances.SingleOrDefault(p => p.Version == targetUnityVersion).Path;

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            var version = futureReleaseInfo.Version;

            var notesText = futureReleaseInfo.Description;

            var sourceDir = unitySolution.SourcePath.Replace(unitySolution.Path, string.Empty).Trim();

            if (sourceDir.StartsWith("/"))
            {
                sourceDir = sourceDir.Substring(1);
            }

            var unityPackageName = DeployedItemsFactory.GetUnityAssetName(version);

            //var unityPackageFullPath = Path.Combine(unityTempDir.FullName, unityPackageName);

            //Exec(new ExportPackageTask(new ExportPackageTaskOptions()
            //{
            //    UnityExeFilePath = unityExePath,
            //    RootDir = unitySolution.Path,
            //    SourceDir = sourceDir,
            //    OutputPackageName = unityPackageFullPath
            //}, this));

            var cliProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CLI);

            Exec(new CopyAndPublishVSProjectOrSolutionDevTask(new CopyAndPublishVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliTempDir.FullName,
                NoLogo = true
            }, this));

            var cliArchName = DeployedItemsFactory.GetCLIArchName(version);

            var cliArchFullPath = Path.Combine(cliArchTempDir.FullName, cliArchName);

            Exec(new ZipTask(new ZipTaskOptions()
            {
                SourceDir = cliTempDir.FullName,
                OutputFilePath = cliArchFullPath
            }, this));

            var token = settings.GetSecret("GitHub");

            Exec(new DeploymentTasksGroup("73C35B3E-6FBC-4999-A17A-A9880AD7E31F", true, this)
            {
                SubItems = _options.Repositories.Select(repository => new GitHubReleaseTask(new GitHubReleaseTaskOptions()
                {
                    Token = token.Value,
                    RepositoryOwner = repository.RepositoryOwner,
                    RepositoryName = repository.RepositoryName,
                    Version = version,
                    NotesText = notesText,
                    //Prerelease = true,
                    //Draft = true,
                    Assets = new List<GitHubReleaseAssetOptions>()
                    {
                        //new GitHubReleaseAssetOptions()
                        //{
                        //    DisplayedName = unityPackageName,
                        //    UploadedFilePath = unityPackageFullPath
                        //},
                        new GitHubReleaseAssetOptions()
                        {
                            DisplayedName = cliArchName,
                            UploadedFilePath = cliArchFullPath
                        }
                    }
                }, this))
            });
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds and commits READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
