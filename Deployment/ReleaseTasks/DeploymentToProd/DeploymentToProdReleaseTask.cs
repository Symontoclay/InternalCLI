using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.DeploymentToProd
{
    public class DeploymentToProdReleaseTask : BaseDeploymentTask
    {
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

            //sb.AppendLine($"{spaces}Builds and commits READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
