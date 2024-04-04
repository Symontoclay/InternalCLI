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
    public abstract class BaseTestDeploymentTask : BaseDeploymentTask
    {
        public BaseTestDeploymentTask(ITaskTestContext testContext, string key, bool shouldBeSkeepedDuringRestoring, IObjectToString options, IDeploymentTask parentTask)
            : base(key, shouldBeSkeepedDuringRestoring, options, parentTask)
        {
            _testContext = testContext;
        }

        protected readonly ITaskTestContext _testContext;

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Do something.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
