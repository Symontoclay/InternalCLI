using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories;
using Deployment.TestDeploymentTasks.CreateAndPushVersionBranchInTestRepositories;
using Deployment.TestDeploymentTasks.RemoveReleasesFromTestRepositories;
using Deployment.TestDeploymentTasks.ResetTestRepositories;
using System.Collections.Generic;
using System.Text;

namespace Deployment.TestDeploymentTasks.PrepareTestDeployment
{
    public class PrepareTestDeploymentTask : BaseDeploymentTask
    {
        public PrepareTestDeploymentTask()
            : this(null)
        {
        }

        public PrepareTestDeploymentTask(IDeploymentTask parentTask)
            : base("4904247D-C33C-4827-A8EF-49A6C41BFD86", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ResetTestRepositoriesTask(this));

            Exec(new RemoveReleasesFromTestRepositoriesTask(this));

            Exec(new DeploymentTasksGroup("9AFA2113-37A1-4430-86B9-E833E78EE2E7", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CopyAndCommitFromProdToTestRepositoriesTask(this)
                }
            });

            Exec(new CreateAndPushVersionBranchInTestRepositoriesTask(this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Prepares test repositories for testing deployment.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
