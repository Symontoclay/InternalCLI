using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Tasks
{
    public class TopSubItemTestDeploymentTask : BaseTestDeploymentTask
    {
        public TopSubItemTestDeploymentTask(ITaskTestContext testContext, IDeploymentTask parentTask)
            : base(testContext, "A4AC61E2-7C15-4ED8-8BED-07FCECA88244", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            _testContext.EmitMessage(GetType(), "Begin");
        }
    }
}
