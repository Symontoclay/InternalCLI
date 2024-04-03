using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.DeleteBranch;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.TestDeploymentTasks.ResetTestRepositories
{
    public class ResetTestRepositoriesTask : BaseDeploymentTask
    {
        public ResetTestRepositoriesTask()
            : this(null)
        {
        }

        public ResetTestRepositoriesTask(IDeploymentTask parentTask)
            : base("1AE67EEE-0487-4317-BD28-56C3C0548C36", true, null, parentTask)
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

                Exec(new DeploymentTasksGroup("67DAC3C4-BBD0-4B9B-B457-524A381795B7", true, this)
                {
                    SubItems = new List<IDeploymentTask>()
                    {
                        new DeploymentTasksGroup("6FCF84AE-6D86-4958-9C70-C7D4983DC7C0", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CheckoutTask(new CheckoutTaskOptions()
                                {
                                    RepositoryPath = targetSolution.Path,
                                    BranchName = "master"
                                }, this)
                            }
                        },
                        new DeleteBranchTask(new DeleteBranchTaskOptions()
                        {
                            RepositoryPath = targetSolution.Path,
                            BranchName = currentBranch,
                            IsOrigin = false
                        }, this),
                        new DeleteBranchTask(new DeleteBranchTaskOptions()
                        {
                            RepositoryPath = targetSolution.Path,
                            BranchName = currentBranch,
                            IsOrigin = true
                        }, this)
                    }
                });
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
