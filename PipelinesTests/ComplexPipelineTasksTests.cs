using CommonUtils;
using CommonUtils.DeploymentTasks;
using NUnit.Framework;
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case2_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case2_a()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case2_a_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName,
                Prefix = "SomePipeline"
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case3()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName,
                Prefix = "SomePipeline",
                StartFromBeginning = true
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case3_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline",
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline",
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case3_a()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName,
                StartFromBeginning = true
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case3_a_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case4()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext));

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case4_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline",
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline();
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case4_a()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext));

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case4_a_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("1"));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("2"));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("3"));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 30:
                        Assert.That(n, Is.EqualTo(30));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 31:
                        Assert.That(n, Is.EqualTo(31));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 32:
                        Assert.That(n, Is.EqualTo(32));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 33:
                        Assert.That(n, Is.EqualTo(33));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("4"));
                        break;

                    case 34:
                        Assert.That(n, Is.EqualTo(34));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 35:
                        Assert.That(n, Is.EqualTo(35));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 36:
                        Assert.That(n, Is.EqualTo(36));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 37:
                        Assert.That(n, Is.EqualTo(37));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 38:
                        Assert.That(n, Is.EqualTo(38));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 39:
                        Assert.That(n, Is.EqualTo(39));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 40:
                        Assert.That(n, Is.EqualTo(40));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 41:
                        Assert.That(n, Is.EqualTo(41));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 42:
                        Assert.That(n, Is.EqualTo(42));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    StartFromBeginning = true
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline();
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case5()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.OnRunMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            DeploymentPipeline.Run(new SimplePipelineTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName,
                Prefix = "OtherPipeline"
            });

            DeploymentPipeline.Run(new TopLevelTestDeploymentTask(testContext), new DeploymentPipelineOptions()
            {
                UseAutorestoring = true,
                DirectoryForAutorestoring = tempDir.FullName,
                Prefix = "SomePipeline"
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }

        [Test]
        [Parallelizable]
        public void Case5_1()
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

                    //Pipeline restarted
                    case 19:
                        Assert.That(n, Is.EqualTo(19));
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.OnRunMesage));
                        break;

                    case 20:
                        Assert.That(n, Is.EqualTo(20));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 21:
                        Assert.That(n, Is.EqualTo(21));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 22:
                        Assert.That(n, Is.EqualTo(22));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 23:
                        Assert.That(n, Is.EqualTo(23));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("5"));
                        break;

                    case 24:
                        Assert.That(n, Is.EqualTo(24));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 25:
                        Assert.That(n, Is.EqualTo(25));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.BeginMesage));
                        break;

                    case 26:
                        Assert.That(n, Is.EqualTo(26));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo("6"));
                        break;

                    case 27:
                        Assert.That(n, Is.EqualTo(27));
                        Assert.That(type, Is.EqualTo(typeof(SubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 28:
                        Assert.That(n, Is.EqualTo(28));
                        Assert.That(type, Is.EqualTo(typeof(TopSubItemTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    case 29:
                        Assert.That(n, Is.EqualTo(29));
                        Assert.That(type, Is.EqualTo(typeof(TopLevelTestDeploymentTask)));
                        Assert.That(message, Is.EqualTo(SimplePipelineTask.EndMesage));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            using var tempDir = new TempDirectory();

            testContext.EnableFailCase1 = true;

            try
            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }
            catch
            {
            }

            testContext.EnableFailCase1 = false;

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "OtherPipeline"
                });
                deploymentPipeline.Add(new SimplePipelineTask(testContext));
                deploymentPipeline.Run();
            }

            {
                var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
                {
                    UseAutorestoring = true,
                    DirectoryForAutorestoring = tempDir.FullName,
                    Prefix = "SomePipeline"
                });
                deploymentPipeline.Add(new TopLevelTestDeploymentTask(testContext));
                deploymentPipeline.Run();
            }

            Assert.That(wasRun, Is.EqualTo(true));
        }
    }
}
