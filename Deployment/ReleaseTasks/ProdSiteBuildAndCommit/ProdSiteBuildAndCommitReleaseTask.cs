using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.ReleaseTasks.ProdSiteBuild;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.ProdSiteBuildAndCommit
{
    public class ProdSiteBuildAndCommitReleaseTask : OldBaseDeploymentTask
    {
        public ProdSiteBuildAndCommitReleaseTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ProdSiteBuildReleaseTask(NextDeep));

            var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Site has been updated",
                RepositoryPaths = new List<string>() { siteSolution.Path }
            }, NextDeep));
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
