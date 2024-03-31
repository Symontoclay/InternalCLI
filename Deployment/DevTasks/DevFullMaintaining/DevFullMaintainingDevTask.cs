using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders;
using Deployment.DevTasks.UpdateAndCommitProjectsVersion;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.DevFullMaintaining
{
    public class DevFullMaintainingDevTask : BaseDeploymentTask
    {
        public DevFullMaintainingDevTask()
            : this(null)
        {
        }

        public DevFullMaintainingDevTask(IDeploymentTask parentTask)
            : base("FFE8EE43-A4B0-4657-A18A-F97F061ECCD6", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "snapshot",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));

            Exec(new UpdateAndCommitProjectsVersionDevTask(this));

            Exec(new UpdateAndCommitCopyrightInFileHeadersDevTask(this));

            //Exec(new RemoveAndCommitSingleLineCommentsDevTask(this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds and commits versions and headers for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
