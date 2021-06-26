using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class DeploymentTaskBasedBuildHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions() { TargetDir = "a" }));
            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = @"c:\Users\Acer\source\repos\InternalCLI\CSharpUtils\",
                DestDir = Path.Combine(Directory.GetCurrentDirectory(), "a")
            })); ;

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }
    }
}
