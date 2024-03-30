using System.Collections.Generic;
using System.Linq;

namespace CommonUtils.DeploymentTasks
{
    public class DeploymentTasksGroup : BaseDeploymentTask
    {
        public DeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, IDeploymentTask parentTask)
            : this(key, shouldBeSkeepedDuringRestoring, parentTask, null)
        {
        }

        public DeploymentTasksGroup(string key, bool shouldBeSkeepedDuringRestoring, IDeploymentTask parentTask, List<IDeploymentTask> subItems)
            : base(key, shouldBeSkeepedDuringRestoring, null, parentTask)
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
