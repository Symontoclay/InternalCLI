using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndTest;
using Deployment.Helpers;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.DeleteBranch;
using Deployment.Tasks.GitTasks.Merge;
using Deployment.Tasks.GitTasks.Push;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.ReleaseTasks.MergeReleaseBranchToMaster
{
    public class MergeReleaseBranchToMasterReleaseTask : BaseDeploymentTask
    {
        private static MergeReleaseBranchToMasterReleaseTaskOptions CreateOptions()
        {
            var options = new MergeReleaseBranchToMasterReleaseTaskOptions();

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            options.Version = futureReleaseInfo.Version;

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            var repositories = new List<RepositoryItem>();

            foreach(var targetSolution in targetSolutions)
            {
                repositories.Add(new RepositoryItem() 
                { 
                    RepositoryPath = targetSolution.Path,
                    TestedProjPath = targetSolution.Projects?.FirstOrDefault(p => p.Kind == KindOfProject.IntegrationTest)?.CsProjPath
                });
            }

            options.Repositories = repositories;

            return options;
        }

        public MergeReleaseBranchToMasterReleaseTask(MergeReleaseBranchToMasterReleaseTaskOptions options)
            : this(options, null)
        {
        }

        public MergeReleaseBranchToMasterReleaseTask(IDeploymentTask parentTask)
            : this(CreateOptions(), parentTask)
        {
        }

        public MergeReleaseBranchToMasterReleaseTask(MergeReleaseBranchToMasterReleaseTaskOptions options, IDeploymentTask parentTask)
            : base("FA1ACB62-22B4-4C61-BD33-89AABA8D9A07", true, options, parentTask)
        {
            _options = options;
        }

        private readonly MergeReleaseBranchToMasterReleaseTaskOptions _options;

        private readonly string _masterBranchName = "master";

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
            ValidateList(nameof(_options.Repositories), _options.Repositories);
            _options.Repositories.ForEach(item => ValidateFileName(nameof(item.RepositoryPath), item.RepositoryPath));
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var versionBranchName = _options.Version;

            var projectsForTesting = _options.Repositories.Where(p => !string.IsNullOrWhiteSpace(p.TestedProjPath)).Select(p => p.TestedProjPath).ToList();

            if(projectsForTesting.Any())
            {
                CheckOut(versionBranchName);

                Exec(new DeploymentTasksGroup("2E48A168-7C39-4BB7-A41F-480ED56E6AA7", false, this)
                {
                    SubItems = projectsForTesting.Select(projPath => new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }, this))
                });
            }

            var masterBackupBranchName = $"master_backup_before_{_options.Version}_{DateTime.Now:yyyy_MM_dd_HH_mm}";

            Exec(new DeploymentTasksGroup("1EE1DE7F-CDFD-4E1B-AD97-A38B592448B4", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DeploymentTasksGroup("8FF3698D-0F48-4A74-816F-C24A5D2E4D81", true, this)
                    {
                        SubItems = CreateCheckOutTasks(_masterBranchName)
                    },
                    new DeploymentTasksGroup("52B83F28-444F-47F7-875F-770155E2584E", true, this)
                    {
                        SubItems = CreateBranchTasks(masterBackupBranchName)
                    }
                }
            });

            var releaseBranchName = $"release_{_options.Version}_{DateTime.Now:yyyy_MM_dd_HH_mm}";

            Exec(new DeploymentTasksGroup()
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DeploymentTasksGroup(),
                    new DeploymentTasksGroup(),
                    new DeploymentTasksGroup(),
                    new DeploymentTasksGroup()
                }
            });

            CreateBranch(releaseBranchName);

            CheckOut(releaseBranchName);

            foreach (var repository in _options.Repositories)
            {
                Exec(new MergeTask(new MergeTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = versionBranchName
                }, NextDeep));
            }

            if (projectsForTesting.Any())
            {
                foreach (var projPath in projectsForTesting)
                {
                    Exec(new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }, NextDeep));
                }
            }

            Exec(new DeploymentTasksGroup());

            CheckOut(_masterBranchName);

            foreach (var repository in _options.Repositories)
            {
                Exec(new MergeTask(new MergeTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = releaseBranchName
                }, NextDeep));
            }

            if (projectsForTesting.Any())
            {
                foreach (var projPath in projectsForTesting)
                {
                    Exec(new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }, NextDeep));
                }
            }

            foreach (var repository in _options.Repositories)
            {
                Exec(new PushTask(new PushTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath
                }, NextDeep));
            }

            Exec(new DeploymentTasksGroup());

            foreach (var repository in _options.Repositories)
            {
                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = releaseBranchName,
                    IsOrigin = false
                }, NextDeep));

                //Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                //{
                //    RepositoryPath = repository.RepositoryPath,
                //    BranchName = versionBranchName,
                //    IsOrigin = false
                //}, NextDeep));

                //Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                //{
                //    RepositoryPath = repository.RepositoryPath,
                //    BranchName = versionBranchName,
                //    IsOrigin = true
                //}, NextDeep));
            }
        }

        private IEnumerable<IDeploymentTask> CreateCheckOutTasks(string branchName)
        {
            return _options.Repositories.Select(repository => new CheckoutTask(new CheckoutTaskOptions()
            {
                RepositoryPath = repository.RepositoryPath,
                BranchName = branchName
            }, this));
        }

        private void CheckOut(string branchName)
        {
            foreach (var task in CreateCheckOutTasks(branchName))
            {
                Exec(task);
            }
        }

        private IEnumerable<IDeploymentTask> CreateBranchTasks(string branchName)
        {
            return _options.Repositories.Select(repository => new CreateBranchTask(new CreateBranchTaskOptions()
            {
                RepositoryPath = repository.RepositoryPath,
                BranchName = branchName
            }, this));
        }

        private void CreateBranch(string branchName)
        {
            foreach (var task in CreateBranchTasks(branchName))
            {
                Exec(task);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds README '{_options.TargetReadmeFileName}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
