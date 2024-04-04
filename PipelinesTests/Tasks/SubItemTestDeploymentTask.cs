using CommonUtils;
using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Tasks
{
    public class SubItemTestDeploymentTask : BaseTestDeploymentTask
    {
        public SubItemTestDeploymentTask(ITaskTestContext testContext, SubItemTestDeploymentTaskOptions options, IDeploymentTask parentTask)
            : base(testContext, MD5Helper.GetHash(options.DirectoryName), false, options, parentTask)
        {
            _options = options;
        }

        private readonly SubItemTestDeploymentTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _testContext.EmitMessage(GetType(), "Begin");

            _testContext.EmitMessage(GetType(), "End");
        }
    }
}
