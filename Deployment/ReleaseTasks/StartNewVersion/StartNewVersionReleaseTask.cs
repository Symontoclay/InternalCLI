using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.PushNewBranchToOrigin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.StartNewVersion
{
    public class StartNewVersionReleaseTask : BaseDeploymentTask
    {
        public StartNewVersionReleaseTask(StartNewVersionReleaseTaskOptions options)
            : this(options, 0u)
        {
        }

        public StartNewVersionReleaseTask(StartNewVersionReleaseTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly StartNewVersionReleaseTaskOptions _options;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
        }

        private readonly string _masterBranchName = "master";

        /// <inheritdoc/>
        protected override void OnRun()
        {
            if (FutureReleaseGuard.MayIMakeRelease())
            {
                _logger.Info("Starting new version is forbiden! New version has already been started!");

                return;
            }

            var versionBranchName = _options.Version;

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            var isVersionBranchExisting = false;

            foreach (var repository in targetSolutions)
            {
                if(GitRepositoryHelper.IsBranchExists(repository.Path, versionBranchName))
                {
                    _logger.Info($"The branch '{versionBranchName}' is already existing at '{repository.Path}'!");

                    isVersionBranchExisting = true;
                }
            }

            if(isVersionBranchExisting)
            {
                _logger.Info("Starting new version has been canceled!");

                return;
            }

            foreach (var repository in targetSolutions)
            {
                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = repository.Path,
                    BranchName = _masterBranchName
                }, NextDeep));

                Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                {
                    Message = "snapshot",
                    RepositoryPaths = new List<string>() { repository.Path }
                }, NextDeep));

                Exec(new CreateBranchTask(new CreateBranchTaskOptions()
                {
                    RepositoryPath = repository.Path,
                    BranchName = versionBranchName
                }, NextDeep));

                Exec(new PushNewBranchToOriginTask(new PushNewBranchToOriginTaskOptions()
                {
                    RepositoryPath = repository.Path,
                    BranchName = versionBranchName
                }));

                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = repository.Path,
                    BranchName = versionBranchName
                }, NextDeep));
            }

            var futureReleaseInfo = FutureReleaseInfoReader.ReadSource();

            futureReleaseInfo.Status = FutureReleaseStatus.Started.ToString();
            futureReleaseInfo.StartDate = DateTime.Now;
            futureReleaseInfo.FinishDate = null;
            futureReleaseInfo.Version = _options.Version;

            FutureReleaseInfoWriter.WriteSource(futureReleaseInfo);

            Exec(new DevFullMaintainingDevTask(NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Starts new version '{_options.Version}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
