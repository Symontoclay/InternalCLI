using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopLevelNewDeploymentTask: BaseDeploymentTask
    {
        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options)
            : this(options, null)
        {
        }

        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options, IDeploymentTask parentTask)
            : base("F32DC066-041B-4786-BCF2-D92E5F5C80F2", true, options, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), "FCD75A10-189C-4F44-815E-0FD6F8AB9A93", this));
            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), "143DEB1D-97E5-44B0-B1FF-5CA85A6A0818", this));

            _logger.Info("End");
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Do something.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
