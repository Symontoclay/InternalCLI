using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.RemoveReleasesFromTestRepositories
{
    public class RemoveReleasesFromTestRepositoriesTask : BaseDeploymentTask
    {
        public RemoveReleasesFromTestRepositoriesTask()
            : this(0u)
        {
        }

        public RemoveReleasesFromTestRepositoriesTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Remove releases from test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
