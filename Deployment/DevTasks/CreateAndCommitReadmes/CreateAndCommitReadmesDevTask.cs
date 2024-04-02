using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CreateReadmes;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitReadmes
{
    public class CreateAndCommitReadmesDevTask : BaseDeploymentTask
    {
        public CreateAndCommitReadmesDevTask()
            : this(null)
        {
        }

        public CreateAndCommitReadmesDevTask(IDeploymentTask parentTask)
            : this(new CreateAndCommitReadmesDevTaskOptions() { 
                Message = "README.md has been updated"
            }, parentTask)
        {
        }

        public CreateAndCommitReadmesDevTask(CreateAndCommitReadmesDevTaskOptions options, IDeploymentTask parentTask)
            : base("83CCE474-A140-4E5D-9715-BCE5D49EE0D9", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateAndCommitReadmesDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CreateReadmesDevTask(this));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions() { 
                Message = _options.Message,
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds and commits READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
