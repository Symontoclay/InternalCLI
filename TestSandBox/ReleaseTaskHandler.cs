using CommonUtils;
using Deployment.DevTasks.DevFullMaintaining;
using Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders;
using Deployment.Helpers;
using Deployment.ReleaseTasks.GitHubRelease;
using Deployment.ReleaseTasks.MarkAsCompleted;
using Deployment.ReleaseTasks.MergeReleaseBranchToMaster;
using Deployment.ReleaseTasks.ProdSiteBuild;
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

            Case8();
            //Case7();
            //Case6();
            //Case5();
            //Case4();
            //Case3();//<==
            //Case2();
            //Case1();

            _logger.Info("End");
        }

        private void Case8()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new GitHubReleaseReleaseTask(new GitHubReleaseReleaseTaskOptions() { 
                 Repositories = new List<GitHubRepositoryInfo>()
                 {
                     new GitHubRepositoryInfo()
                     {
                         RepositoryName = "a1",
                         RepositoryOwner = "metatypeman"
                     }
                 }
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case7()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new ProdSiteBuildReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case6()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new MarkAsCompletedReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case5()
        {
            var mayIMakeRelease = FutureReleaseGuard.MayIMakeRelease();

            _logger.Info($"mayIMakeRelease = {mayIMakeRelease}");
        }

        private void Case4()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new UpdateAndCommitCopyrightInFileHeadersDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case3()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new DevFullMaintainingDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case2()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

            //deploymentPipeline.Add(new MergeReleaseBranchToMasterReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case1()
        {
            var deploymentPipeline = new OldDeploymentPipeline();

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
