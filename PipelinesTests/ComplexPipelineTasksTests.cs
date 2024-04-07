using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;
using PipelinesTests.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests
{
    public class ComplexPipelineTasksTests
    {
        [Test]
        [Parallelizable]
        public void Case1()
        {
            var wasRun = false;

            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                switch (n)
                {
                    case 1:
                        wasRun = true;
                        Assert.That(n, Is.EqualTo(1));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 2:
                        Assert.That(n, Is.EqualTo(2));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 3:
                        Assert.That(n, Is.EqualTo(3));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 4:
                        Assert.That(n, Is.EqualTo(4));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 5:
                        Assert.That(n, Is.EqualTo(5));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 6:
                        Assert.That(n, Is.EqualTo(6));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 7:
                        Assert.That(n, Is.EqualTo(7));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 8:
                        Assert.That(n, Is.EqualTo(8));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 9:
                        Assert.That(n, Is.EqualTo(9));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 10:
                        Assert.That(n, Is.EqualTo(10));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 11:
                        Assert.That(n, Is.EqualTo(11));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 12:
                        Assert.That(n, Is.EqualTo(12));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 13:
                        Assert.That(n, Is.EqualTo(13));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 14:
                        Assert.That(n, Is.EqualTo(14));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 15:
                        Assert.That(n, Is.EqualTo(15));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 16:
                        Assert.That(n, Is.EqualTo(16));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 17:
                        Assert.That(n, Is.EqualTo(17));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 18:
                        Assert.That(n, Is.EqualTo(18));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
            deploymentPipeline.Run();

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case1_1()
        {
            var wasRun = false;

            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                switch (n)
                {
                    case 1:
                        wasRun = true;
                        Assert.That(n, Is.EqualTo(1));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 2:
                        Assert.That(n, Is.EqualTo(2));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 3:
                        Assert.That(n, Is.EqualTo(3));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 4:
                        Assert.That(n, Is.EqualTo(4));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 5:
                        Assert.That(n, Is.EqualTo(5));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 6:
                        Assert.That(n, Is.EqualTo(6));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 7:
                        Assert.That(n, Is.EqualTo(7));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 8:
                        Assert.That(n, Is.EqualTo(8));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 9:
                        Assert.That(n, Is.EqualTo(9));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 10:
                        Assert.That(n, Is.EqualTo(10));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 11:
                        Assert.That(n, Is.EqualTo(11));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 12:
                        Assert.That(n, Is.EqualTo(12));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 13:
                        Assert.That(n, Is.EqualTo(13));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 14:
                        Assert.That(n, Is.EqualTo(14));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 15:
                        Assert.That(n, Is.EqualTo(15));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 16:
                        Assert.That(n, Is.EqualTo(16));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 17:
                        Assert.That(n, Is.EqualTo(17));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 18:
                        Assert.That(n, Is.EqualTo(18));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext));

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case1_a()
        {
            var wasRun = false;

            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                switch (n)
                {
                    case 1:
                        wasRun = true;
                        Assert.That(n, Is.EqualTo(1));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 2:
                        Assert.That(n, Is.EqualTo(2));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 3:
                        Assert.That(n, Is.EqualTo(3));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 4:
                        Assert.That(n, Is.EqualTo(4));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 5:
                        Assert.That(n, Is.EqualTo(5));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 6:
                        Assert.That(n, Is.EqualTo(6));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 7:
                        Assert.That(n, Is.EqualTo(7));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 8:
                        Assert.That(n, Is.EqualTo(8));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 9:
                        Assert.That(n, Is.EqualTo(9));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 10:
                        Assert.That(n, Is.EqualTo(10));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 11:
                        Assert.That(n, Is.EqualTo(11));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 12:
                        Assert.That(n, Is.EqualTo(12));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 13:
                        Assert.That(n, Is.EqualTo(13));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 14:
                        Assert.That(n, Is.EqualTo(14));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 15:
                        Assert.That(n, Is.EqualTo(15));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 16:
                        Assert.That(n, Is.EqualTo(16));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 17:
                        Assert.That(n, Is.EqualTo(17));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 18:
                        Assert.That(n, Is.EqualTo(18));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
            {
                Prefix = "SomePipeline"
            });
            deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
            deploymentPipeline.Run();

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case1_a_1()
        {
            var wasRun = false;

            var testContext = new TaskTestContext();
            testContext.OnMessage += (n, type, message) =>
            {
                switch (n)
                {
                    case 1:
                        wasRun = true;
                        Assert.That(n, Is.EqualTo(1));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 2:
                        Assert.That(n, Is.EqualTo(2));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 3:
                        Assert.That(n, Is.EqualTo(3));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 4:
                        Assert.That(n, Is.EqualTo(4));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 5:
                        Assert.That(n, Is.EqualTo(5));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 6:
                        Assert.That(n, Is.EqualTo(6));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 7:
                        Assert.That(n, Is.EqualTo(7));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 8:
                        Assert.That(n, Is.EqualTo(8));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 9:
                        Assert.That(n, Is.EqualTo(9));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 10:
                        Assert.That(n, Is.EqualTo(10));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 11:
                        Assert.That(n, Is.EqualTo(11));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 12:
                        Assert.That(n, Is.EqualTo(12));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 13:
                        Assert.That(n, Is.EqualTo(13));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 14:
                        Assert.That(n, Is.EqualTo(14));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 15:
                        Assert.That(n, Is.EqualTo(15));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 16:
                        Assert.That(n, Is.EqualTo(16));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 17:
                        Assert.That(n, Is.EqualTo(17));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 18:
                        Assert.That(n, Is.EqualTo(18));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                Prefix = "SomePipeline"
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case2()
        {
            throw new NotImplementedException();
        }
    }
}
