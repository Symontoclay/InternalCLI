using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.SiteTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateReleaseNotesTask(UpdateReleaseNotesTaskOptions options)
        {
            _options = options;
        }

        private readonly UpdateReleaseNotesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.FutureReleaseFilePath), _options.FutureReleaseFilePath);
            ValidateFileName(nameof(_options.ReleaseNotesFilePath), _options.ReleaseNotesFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Exports directory '{_options.SourceDir}' of '{_options.RootDir}' to Unity package '{_options.OutputPackageName}'");
            //sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
