using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.RemoveSingleLineComments;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Commit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.RemoveAndCommitSingleLineComments
{
    public class RemoveAndCommitSingleLineCommentsDevTask : BaseDeploymentTask
    {
        private static RemoveAndCommitSingleLineCommentsOptions CreateDefaultOptions()
        {
            var result = new RemoveAndCommitSingleLineCommentsOptions();

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            result.TargetDir = targetSolutions.First(p => p.Kind == KindOfProject.CoreSolution).Path;

            return result;
        }

        public RemoveAndCommitSingleLineCommentsDevTask()
            : this(0u)
        {
        }

        public RemoveAndCommitSingleLineCommentsDevTask(uint deep)
            : this(CreateDefaultOptions(), deep)
        {
        }

        public RemoveAndCommitSingleLineCommentsDevTask(RemoveAndCommitSingleLineCommentsOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly RemoveAndCommitSingleLineCommentsOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.TargetDir), _options.TargetDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new RemoveSingleLineCommentsDevTask(new RemoveSingleLineCommentsDevTaskOptions()
            {
                TargetDir = _options.TargetDir,
            }));

            Exec(new CommitTask(new CommitTaskOptions()
            {
                RepositoryPath = _options.TargetDir,
                Message = "Unnecessary comments have been removed"
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Removes single line comments and commits in '{_options.TargetDir}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
