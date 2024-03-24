using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class NewDeploymentTasksGroup : NewBaseDeploymentTask
    {
        public NewDeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, INewDeploymentPipelineContext context, INewDeploymentTask parentTask)
            : this(key, shouldBeSkeepedDuringRestoring, context, parentTask, null)
        {
        }

        public NewDeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, INewDeploymentPipelineContext context, INewDeploymentTask parentTask, List<INewDeploymentTask> subItems)
            : base(context, key, shouldBeSkeepedDuringRestoring, null, parentTask)
        {
        }

        public IEnumerable<INewDeploymentTask> SubItems { get; set; }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            foreach(var subItem in SubItems ?? Enumerable.Empty<INewDeploymentTask>())
            {
                Exec(subItem);
            }

            _logger.Info("End");
        }
    }
}
