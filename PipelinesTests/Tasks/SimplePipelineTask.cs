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
    public class SimplePipelineTask: BaseDeploymentTask
    {
        public SimplePipelineTask(ITaskTestContext testContext)
            : base("1AAD6F4A-7DBA-4059-ACDC-3319E57F1755", false, null, null)
        {
            _testContext = testContext;
        }

        private readonly ITaskTestContext _testContext;

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info("OnRun");
#endif

            _testContext.EmitMessage(GetType(), "OnRun");
        }

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
