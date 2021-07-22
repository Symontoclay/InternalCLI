using CommonUtils;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CreateBranch;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class GitTasksHandler
    {
        public GitTasksHandler()
        {
            _reposPath = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\a1\");

            _logger.Info($"_reposPath = {_reposPath}");
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private string _reposPath;

        public void Run()
        {
            _logger.Info("Begin");

            Case1();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void Case1()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateBranchTask(new CreateBranchTaskOptions() { 
                RepositoryPath = _reposPath,
                BranchName = "tst_branch"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }
    }
}
/*



















*/