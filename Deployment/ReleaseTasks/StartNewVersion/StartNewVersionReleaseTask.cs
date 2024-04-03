using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.Helpers;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.PushNewBranchToOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.ReleaseTasks.StartNewVersion
{
    public class StartNewVersionReleaseTask : BaseDeploymentTask
    {
        public StartNewVersionReleaseTask(StartNewVersionReleaseTaskOptions options)
            : this(options, null)
        {
        }

        public StartNewVersionReleaseTask(StartNewVersionReleaseTaskOptions options, IDeploymentTask parentTask)
            : base("B33EDD1E-1CCD-4635-A1F3-4950DE9932FE", true, options, parentTask)
        {
            _options = options;
        }

        private readonly StartNewVersionReleaseTaskOptions _options;

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

            Exec(new DeploymentTasksGroup("4F0ABBB3-1B57-4EDD-8028-11954C301265", true, this)
            {
                SubItems = targetSolutions.Select(repository => new DeploymentTasksGroup(MD5Helper.GetHash(repository.Path), true, this)
                {
                    SubItems = new List<IDeploymentTask>()
                    {
                        new DeploymentTasksGroup("6E7E69C1-581B-4E35-8E95-4769955955F6", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CheckoutTask(new CheckoutTaskOptions()
                                {
                                    RepositoryPath = repository.Path,
                                    BranchName = _masterBranchName
                                }, this)
                            }
                        },
                        new DeploymentTasksGroup("1C337152-8739-45FF-A0AA-65A4F2F2229A", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                                {
                                    Message = "snapshot",
                                    RepositoryPaths = new List<string>() { repository.Path }
                                }, this)
                            }
                        },
                        new CreateBranchTask(new CreateBranchTaskOptions()
                        {
                            RepositoryPath = repository.Path,
                            BranchName = versionBranchName
                        }, this),
                        new PushNewBranchToOriginTask(new PushNewBranchToOriginTaskOptions()
                        {
                            RepositoryPath = repository.Path,
                            BranchName = versionBranchName
                        }, this),
                        new DeploymentTasksGroup("FA302596-4624-4F66-9CDF-CC7AFC68BA34", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CheckoutTask(new CheckoutTaskOptions()
                                {
                                    RepositoryPath = repository.Path,
                                    BranchName = versionBranchName
                                }, this)
                            }
                        }
                    }
                })
            });

            var futureReleaseInfo = FutureReleaseInfoReader.ReadSource();

            futureReleaseInfo.Status = FutureReleaseStatus.Started.ToString();
            futureReleaseInfo.StartDate = DateTime.Now;
            futureReleaseInfo.FinishDate = null;
            futureReleaseInfo.Version = _options.Version;

            FutureReleaseInfoWriter.WriteSource(futureReleaseInfo);

            Exec(new DevFullMaintainingDevTask(this));
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
