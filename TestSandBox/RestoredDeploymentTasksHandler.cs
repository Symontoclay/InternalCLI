using CommonUtils.DeploymentTasks;
using NLog;
using PipelinesTests.Common;
using PipelinesTests.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSandBox.RestoredDeploymentTasks;

namespace TestSandBox
{
    public class RestoredDeploymentTasksHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            Case2();
            //Case1();

            _logger.Info("End");
        }

        private void Case2()
        {
            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                _logger.Info(n);
                _logger.Info(type.Name);
                _logger.Info(message);
            };

            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new SimplePipelineTask(testContext));
            deploymentPipeline.Run();
        }

        private void Case1()
        {
            var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                Prefix = "Hi"
                //StartFromBeginning = true
            });

            deploymentPipeline.Add(new TopLevelNewDeploymentTask(new TopLevelNewDeploymentTaskOptions()));

            deploymentPipeline.Run();
        }
    }
}
