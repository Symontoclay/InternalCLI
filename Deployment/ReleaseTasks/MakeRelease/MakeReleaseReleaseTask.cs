using CommonUtils.DebugHelpers;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.ReleaseTasks.DeploymentToProd;
using Deployment.ReleaseTasks.MergeReleaseBranchToMaster;
using Deployment.Tasks;
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
        /// <inheritdoc/>
        protected override void OnRun()
        {
            throw new NotImplementedException();

            Exec(new DevFullMaintainingDevTask());

            throw new NotImplementedException();

            Exec(new MergeReleaseBranchToMasterReleaseTask());

            throw new NotImplementedException();

            Exec(new DeploymentToProdReleaseTask());

            throw new NotImplementedException();

            MarkAsReleased();

            throw new NotImplementedException();
        }

        private void MarkAsReleased()
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
