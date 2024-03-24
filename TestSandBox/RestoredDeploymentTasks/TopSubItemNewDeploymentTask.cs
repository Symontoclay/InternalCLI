using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopSubItemNewDeploymentTask : NewBaseDeploymentTask
    {
        public TopSubItemNewDeploymentTask(TopSubItemNewDeploymentTaskOptions options)
            : this(options, null, null)
        {
        }

        public TopSubItemNewDeploymentTask(TopSubItemNewDeploymentTaskOptions options, INewDeploymentPipelineContext context, INewDeploymentTask parentTask)
            : base(context, "FCD75A10-189C-4F44-815E-0FD6F8AB9A93", true, options, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            var itemsList = new List<string>() { "1://someDir", "2://someDir", "3://someDir" };

            Exec(new NewDeploymentTasksGroup("709E865B-124A-4C10-96C0-2FB0D1ED0B95", false, _context, this)
            {
                SubItems = itemsList.Select(item => new SubItemNewDeploymentTask(new SubItemNewDeploymentTaskOptions() { DirectoryName = item }, _context, this))
            });

            _logger.Info("End");
        }
    }
}
