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

            //Case5();
            //Case4();
            Case3();
            //Case2();
            //Case1();

            _logger.Info("End");
        }

        private void Case5()
        {
            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                _logger.Info("--------------------------");
                _logger.Info(n);
                _logger.Info(type.Name);
                _logger.Info(message);
            };

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline();
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch (Exception ex)
            {
                _logger.Info(ex);
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline();
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
        }

        private void Case4()
        {
            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                _logger.Info("--------------------------");
                _logger.Info(n);
                _logger.Info(type.Name);
                _logger.Info(message);
            };

            testContext.EnableFailCase1 = true;

            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
            deploymentPipeline.Run();
        }

        private void Case3()
        {
            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                _logger.Info("--------------------------");
                _logger.Info(n);
                _logger.Info(type.Name);
                _logger.Info(message);
            };

            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
            deploymentPipeline.Run();
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
