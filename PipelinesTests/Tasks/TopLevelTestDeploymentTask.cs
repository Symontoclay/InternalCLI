using CommonUtils.DeploymentTasks;
using NUnit.Framework;
using PipelinesTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Tasks
{
    public class TopLevelTestDeploymentTask : BaseTestDeploymentTask
    {
        public TopLevelTestDeploymentTask(ITaskTestContext testContext)
            : base(testContext,"CA38B73F-F095-474F-AA2D-3D93682C43EE", false, null, null)
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
