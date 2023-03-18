using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.RemoveSingleLineComments;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Commit;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
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

            result.TargetDirsList = new List<string>() { targetSolutions.First(p => p.Kind == KindOfProject.CoreSolution).Path };

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
            ValidateList(nameof(_options.TargetDirsList), _options.TargetDirsList);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new RemoveSingleLineCommentsDevTask(new RemoveSingleLineCommentsDevTaskOptions()
            {
                TargetDirsList = _options.TargetDirsList,
            }));

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Unnecessary comments have been removed",
                RepositoryPaths = _options.TargetDirsList
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = DisplayHelper.Spaces(nextN);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Removes single line comments and commits in:");
            foreach (var item in _options.TargetDirsList)
            {
                sb.AppendLine($"{nextNSpaces}{item}");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
