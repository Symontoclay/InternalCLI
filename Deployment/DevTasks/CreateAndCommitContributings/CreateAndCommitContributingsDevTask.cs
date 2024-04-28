using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CreateContributings;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitContributings
{
    public class CreateAndCommitContributingsDevTask : BaseDeploymentTask
    {
        public CreateAndCommitContributingsDevTask()
            : this(null)
        {
        }

        public CreateAndCommitContributingsDevTask(IDeploymentTask parentTask)
            : this(new CreateAndCommitContributingsDevTaskOptions()
            {
                Message = "CONTRIBUTING.md has been updated"
            }, parentTask)
        {
        }

        public CreateAndCommitContributingsDevTask(CreateAndCommitContributingsDevTaskOptions options, IDeploymentTask parentTask)
            : base("EE923867-BE01-4D9C-A001-8151EEE0C423", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateAndCommitContributingsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CreateContributingsDevTask(this));

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

            sb.AppendLine($"{spaces}Builds and commits CONTRIBUTINGs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
