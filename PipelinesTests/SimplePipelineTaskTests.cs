using CommonUtils.DeploymentTasks;
using PipelinesTests.Common;
using PipelinesTests.Tasks;

namespace PipelinesTests
{
    public class SimplePipelineTaskTests
    {
        [SetUp]
        public void Setup()
        {
        }

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            var deploymentPipeline = new DeploymentPipeline();
            deploymentPipeline.Add(new SimplePipelineTask(testContext));
            deploymentPipeline.Run();

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            DeploymentPipeline.Run(new SimplePipelineTask(testContext));

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions());
            deploymentPipeline.Add(new SimplePipelineTask(testContext));
            deploymentPipeline.Run();

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            DeploymentPipeline.Run(new SimplePipelineTask(testContext), new DeploymentPipelineOptions());

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            var deploymentPipeline = new DeploymentPipeline(new DeploymentPipelineOptions()
            {
                Prefix = "prefix",
            });
            deploymentPipeline.Add(new SimplePipelineTask(testContext));
            deploymentPipeline.Run();

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
                        Assert.That(type, Is.EqualTo(typeof(SimplePipelineTask)));
                        Assert.That(message, Is.EqualTo("OnRun"));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n, null);
                }
            };

            DeploymentPipeline.Run(new SimplePipelineTask(testContext), new DeploymentPipelineOptions()
            {
                Prefix = "prefix",
            });

            Assert.That(wasRun, Is.EqualTo(true));
        }
    }
}