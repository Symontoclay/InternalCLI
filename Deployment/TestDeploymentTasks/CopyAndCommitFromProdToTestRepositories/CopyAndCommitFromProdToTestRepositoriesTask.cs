using BaseDevPipeline;
using BaseDevPipeline.Data;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Linq;
using System.Text;

namespace Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories
{
    public class CopyAndCommitFromProdToTestRepositoriesTask : BaseDeploymentTask
    {
        public CopyAndCommitFromProdToTestRepositoriesTask()
            : this(null)
        {
        }

        public CopyAndCommitFromProdToTestRepositoriesTask(IDeploymentTask parentTask)
            : base("E89CD07C-F352-4C32-B863-248DF2E92D70", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prodSettings = ProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();
            var testSettings = TestProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();

            var prodTargetSolutions = prodSettings.Solutions;//.GetSolutionsWithMaintainedReleases();
            var testTargetSolutions = testSettings.Solutions;//.GetSolutionsWithMaintainedReleases();
            var testTargetSolutionsDict = testTargetSolutions.ToDictionary(p => p.Name, p => p);

            foreach(var prodSolution in prodTargetSolutions)
            {
#if DEBUG
                //_logger.Info($"prodSolution.Name = {prodSolution.Name}");
#endif

                if(!testTargetSolutionsDict.TryGetValue(prodSolution.Name, out ISolutionSettings testSolution))
                {
                    throw new Exception($"Absent test repository for {prodSolution.Name}");
                }

#if DEBUG
                //_logger.Info($"prodSolution.Path = {prodSolution.Path}");
                //_logger.Info($"testSolution.Path = {testSolution.Path}");
#endif

                Exec(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions()
                {
                    SourceDir = prodSolution.Path,
                    DestDir = testSolution.Path,
                }, this));
            }

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                RepositoryPaths = testTargetSolutions.Select(p => p.Path).ToList(),
                Message = "Commit uncommited changes"
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copy prod repositories to test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
