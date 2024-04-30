using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CoreToInternalCLIDist;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.DevTasks.DevSiteFullBuildAndCommit;
using Deployment.Helpers;
using Deployment.ReleaseTasks.DeploymentToProd;
using Deployment.ReleaseTasks.MarkAsCompleted;
using Deployment.ReleaseTasks.MergeReleaseBranchToMaster;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.SetUpRepository;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.ReleaseTasks.MakeRelease
{
    /// <summary>
    /// It is the task that makes relese and closes version development!
    /// Be careful during using the task.
    /// </summary>
    public class MakeReleaseReleaseTask : BaseDeploymentTask
    {
        public MakeReleaseReleaseTask()
            : this(null)
        {
        }
        
        public MakeReleaseReleaseTask(IDeploymentTask parentTask)
            : base("E791FF7A-6A23-4B01-9069-21A3A1870112", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            if(!FutureReleaseGuard.MayIMakeRelease())
            {
                _logger.Info("Making release is forbiden! New version has not been started!");

                return;
            }

            if(!CheckGitHubToken())
            {
                return;
            }

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            var versionBranchName = futureReleaseInfo.Version;

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new DeploymentTasksGroup("F25F27AF-AAFC-4357-AE06-3F55C933B633", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DeploymentTasksGroup("BC98393A-ECF0-479C-A8A4-D36873CF5DDB", true, this)
                    {
                        SubItems = targetSolutions.Select(repository => new SetUpRepositoryTask(new SetUpRepositoryTaskOptions()
                        {
                            RepositoryPath = repository.Path
                        }, this))
                    },
                    new DeploymentTasksGroup("A9989DE4-927B-4D1A-A87B-FE3B43A7DB9D", false, this)
                    {
                        SubItems = targetSolutions.Select(repository => new CheckoutTask(new CheckoutTaskOptions()
                        {
                            RepositoryPath = repository.Path,
                            BranchName = versionBranchName
                        }, this))
                    }
                }
            });

            //Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            //{
            //    Message = "snapshot",
            //    RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            //}, this));

            Exec(new DeploymentTasksGroup("F57D8666-91D6-4F59-BE35-8DC959D33A1F", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CoreToInternalCLIDistDevTask(this)
                }
            });

            Exec(new DeploymentTasksGroup("543A0B7C-95E8-41C1-94E7-665B694BE95F", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DevSiteFullBuildAndCommitTask(this),
                    new DevFullMaintainingDevTask(this)
                }
            });

            Exec(new MergeReleaseBranchToMasterReleaseTask(this));

            Exec(new DeploymentToProdReleaseTask(this));

            Exec(new MarkAsCompletedReleaseTask(this));
        }

        private bool CheckGitHubToken()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var token = settings.GetSecret("GitHub");

            if(string.IsNullOrEmpty(token.Value))
            {
                _logger.Info("Making release is forbiden! GitHub token is empty!");

                return false;
            }

            if(!token.ExpDate.HasValue)
            {
                _logger.Info("Making release is forbiden! ExpDate of GitHub token is empty!");

                return false;
            }

            if(token.ExpDate <= DateTime.Now)
            {
                _logger.Info("Making release is forbiden! GitHub token is expired!");

                return false;
            }

            return true;
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
