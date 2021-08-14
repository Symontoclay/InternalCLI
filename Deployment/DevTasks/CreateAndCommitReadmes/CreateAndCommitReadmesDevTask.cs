using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CreateReadmes;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Commit;
using Deployment.Tasks.GitTasks.Push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateAndCommitReadmes
{
    public class CreateAndCommitReadmesDevTask : BaseDeploymentTask
    {
        public CreateAndCommitReadmesDevTask()
            : this(new CreateAndCommitReadmesDevTaskOptions() { 
                Message = "README.md has been updated"
            })
        {
        }

        public CreateAndCommitReadmesDevTask(CreateAndCommitReadmesDevTaskOptions options)
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
            Exec(new CreateReadmesDevTask());

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

            foreach (var targetSolution in targetSolutions)
            {
                Exec(new CommitTask(new CommitTaskOptions()
                {
                    RepositoryPath = targetSolution.Path,
                    Message = _options.Message
                }));
            }

            foreach (var targetSolution in targetSolutions)
            {
                Exec(new PushTask(new PushTaskOptions()
                {
                    RepositoryPath = targetSolution.Path
                }));
            }
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
