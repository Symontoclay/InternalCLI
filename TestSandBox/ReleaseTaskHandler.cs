using CommonUtils;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders;
using Deployment.ReleaseTasks.MergeReleaseBranchToMaster;
using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class ReleaseTaskHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //Case4();
            Case3();//<==
            //Case2();
            //Case1();

            _logger.Info("End");
        }

        private void Case4()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateAndCommitCopyrightInFileHeadersDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case3()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new DevFullMaintainingDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case2()
        {
            var deploymentPipeline = new DeploymentPipeline();

            //deploymentPipeline.Add(new MergeReleaseBranchToMasterReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case1()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new MergeReleaseBranchToMasterReleaseTask(new MergeReleaseBranchToMasterReleaseTaskOptions() { 
                Version = "0.3.2",
                Repositories = new List<RepositoryItem>()
                {
                    new RepositoryItem()
                    {
                        RepositoryPath = PathsHelper.Normalize("%USERPROFILE%/source/repos/b1")
                    }
                }
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }
    }
}
