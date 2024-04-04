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
            _testContext.EmitMessage(GetType(), "Begin");

            Exec(new DeploymentTasksGroup("6D6F08F4-461C-4D99-9D4B-927A09389822", true, this)
            {
                SubItems = new List<IDeploymentTask> 
                { 
                    new TopSubItemTestDeploymentTask(_testContext, new TopSubItemTestDeploymentTaskOptions()
                    {
                        N = 0
                    }, this) 
                }
            });
            Exec(new DeploymentTasksGroup("A05C9D44-14A9-4444-B249-F9BEED404A88", true, this)
            {
                SubItems = new List<IDeploymentTask> 
                { 
                    new TopSubItemTestDeploymentTask(_testContext, new TopSubItemTestDeploymentTaskOptions()
                    {
                        N = 3
                    }, this) 
                }
            });

            _testContext.EmitMessage(GetType(), "End");
        }
    }
}
