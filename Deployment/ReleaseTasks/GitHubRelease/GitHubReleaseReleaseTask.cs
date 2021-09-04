using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.ArchTasks.Zip;
using Deployment.Tasks.BuildTasks.Publish;
using Deployment.Tasks.UnityTasks.ExportPackage;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.GitHubRelease
{
    public class GitHubReleaseReleaseTask : BaseDeploymentTask
    {//GitHubReleaseTask
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public GitHubReleaseReleaseTask()
            : this(new GitHubReleaseReleaseTaskOptions() { 
                Repositories = ProjectsDataSource.GetSolutionsWithMaintainedReleases().Select(p => p.Path).ToList()
            })
        {
        }

        public GitHubReleaseReleaseTask(GitHubReleaseReleaseTaskOptions options)
        {
            _options = options;
        }

        private readonly GitHubReleaseReleaseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);

            ValidateList(nameof(_options.Repositories), _options.Repositories);
            _options.Repositories.ForEach(item => ValidateFileName(nameof(item), item));
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif
            using var unityTempDir = new TempDirectory();
            using var cliTempDir = new TempDirectory();
            using var cliArchTempDir = new TempDirectory();

            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var unitySolution = settings.GetSolution(KindOfProject.Unity);

            var targetUnityVersion = UnityHelper.GetTargetUnityVersion(unitySolution.Path);

            var unityExePath = settings.UtityExeInstances.SingleOrDefault(p => p.Version == targetUnityVersion).Path;

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

#if DEBUG
            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");
#endif

            var version = futureReleaseInfo.Version;

            var sourceDir = unitySolution.SourcePath.Replace(unitySolution.Path, string.Empty).Trim();

            if (sourceDir.StartsWith("/"))
            {
                sourceDir = sourceDir.Substring(1);
            }

            var unityPackageName = DeployedItemsFactory.GetUnityAssetName(version);

#if DEBUG
            _logger.Info($"unityPackageName = {unityPackageName}");
#endif

            var unityPackageFullPath = Path.Combine(unityTempDir.FullName, unityPackageName);

#if DEBUG
            _logger.Info($"unityPackageFullPath = {unityPackageFullPath}");
#endif

            Exec(new ExportPackageTask(new ExportPackageTaskOptions()
            {
                UnityExeFilePath = unityExePath,
                RootDir = unitySolution.Path,
                SourceDir = sourceDir,
                OutputPackageName = unityPackageFullPath
            }));

            var cliProject = ProjectsDataSource.GetProject(KindOfProject.CLI);

            Exec(new PublishTask(new PublishTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliTempDir.FullName,
                NoLogo = true
            }));

            var cliArchName = DeployedItemsFactory.GetCLIArchName(version);

#if DEBUG
            _logger.Info($"cliArchName = {cliArchName}");
#endif

            var cliArchFullPath = Path.Combine(cliArchTempDir.FullName, cliArchName);

#if DEBUG
            _logger.Info($"cliArchFullPath = {cliArchFullPath}");
#endif

            Exec(new ZipTask(new ZipTaskOptions()
            {
                SourceDir = cliTempDir.FullName,
                OutputFilePath = cliArchFullPath
            }));



            throw new NotImplementedException();
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
