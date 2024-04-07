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
            : base(testContext, MD5Helper.GetHash(options.DirectoryName), true, options, parentTask)
        {
            _options = options;
        }

        private readonly SubItemTestDeploymentTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _testContext.EmitMessage(GetType(), "Begin");
            _testContext.EmitMessage(GetType(), _options.N.ToString());

            if (_testContext.EnableFailCase1 && _options.N == 5)
            {
                throw new NotImplementedException();
            }

            _testContext.EmitMessage(GetType(), "End");
        }
    }
}
