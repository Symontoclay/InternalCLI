using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.SetDocumentationFileIfEmpty
{
    public class SetDocumentationFileIfEmptyTask : BaseDeploymentTask
    {
        public SetDocumentationFileIfEmptyTask(SetDocumentationFileIfEmptyTaskOptions options)
            : this(options, null)
        {
        }

        public SetDocumentationFileIfEmptyTask(SetDocumentationFileIfEmptyTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.ProjectFilePath), false, options, parentTask)
        {
            _options = options;
        }

        private readonly SetDocumentationFileIfEmptyTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectFilePath), _options.ProjectFilePath);
            ValidateFileName(nameof(_options.DocumentationFilePath), _options.DocumentationFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CSharpProjectHelper.SetDocumentationFileIfEmpty(_options.ProjectFilePath, _options.DocumentationFilePath);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Add xml documentation file name '{_options.DocumentationFilePath}' into project '{_options.ProjectFilePath}' if It needs.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
