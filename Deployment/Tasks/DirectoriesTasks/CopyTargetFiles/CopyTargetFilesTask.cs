using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopyTargetFiles
{
    public class CopyTargetFilesTask : BaseDeploymentTask
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public CopyTargetFilesTask(CopyTargetFilesTaskOptions options)
        {
            _options = options;
        }

        private readonly CopyTargetFilesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory("DestDir", _options.DestDir);
            ValidateList("TargetFiles", _options.TargetFiles);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Begin");

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            sb.Append($"{spaces}Copies target files to directory {_options.DestDir}.");
            if (_options.SaveSubDirs)
            {
                sb.Append(" Saves subdirectories' structure.");
            }
            else
            {
                sb.Append(" All fles will be put to dest directory without saving subdirectories' structure.");
            }
            if (!_options.TargetFiles.IsNullOrEmpty())
            {
                sb.AppendLine();
                sb.AppendLine($"{spaces}The target copied files:");
                foreach (var targetFile in _options.TargetFiles)
                {
                    sb.AppendLine($"{nextSpaces}{targetFile}");
                }
            }
            sb.AppendLine();
            sb.Append(PrintValidation(n));
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
