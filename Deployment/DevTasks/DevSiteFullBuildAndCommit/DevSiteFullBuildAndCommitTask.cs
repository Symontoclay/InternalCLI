using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.DevSiteFullBuild;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.DevTasks.DevSiteFullBuildAndCommit
{
    public class DevSiteFullBuildAndCommitTask : BaseDeploymentTask
    {
        public DevSiteFullBuildAndCommitTask()
            : this(null)
        {
        }
        
        public DevSiteFullBuildAndCommitTask(IDeploymentTask parentTask)
            : base("7ADE98A6-A7DC-45C4-A242-52F334329A52", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new DevSiteFullBuildTask(this));

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

            sb.AppendLine($"{spaces}Makes Full dev site build and commits.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
