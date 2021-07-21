using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using CSharpUtils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeaders
{
    public class UpdateCopyrightInFileHeadersTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateCopyrightInFileHeadersTask(UpdateCopyrightInFileHeadersTaskOptions options)
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
            var next_N = n + 4;
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
