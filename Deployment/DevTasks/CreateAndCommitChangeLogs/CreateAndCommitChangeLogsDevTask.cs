using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CreateChangeLogs;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitChangeLogs
{
    public class CreateAndCommitChangeLogsDevTask : BaseDeploymentTask
    {
        public CreateAndCommitChangeLogsDevTask()
            : this(null)
        {
        }

        public CreateAndCommitChangeLogsDevTask(IDeploymentTask parentTask)
            : this(new CreateAndCommitChangeLogsDevTaskOptions()
            {
                Message = "CHANGELOG.md has been updated"
            }, parentTask)
        {
        }

        public CreateAndCommitChangeLogsDevTask(CreateAndCommitChangeLogsDevTaskOptions options, IDeploymentTask parentTask)
            : base("F5BC338E-99FE-420B-B733-39209285BF72", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateAndCommitChangeLogsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CreateChangeLogsDevTask(this));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = _options.Message,
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds and commits CHANGELOGs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
