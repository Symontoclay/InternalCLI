using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.ReleaseTasks.MarkAsCompleted
{
    public class MarkAsCompletedReleaseTask : BaseDeploymentTask
    {
        public MarkAsCompletedReleaseTask()
            : this(null)
        {
        }

        public MarkAsCompletedReleaseTask(IDeploymentTask parentTask)
            : base("ACB8AF3C-DEC2-4411-A952-7DB067B1D524", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var futureReleaseInfo = FutureReleaseInfoReader.ReadSource();

            futureReleaseInfo.Status = FutureReleaseStatus.Completed.ToString();
            futureReleaseInfo.FinishDate = DateTime.Now;

            FutureReleaseInfoWriter.WriteSource(futureReleaseInfo);
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
