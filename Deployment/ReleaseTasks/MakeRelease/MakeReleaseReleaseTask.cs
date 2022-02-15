using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.Helpers;
using Deployment.ReleaseTasks.DeploymentToProd;
using Deployment.ReleaseTasks.MarkAsCompleted;
using Deployment.ReleaseTasks.MergeReleaseBranchToMaster;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.SetUpRepository;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.MakeRelease
{
    /// <summary>
    /// It is the task that makes relese and closes version development!
    /// Be careful during using the task.
    /// </summary>
    public class MakeReleaseReleaseTask : BaseDeploymentTask
    {
        public MakeReleaseReleaseTask()
            : this(0u)
        {
        }
        
        public MakeReleaseReleaseTask(uint deep)
            : base(null, deep)
        {
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void OnRun()
        {
            if(!FutureReleaseGuard.MayIMakeRelease())
            {
                _logger.Info("Making release is forbiden! New version has not been started!");

                return;
            }

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            var versionBranchName = futureReleaseInfo.Version;

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

            //Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            //{
            //    Message = "snapshot",
            //    RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            //}, NextDeep));

            foreach (var repository in targetSolutions)
            {
                Exec(new SetUpRepositoryTask(new SetUpRepositoryTaskOptions()
                {
                    RepositoryPath = repository.Path
                }, NextDeep));
            }

            foreach (var repository in targetSolutions)
            {
                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = repository.Path,
                    BranchName = versionBranchName
                }, NextDeep));
            }

            Exec(new DevFullMaintainingDevTask(NextDeep));

            Exec(new MergeReleaseBranchToMasterReleaseTask(NextDeep));

            Exec(new DeploymentToProdReleaseTask(NextDeep));

            Exec(new MarkAsCompletedReleaseTask(NextDeep));
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
