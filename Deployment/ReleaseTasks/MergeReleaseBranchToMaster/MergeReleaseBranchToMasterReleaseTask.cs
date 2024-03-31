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
                Exec(new DeploymentTasksGroup("FBF387D5-D1C9-4164-B2D2-5CE82DCFCE8A", true, this)
                {
                    SubItems = new List<IDeploymentTask>()
                    {
                        new DeploymentTasksGroup("64922EF9-3184-48CC-9E36-0FA922F2337A", false, this)
                        {
                            SubItems = CreateCheckOutTasks(versionBranchName)
                        },
                        new DeploymentTasksGroup("2E48A168-7C39-4BB7-A41F-480ED56E6AA7", false, this)
                        {
                            SubItems = projectsForTesting.Select(projPath => new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                            {
                                ProjectOrSoutionFileName = projPath
                            }, this))
                        }
                    }
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

            Exec(new DeploymentTasksGroup("4DBD490A-CF92-4193-9436-7B59C81C6862", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DeploymentTasksGroup("5818B9BB-5CD0-421D-B618-466322B03AB1", true, this)
                    {
                        SubItems = CreateBranchTasks(releaseBranchName)
                    },
                    new DeploymentTasksGroup("73AD9821-5017-44F1-AA81-394861F9CB68", true, this)
                    {
                        SubItems = CreateCheckOutTasks(releaseBranchName)
                    },
                    new DeploymentTasksGroup("1F8A602C-DFEA-48CD-9DC9-1C7BF939AA86", true, this)
                    {
                        SubItems = _options.Repositories.Select(repository => new MergeTask(new MergeTaskOptions()
                        {
                            RepositoryPath = repository.RepositoryPath,
                            BranchName = versionBranchName
                        }, this))
                    },
                    new DeploymentTasksGroup("851C90A8-D7B6-4520-9867-18BBFF595539", false, this)
                    {
                        SubItems = projectsForTesting.Select(projPath => new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                        {
                            ProjectOrSoutionFileName = projPath
                        }, this))
                    }
                }
            });

            Exec(new DeploymentTasksGroup("E6B96BCD-9B54-4D32-8B5F-7CF5AAE014D5", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new DeploymentTasksGroup("46AC1A34-6437-4D5C-8A97-C9670F39EFCC", true, this)
                    {
                        SubItems = CreateCheckOutTasks(_masterBranchName)
                    },
                    new DeploymentTasksGroup("17723B45-4E0E-49D3-8C9B-4EE84A0B1A58", true, this)
                    {
                        SubItems = _options.Repositories.Select(repository => new MergeTask(new MergeTaskOptions()
                        {
                            RepositoryPath = repository.RepositoryPath,
                            BranchName = releaseBranchName
                        }, this))
                    },
                    new DeploymentTasksGroup("DCDFE8E8-2711-4093-92CD-FC945A5EC16B", false, this)
                    {
                        SubItems = projectsForTesting.Select(projPath => new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
                        {
                            ProjectOrSoutionFileName = projPath
                        }, this))
                    },
                    new DeploymentTasksGroup("D7273B46-A7D7-4109-8ED7-8CDEE9E14E6C", false, this)
                    {
                        SubItems = _options.Repositories.Select(repository => new PushTask(new PushTaskOptions()
                        {
                            RepositoryPath = repository.RepositoryPath
                        }, this))
                    }
                }
            });

            Exec(new DeploymentTasksGroup("3584C4BD-D86C-4437-8F34-5F11D932B1CD", true, this)
            {
                SubItems = _options.Repositories.Select(repository => new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = releaseBranchName,
                    IsOrigin = false
                }, this))
            });

            //foreach (var repository in _options.Repositories)
            //{
                //Exec();

                //Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                //{
                //    RepositoryPath = repository.RepositoryPath,
                //    BranchName = versionBranchName,
                //    IsOrigin = false
                //}, this));

                //Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                //{
                //    RepositoryPath = repository.RepositoryPath,
                //    BranchName = versionBranchName,
                //    IsOrigin = true
                //}, this));
            //}
        }

        private IEnumerable<IDeploymentTask> CreateCheckOutTasks(string branchName)
        {
            return _options.Repositories.Select(repository => new CheckoutTask(new CheckoutTaskOptions()
            {
                RepositoryPath = repository.RepositoryPath,
                BranchName = branchName
            }, this));
        }

        private IEnumerable<IDeploymentTask> CreateBranchTasks(string branchName)
        {
            return _options.Repositories.Select(repository => new CreateBranchTask(new CreateBranchTaskOptions()
            {
                RepositoryPath = repository.RepositoryPath,
                BranchName = branchName
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Merges release branch to master branch.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
