using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.PushNewBranchToOrigin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.TestDeploymentTasks.CreateAndPushVersionBranchInTestRepositories
{
    public class CreateAndPushVersionBranchInTestRepositoriesTask : BaseDeploymentTask
    {
        public CreateAndPushVersionBranchInTestRepositoriesTask()
            : this(null)
        {
        }

        public CreateAndPushVersionBranchInTestRepositoriesTask(IDeploymentTask parentTask)
            : base("75DD967D-062B-4850-B11C-A38E77D17940", true, null, parentTask)
        {
        }

        private readonly string _masterBranchName = "master";

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var testSettings = TestProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();
            var testTargetSolutions = testSettings.GetSolutionsWithMaintainedReleases();

            var releaseMngrRepositoryPath = testSettings.GetSolution(KindOfProject.ReleaseMngrSolution).Path;

#if DEBUG
            //_logger.Info($"releaseMngrRepositoryPath = {releaseMngrRepositoryPath}");
#endif

            var futureReleaseInfo = FutureReleaseInfoReader.Read(releaseMngrRepositoryPath);

            var versionBranchName = futureReleaseInfo.Version;

#if DEBUG
            //_logger.Info($"versionBranchName = {versionBranchName}");
#endif

            foreach (var repository in testTargetSolutions)
            {
                if (GitRepositoryHelper.IsBranchExists(repository.Path, versionBranchName))
                {
                    throw new Exception($"The branch '{versionBranchName}' is already existing at '{repository.Path}'!");
                }
            }

            foreach (var solution in testTargetSolutions)
            {
#if DEBUG
                //_logger.Info($"solution.Path = {solution.Path}");
#endif

                Exec(new DeploymentTasksGroup(MD5Helper.GetHash(solution.Path), true, this)
                {
                    SubItems = new List<IDeploymentTask>()
                    {
                        new DeploymentTasksGroup("E800C61F-B7E4-456F-974F-F2411BAA8192", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                                {
                                    Message = "snapshot",
                                    RepositoryPaths = new List<string>() { solution.Path }
                                }, this)
                            }
                        },
                        new DeploymentTasksGroup("5EA68471-C168-4E07-BDB2-75EDBAB66FBF", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CheckoutTask(new CheckoutTaskOptions()
                                {
                                    RepositoryPath = solution.Path,
                                    BranchName = _masterBranchName
                                }, this)
                            }
                        },
                        new DeploymentTasksGroup("50D01985-A996-4972-A75F-E589510841D4", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                                {
                                    Message = "snapshot",
                                    RepositoryPaths = new List<string>() { solution.Path }
                                }, this)
                            }
                        },
                        new CreateBranchTask(new CreateBranchTaskOptions()
                        {
                            RepositoryPath = solution.Path,
                            BranchName = versionBranchName
                        }, this),
                        new PushNewBranchToOriginTask(new PushNewBranchToOriginTaskOptions()
                        {
                            RepositoryPath = solution.Path,
                            BranchName = versionBranchName
                        }, this),
                        new DeploymentTasksGroup("45D1B60F-859C-4166-A76B-C1C2835A0592", true, this)
                        {
                            SubItems = new List<IDeploymentTask>()
                            {
                                new CheckoutTask(new CheckoutTaskOptions()
                                {
                                    RepositoryPath = solution.Path,
                                    BranchName = versionBranchName
                                }, this)
                            }
                        }
                    }
                });
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Create and push version brunch in test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
