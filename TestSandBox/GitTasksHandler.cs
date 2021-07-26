using BaseDevPipeline;
using CommonUtils;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.GitHubTasks.GitHubRelease;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.Commit;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.DeleteBranch;
using Deployment.Tasks.GitTasks.Merge;
using Deployment.Tasks.GitTasks.Pull;
using Deployment.Tasks.GitTasks.Push;
using Deployment.Tasks.GitTasks.PushNewBranchToOrigin;
using Deployment.Tasks.GitTasks.UndoChanges;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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

            Case11();
            //Case10();
            //Case9();
            //Case8();
            //Case7();
            //Case6();
            //Case5();
            //Case4();
            //Case3();
            //Case2();
            //Case1();

            //_logger.Info($" = {}");

            _logger.Info("End");
        }

        private void Case11()
        {
            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            _logger.Info($"siteSolution = {siteSolution}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateReleaseNotesDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case10()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            _logger.Info($"settings = {settings}");

            //var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            //_logger.Info($"secretsFileName = {secretsFileName}");

            //var secrets = SecretFile.ReadSecrets(secretsFileName);
            //var token = secrets["GitHub"];
            //var token = "example_token!!!!!";
            var token = settings.GetSecret("GitHub");

            var owner = "metatypeman";
            var repo = "a1";

            var version = "3.6.6";

            //var notesText = "**This** is some *Markdown*";
            var notesText = "* Function\r\n* Method\r\n* Calling function with position parameters\r\n* Improving calling function with named parameters\r\n* Fixing issues";

            var packageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "cleanedNPCPackage.unitypackage");

            _logger.Info($"packageFilePath = {packageFilePath}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new GitHubReleaseTask(new GitHubReleaseTaskOptions() {
                Token = token,
                RepositoryOwner = owner,
                RepositoryName = repo,
                Version = version,
                NotesText = notesText,
                //Prerelease = true,
                //Draft = true,
                Assets = new List<GitHubReleaseAssetOptions>()
                {
                    new GitHubReleaseAssetOptions()
                    {
                        DisplayedName = $"MyPackage-{version}.unitypackage",
                        UploadedFilePath = packageFilePath
                    }
                }
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case9()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new DeleteBranchTask(new DeleteBranchTaskOptions() {
                RepositoryPath = _reposPath,
                BranchName = "tst_branch",
                IsOrigin = true
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case8()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CheckoutTask(new CheckoutTaskOptions()
            {
                RepositoryPath = _reposPath,
                BranchName = "my2.6.14"
            }));

            deploymentPipeline.Add(new MergeTask(new MergeTaskOptions() {
                RepositoryPath = _reposPath,
                BranchName = "tst_branch"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case7()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PullTask(new PullTaskOptions() {
                RepositoryPath = _reposPath
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case6()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PushTask(new PushTaskOptions() {
                RepositoryPath = _reposPath
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case5()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CommitTask(new CommitTaskOptions() {
                RepositoryPath = _reposPath,
                Message = "My first automatic comit!"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case4()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UndoChangesTask(new UndoChangesTaskOptions() {
                RepositoryPath = _reposPath,
                RelativeTargetFilePath = "tmp.txt"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case3()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PushNewBranchToOriginTask(new PushNewBranchToOriginTaskOptions() {
                RepositoryPath = _reposPath,
                BranchName = "tst_branch"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case2()
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CheckoutTask(new CheckoutTaskOptions()
            {
                RepositoryPath = _reposPath,
                BranchName = "tst_branch"
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private void Case1()
        {
            if(GitRepositoryHelper.IsBranchExists(_reposPath, "tst_branch"))
            {
                _logger.Info("'tst_branch' exists!");

                return;
            }

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateBranchTask(new CreateBranchTaskOptions()
            {
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