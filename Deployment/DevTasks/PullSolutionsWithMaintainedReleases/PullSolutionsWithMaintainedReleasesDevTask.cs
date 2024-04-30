using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.GitTasks.Pull;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.PullSolutionsWithMaintainedReleases
{
    public class PullSolutionsWithMaintainedReleasesDevTask : BaseDeploymentTask
    {
        public PullSolutionsWithMaintainedReleasesDevTask()
            : this(null)
        {
        }

        public PullSolutionsWithMaintainedReleasesDevTask(IDeploymentTask parentTask)
            : base("755804CD-B968-4555-9064-EC765E32992C", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            foreach(var targetSolution in targetSolutions)
            {
                Exec(new PullTask(new PullTaskOptions()
                {
                    RepositoryPath = targetSolution.Path
                }, this));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Pulls commits of all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
