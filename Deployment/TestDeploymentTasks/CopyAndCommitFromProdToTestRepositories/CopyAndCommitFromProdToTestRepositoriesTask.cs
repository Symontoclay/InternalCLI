using BaseDevPipeline;
using BaseDevPipeline.Data;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories
{
    public class CopyAndCommitFromProdToTestRepositoriesTask : OldBaseDeploymentTask
    {
        public CopyAndCommitFromProdToTestRepositoriesTask()
            : this(0u)
        {
        }

        public CopyAndCommitFromProdToTestRepositoriesTask(uint deep)
            : base(null, deep)            
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
                }, NextDeep));
            }

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                RepositoryPaths = testTargetSolutions.Select(p => p.Path).ToList(),
                Message = "Commit uncommited changes"
            }, NextDeep));
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
