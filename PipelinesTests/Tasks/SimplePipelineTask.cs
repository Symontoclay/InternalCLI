using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Tasks
{
    public class SimplePipelineTask: BaseTestDeploymentTask
    {
        public const string OnRunMesage = "Run";

        public SimplePipelineTask(ITaskTestContext testContext)
            : base(testContext, "1AAD6F4A-7DBA-4059-ACDC-3319E57F1755", false, null, null)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _testContext.EmitMessage(GetType(), OnRunMesage);
        }
    }
}
