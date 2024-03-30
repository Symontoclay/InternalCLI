using System.Collections.Generic;
using System.Linq;

namespace CommonUtils.DeploymentTasks
{
    public class DeploymentTasksGroup : BaseDeploymentTask
    {
        public DeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, IDeploymentPipelineContext context, IDeploymentTask parentTask)
            : this(key, shouldBeSkeepedDuringRestoring, context, parentTask, null)
        {
        }

        public DeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, IDeploymentPipelineContext context, IDeploymentTask parentTask, List<IDeploymentTask> subItems)
            : base(context, key, shouldBeSkeepedDuringRestoring, null, parentTask)
        {
        }

        public IEnumerable<IDeploymentTask> SubItems { get; set; }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            //_logger.Info("Being");

            foreach (var subItem in SubItems ?? Enumerable.Empty<IDeploymentTask>())
            {
                subItem.SetParentTask(this);
                Exec(subItem);
            }

            //_logger.Info("End");
        }
    }
}
