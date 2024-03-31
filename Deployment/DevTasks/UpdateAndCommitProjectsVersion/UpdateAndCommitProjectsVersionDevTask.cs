using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.UpdateProjectsVersion;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.UpdateAndCommitProjectsVersion
{
    public class UpdateAndCommitProjectsVersionDevTask : BaseDeploymentTask
    {
        public UpdateAndCommitProjectsVersionDevTask()
            : this(null)
        {
        }

        public UpdateAndCommitProjectsVersionDevTask(IDeploymentTask parentTask)
            : base("70448859-54D0-4738-AC88-9ECCE0AE3354", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateProjectsVersionDevTask(this));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Version has been updated",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates and commits Version and Copyright for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
