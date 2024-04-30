using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders
{
    public class UpdateCopyrightInFileHeadersTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateCopyrightInFileHeadersTask(UpdateCopyrightInFileHeadersTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateCopyrightInFileHeadersTask(UpdateCopyrightInFileHeadersTaskOptions options, IDeploymentTask parentTask)
            : base("4EFBFEB5-D0EF-4C38-B035-8B9D5E202B32", false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateCopyrightInFileHeadersTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateList(nameof(_options.TargetFiles), _options.TargetFiles);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var licenceText = _options.Text;

            if (!licenceText.StartsWith("/*"))
            {
                licenceText = $"/*{licenceText}*/";
            }

            foreach (var fileName in _options.TargetFiles)
            {
                CSharpFileHelper.AddCopyrightHeader(fileName, licenceText);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + DisplayHelper.IndentationStep;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in files' headers.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Text);
            if (!_options.TargetFiles.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}The target copied files:");
                foreach (var targetFile in _options.TargetFiles)
                {
                    sb.AppendLine($"{nextSpaces}{targetFile}");
                }
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
