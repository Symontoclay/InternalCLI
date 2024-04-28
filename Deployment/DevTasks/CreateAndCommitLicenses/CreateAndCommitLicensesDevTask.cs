using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CreateLicenses;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitLicenses
{
    public class CreateAndCommitLicensesDevTask : BaseDeploymentTask
    {
        public CreateAndCommitLicensesDevTask()
            : this(null)
        {
        }

        public CreateAndCommitLicensesDevTask(IDeploymentTask parentTask)
            : this(new CreateAndCommitLicensesDevTaskOptions()
            {
                Message = "LICENSE has been updated"
            }, parentTask)
        {
        }

        public CreateAndCommitLicensesDevTask(CreateAndCommitLicensesDevTaskOptions options, IDeploymentTask parentTask)
            : base("E26C3B5B-A37C-4F8D-BCC3-AA2864CA04D1", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateAndCommitLicensesDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CreateLicensesDevTask(this));

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

            sb.AppendLine($"{spaces}Builds and commits LICENSEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
