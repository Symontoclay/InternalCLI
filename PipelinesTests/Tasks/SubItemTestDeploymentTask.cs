using CommonUtils;
using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;

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
                throw new NotImplementedException("5A4644E6-8BFF-4AA2-AF0B-36B8E0E30939");
            }

            _testContext.EmitMessage(GetType(), "End");
        }
    }
}
