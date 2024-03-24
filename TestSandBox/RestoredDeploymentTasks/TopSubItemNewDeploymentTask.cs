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

            foreach(var item in itemsList)
            {
                Exec(new SubItemNewDeploymentTask(new SubItemNewDeploymentTaskOptions() { DirectoryName = item }, _context, this));
            }

            _logger.Info("End");
        }
    }
}
