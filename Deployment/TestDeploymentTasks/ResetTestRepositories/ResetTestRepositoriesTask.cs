using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.Commit;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.DeleteBranch;
using NLog.Fluent;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.ResetTestRepositories
{
    public class ResetTestRepositoriesTask : OldBaseDeploymentTask
    {
        public ResetTestRepositoriesTask()
            : this(0u)
        {
        }

        public ResetTestRepositoriesTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var testSettings = TestProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();

            var targetSolutions = testSettings.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList(),
                Message = "Commit uncommited changes"
            }, this));

            foreach (var targetSolution in targetSolutions)
            {
#if DEBUG
                //_logger.Info($"targetSolution.Path = {targetSolution.Path}");
#endif

                var currentBranch = GitRepositoryHelper.GetCurrentBranchName(targetSolution.Path);

#if DEBUG
                //_logger.Info($"currentBranch = {currentBranch}");
#endif

                if (currentBranch == "master")
                {
                    continue;
                }

                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = targetSolution.Path,
                    BranchName = "master"
                }, this));

                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = targetSolution.Path,
                    BranchName = currentBranch,
                    IsOrigin = false
                }, this));

                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = targetSolution.Path,
                    BranchName = currentBranch,
                    IsOrigin = true
                }, this));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Reset test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
