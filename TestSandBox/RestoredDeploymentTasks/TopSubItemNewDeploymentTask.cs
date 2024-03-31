using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopSubItemNewDeploymentTask : BaseDeploymentTask
    {
        public TopSubItemNewDeploymentTask(TopSubItemNewDeploymentTaskOptions options)
            : this(options, string.Empty, null)
        {
        }

        public TopSubItemNewDeploymentTask(TopSubItemNewDeploymentTaskOptions options, string key, IDeploymentTask parentTask)
            : base(key, true, options, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            var itemsList = new List<string>() { "1://someDir", "2://someDir", "3://someDir" };

            Exec(new DeploymentTasksGroup("709E865B-124A-4C10-96C0-2FB0D1ED0B95", false, this)
            {
                SubItems = itemsList.Select(item => new SubItemNewDeploymentTask(new SubItemNewDeploymentTaskOptions() { DirectoryName = item }, this))
            });

            _logger.Info("End");
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Do someting.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
