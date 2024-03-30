using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CreateChangeLogs;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateAndCommitChangeLogs
{
    public class CreateAndCommitChangeLogsDevTask : OldBaseDeploymentTask
    {
        public CreateAndCommitChangeLogsDevTask()
            : this(0u)
        {
        }

        public CreateAndCommitChangeLogsDevTask(uint deep)
            : this(new CreateAndCommitChangeLogsDevTaskOptions()
            {
                Message = "CHANGELOG.md has been updated"
            }, deep)
        {
        }

        public CreateAndCommitChangeLogsDevTask(CreateAndCommitChangeLogsDevTaskOptions options, uint deep)
            : base(options, deep)
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
            Exec(new CreateChangeLogsDevTask(NextDeep));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = _options.Message,
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, NextDeep));
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
