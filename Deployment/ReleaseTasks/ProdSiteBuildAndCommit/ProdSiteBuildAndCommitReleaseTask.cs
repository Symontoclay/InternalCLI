using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.ReleaseTasks.ProdSiteBuild;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Collections.Generic;
using System.Text;

namespace Deployment.ReleaseTasks.ProdSiteBuildAndCommit
{
    public class ProdSiteBuildAndCommitReleaseTask : BaseDeploymentTask
    {
        public ProdSiteBuildAndCommitReleaseTask(IDeploymentTask parentTask)
            : base("8F571945-06A1-4EEA-97E0-AF85813741B6", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ProdSiteBuildReleaseTask(this));

            var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Site has been updated",
                RepositoryPaths = new List<string>() { siteSolution.Path }
            }, this));
        }
        
        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Makes PROD site build and commits.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
