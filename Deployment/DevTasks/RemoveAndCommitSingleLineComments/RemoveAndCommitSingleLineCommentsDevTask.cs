using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.RemoveSingleLineComments;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.RemoveAndCommitSingleLineComments
{
    public class RemoveAndCommitSingleLineCommentsDevTask : BaseDeploymentTask
    {
        private static RemoveAndCommitSingleLineCommentsOptions CreateDefaultOptions()
        {
            var result = new RemoveAndCommitSingleLineCommentsOptions();

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            result.TargetDirsList = new List<string>() { targetSolutions.First(p => p.Kind == KindOfProject.CoreSolution).Path };

            return result;
        }

        public RemoveAndCommitSingleLineCommentsDevTask()
            : this(null)
        {
        }

        public RemoveAndCommitSingleLineCommentsDevTask(IDeploymentTask parentTask)
            : this(CreateDefaultOptions(), parentTask)
        {
        }

        public RemoveAndCommitSingleLineCommentsDevTask(RemoveAndCommitSingleLineCommentsOptions options, IDeploymentTask parentTask)
            : base("350859E7-7A6D-4EC9-9E5B-A8C69E7ECBCE", false, options, parentTask)
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
            }, this));

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Unnecessary comments have been removed",
                RepositoryPaths = _options.TargetDirsList
            }, this));
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
