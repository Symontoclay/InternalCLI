using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CreateCodeOfConducts;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitCodeOfConducts
{
    public class CreateAndCommitCodeOfConductsDevTask : BaseDeploymentTask
    {
        public CreateAndCommitCodeOfConductsDevTask()
            : this(null)
        {
        }

        public CreateAndCommitCodeOfConductsDevTask(IDeploymentTask parentTask)
            : this(new CreateAndCommitCodeOfConductsDevTaskOptions()
            {
                Message = "CODE_OF_CONDUCT.md has been updated"
            }, parentTask)
        {
        }

        public CreateAndCommitCodeOfConductsDevTask(CreateAndCommitCodeOfConductsDevTaskOptions options, IDeploymentTask parentTask)
            : base("CB70C8AE-16F0-4140-A6D7-3785B66FF759", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateAndCommitCodeOfConductsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CreateCodeOfConductsDevTask(this));

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

            sb.AppendLine($"{spaces}Builds and commits CODE_OF_CONDUCTs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
