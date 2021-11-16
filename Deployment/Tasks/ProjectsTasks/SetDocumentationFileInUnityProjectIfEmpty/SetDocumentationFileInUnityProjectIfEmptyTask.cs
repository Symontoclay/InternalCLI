using CommonUtils.DebugHelpers;
using CSharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.SetDocumentationFileInUnityProjectIfEmpty
{
    public class SetDocumentationFileInUnityProjectIfEmptyTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public SetDocumentationFileInUnityProjectIfEmptyTask(SetDocumentationFileInUnityProjectIfEmptyTaskOptions options)
            : this(options, 0u)
        {
        }

        public SetDocumentationFileInUnityProjectIfEmptyTask(SetDocumentationFileInUnityProjectIfEmptyTaskOptions options, uint deep)
            : base(options, deep)
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
