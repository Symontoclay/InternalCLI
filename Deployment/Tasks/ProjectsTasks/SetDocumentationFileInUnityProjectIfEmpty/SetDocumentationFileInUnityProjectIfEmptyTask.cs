using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.SetDocumentationFileInUnityProjectIfEmpty
{
    public class SetDocumentationFileInUnityProjectIfEmptyTask : BaseDeploymentTask
    {
        public SetDocumentationFileInUnityProjectIfEmptyTask(SetDocumentationFileInUnityProjectIfEmptyTaskOptions options)
            : this(options, null)
        {
        }

        public SetDocumentationFileInUnityProjectIfEmptyTask(SetDocumentationFileInUnityProjectIfEmptyTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.ProjectFilePath), false, options, parentTask)
        {
            _options = options;
        }

        private readonly SetDocumentationFileInUnityProjectIfEmptyTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectFilePath), _options.ProjectFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(_options.ProjectFilePath);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Add xml documentation file name into project '{_options.ProjectFilePath}' if It needs.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
