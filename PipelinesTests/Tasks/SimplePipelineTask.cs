using PipelinesTests.Common;

namespace PipelinesTests.Tasks
{
    public class SimplePipelineTask: BaseTestDeploymentTask
    {
        public const string OnRunMesage = "Run";
        public const string BeginMesage = "Begin";
        public const string EndMesage = "End";

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
