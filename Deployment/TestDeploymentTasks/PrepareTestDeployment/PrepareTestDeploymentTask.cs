using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories;
using Deployment.TestDeploymentTasks.CreateAndPushVersionBranchInTestRepositories;
using Deployment.TestDeploymentTasks.RemoveReleasesFromTestRepositories;
using Deployment.TestDeploymentTasks.ResetTestRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.PrepareTestDeployment
{
    public class PrepareTestDeploymentTask : OldBaseDeploymentTask
    {
        public PrepareTestDeploymentTask()
            : this(0u)
        {
        }

        public PrepareTestDeploymentTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ResetTestRepositoriesTask(this));

            Exec(new RemoveReleasesFromTestRepositoriesTask(this));

            Exec(new CopyAndCommitFromProdToTestRepositoriesTask(this));

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
