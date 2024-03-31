using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateProjectCopyright
{
    public class UpdateProjectCopyrightTask : BaseDeploymentTask
    {
        public UpdateProjectCopyrightTask(UpdateProjectCopyrightTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.ProjectFilePath), false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateProjectCopyrightTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectFilePath), _options.ProjectFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CSharpProjectHelper.SetCopyright(_options.ProjectFilePath, _options.Copyright);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in project '{_options.ProjectFilePath}'.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Copyright);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
