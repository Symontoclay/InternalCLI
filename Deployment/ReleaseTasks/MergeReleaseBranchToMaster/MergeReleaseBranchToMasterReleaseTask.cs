using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Test;
using Deployment.Tasks.GitTasks.Checkout;
using Deployment.Tasks.GitTasks.CreateBranch;
using Deployment.Tasks.GitTasks.DeleteBranch;
using Deployment.Tasks.GitTasks.Merge;
using Deployment.Tasks.GitTasks.Push;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.MergeReleaseBranchToMaster
{
    public class MergeReleaseBranchToMasterReleaseTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static MergeReleaseBranchToMasterReleaseTaskOptions CreateOptions()
        {
            var options = new MergeReleaseBranchToMasterReleaseTaskOptions();

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

#if DEBUG
            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");
#endif

            options.Version = futureReleaseInfo.Version;

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

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

#if DEBUG
            _logger.Info($"options = {options}");
#endif

            return options;
        }

        public MergeReleaseBranchToMasterReleaseTask()
            : this(CreateOptions())
        {
        }

        public MergeReleaseBranchToMasterReleaseTask(MergeReleaseBranchToMasterReleaseTaskOptions options)
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
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif

#if DEBUG
            return;//tmp
#endif

            var versionBranchName = _options.Version;

#if DEBUG
            _logger.Info($"versionBranchName = {versionBranchName}");
#endif

            var projectsForTesting = _options.Repositories.Where(p => !string.IsNullOrWhiteSpace(p.TestedProjPath)).Select(p => p.TestedProjPath).ToList();

#if DEBUG
            _logger.Info($"projectsForTesting = {JsonConvert.SerializeObject(projectsForTesting, Formatting.Indented)}");
#endif

            if(projectsForTesting.Any())
            {
                CheckOut(versionBranchName);

                foreach(var projPath in projectsForTesting)
                {
                    Exec(new TestTask(new TestTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }));
                }
            }

            CheckOut(_masterBranchName);

            var masterBackupBranchName = $"master_backup_before_{_options.Version}_{DateTime.Now:yyyy_MM_dd_HH_mm}";

#if DEBUG
            _logger.Info($"masterBackupBranchName = {masterBackupBranchName}");
#endif

            CreateBranch(masterBackupBranchName);

            var releaseBranchName = $"release_{_options.Version}_{DateTime.Now:yyyy_MM_dd_HH_mm}";

#if DEBUG
            _logger.Info($"releaseBranchName = {releaseBranchName}");
#endif

            CreateBranch(releaseBranchName);

            CheckOut(releaseBranchName);

            foreach (var repository in _options.Repositories)
            {
                Exec(new MergeTask(new MergeTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = versionBranchName
                }));
            }

            if (projectsForTesting.Any())
            {
                foreach (var projPath in projectsForTesting)
                {
                    Exec(new TestTask(new TestTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }));
                }
            }

            CheckOut(_masterBranchName);

            foreach (var repository in _options.Repositories)
            {
                Exec(new MergeTask(new MergeTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = releaseBranchName
                }));
            }

            if (projectsForTesting.Any())
            {
                foreach (var projPath in projectsForTesting)
                {
                    Exec(new TestTask(new TestTaskOptions()
                    {
                        ProjectOrSoutionFileName = projPath
                    }));
                }
            }

            foreach (var repository in _options.Repositories)
            {
                Exec(new PushTask(new PushTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath
                }));
            }

            foreach (var repository in _options.Repositories)
            {
                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = releaseBranchName,
                    IsOrigin = false
                }));

                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = versionBranchName,
                    IsOrigin = false
                }));

                Exec(new DeleteBranchTask(new DeleteBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = versionBranchName,
                    IsOrigin = true
                }));
            }
        }

        private void CheckOut(string branchName)
        {
            foreach (var repository in _options.Repositories)
            {
                Exec(new CheckoutTask(new CheckoutTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = branchName
                }));
            }
        }

        private void CreateBranch(string branchName)
        {
            foreach (var repository in _options.Repositories)
            {
                Exec(new CreateBranchTask(new CreateBranchTaskOptions()
                {
                    RepositoryPath = repository.RepositoryPath,
                    BranchName = branchName
                }));
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
