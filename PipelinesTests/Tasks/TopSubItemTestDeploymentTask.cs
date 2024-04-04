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
        public TopSubItemTestDeploymentTask(ITaskTestContext testContext, TopSubItemTestDeploymentTaskOptions options, IDeploymentTask parentTask)
            : base(testContext, "A4AC61E2-7C15-4ED8-8BED-07FCECA88244", true, options, parentTask)
        {
            _options = options;
        }

        private readonly TopSubItemTestDeploymentTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _testContext.EmitMessage(GetType(), "Begin");

            var itemsList = new List<(int, string)>() { (1, "1://someDir"), (2, "2://someDir"), (3, "3://someDir") };

            Exec(new DeploymentTasksGroup("9537A2AD-3213-4F2A-BE2A-2BD03BDD59BC", false, this)
            {
                SubItems = itemsList.Select(item => new SubItemTestDeploymentTask(_testContext, new SubItemTestDeploymentTaskOptions() 
                { 
                    DirectoryName = item.Item2,
                    N = item.Item1 + _options.N,
                }, this))
            });

            _testContext.EmitMessage(GetType(), "End");
        }
    }
}
