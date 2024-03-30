using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.PushNewBranchToOrigin;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.CreateAndPushVersionBranchInTestRepositories
{
    public class CreateAndPushVersionBranchInTestRepositoriesTask : OldBaseDeploymentTask
    {
        public CreateAndPushVersionBranchInTestRepositoriesTask()
            : this(0u)
        {
        }

        public CreateAndPushVersionBranchInTestRepositoriesTask(uint deep)
            : base(null, deep)
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

                Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                {
                    Message = "snapshot",
                    RepositoryPaths = new List<string>() { solution.Path }
                }, NextDeep));

                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = solution.Path,
                    BranchName = _masterBranchName
                }, NextDeep));

                Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                {
                    Message = "snapshot",
                    RepositoryPaths = new List<string>() { solution.Path }
                }, NextDeep));

                Exec(new CreateBranchTask(new CreateBranchTaskOptions()
                {
                    RepositoryPath = solution.Path,
                    BranchName = versionBranchName
                }, NextDeep));

                Exec(new PushNewBranchToOriginTask(new PushNewBranchToOriginTaskOptions()
                {
                    RepositoryPath = solution.Path,
                    BranchName = versionBranchName
                }));

                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = solution.Path,
                    BranchName = versionBranchName
                }, NextDeep));
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
