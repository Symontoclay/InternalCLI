using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CreateLicenses;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateAndCommitLicenses
{
    public class CreateAndCommitLicensesDevTask : OldBaseDeploymentTask
    {
        public CreateAndCommitLicensesDevTask()
            : this(0u)
        {
        }

        public CreateAndCommitLicensesDevTask(uint deep)
            : this(new CreateAndCommitLicensesDevTaskOptions()
            {
                Message = "LICENSE has been updated"
            }, deep)
        {
        }

        public CreateAndCommitLicensesDevTask(CreateAndCommitLicensesDevTaskOptions options, uint deep)
            : base(options, deep)
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
