using CommonUtils.DebugHelpers;
using CSharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateProjectCopyright
{
    public class UpdateProjectCopyrightTask : OldBaseDeploymentTask
    {
        public UpdateProjectCopyrightTask(UpdateProjectCopyrightTaskOptions options, uint deep)
            : base(options, deep)
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
