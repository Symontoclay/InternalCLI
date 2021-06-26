using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CreateDirectory
{
    public class CreateDirectoryTask: BaseDeploymentTask
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public CreateDirectoryTask(CreateDirectoryTaskOptions options)
        {
            _options = options;
        }

        private readonly CreateDirectoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            if (string.IsNullOrWhiteSpace(_options?.TargetDir))
            {
                AddValidationMessage("The TargetDir can not be null or empty!");
            }
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetDir = _options.TargetDir;

            if (Directory.Exists(targetDir))
            {
                if (!_options.SkipExistingFilesInTargetDir)
                {
                    Directory.Delete(targetDir, true);
                    Directory.CreateDirectory(targetDir);
                }
            }
            else
            {
                Directory.CreateDirectory(targetDir);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.Append($"{spaces}Creates directory '{_options.TargetDir}' if It needs.");
            if(_options.SkipExistingFilesInTargetDir)
            {
                sb.Append(" Skips existing files in the directory.");
            }
            else
            {
                sb.Append(" Removes existing files in the directory.");
            }
            sb.AppendLine();
            sb.Append(PrintValidation(n));
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
