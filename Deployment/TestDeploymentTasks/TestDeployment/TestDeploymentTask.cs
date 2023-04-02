using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories;
using Deployment.TestDeploymentTasks.ResetTestRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.TestDeployment
{
    public class TestDeploymentTask : BaseDeploymentTask
    {
        public TestDeploymentTask()
            : this(0u)
        {
        }

        public TestDeploymentTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ResetTestRepositoriesTask(NextDeep));
            Exec(new CopyAndCommitFromProdToTestRepositoriesTask(NextDeep));
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
