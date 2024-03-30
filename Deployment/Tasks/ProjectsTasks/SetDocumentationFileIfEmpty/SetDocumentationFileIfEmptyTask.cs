using CommonUtils.DebugHelpers;
using CSharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.SetDocumentationFileIfEmpty
{
    public class SetDocumentationFileIfEmptyTask : OldBaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public SetDocumentationFileIfEmptyTask(SetDocumentationFileIfEmptyTaskOptions options)
            : this(options, 0u)
        {
        }

        public SetDocumentationFileIfEmptyTask(SetDocumentationFileIfEmptyTaskOptions options, uint deep)
            : base(options, deep)
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
