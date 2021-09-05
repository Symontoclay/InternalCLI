using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.MarkAsCompleted
{
    public class MarkAsCompletedReleaseTask : BaseDeploymentTask
    {
        public MarkAsCompletedReleaseTask()
            : this(0u)
        {
        }

        public MarkAsCompletedReleaseTask(uint deep)
            : base(null, deep)
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
