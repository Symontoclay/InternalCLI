using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Deployment.Tasks.DirectoriesTasks.CreateDirectory
{
    public class CreateDirectoryTask: BaseDeploymentTask
    {
        public CreateDirectoryTask(CreateDirectoryTaskOptions options)
            : this(options, null)
        {
        }

        public CreateDirectoryTask(CreateDirectoryTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.TargetDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateDirectoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.TargetDir), _options.TargetDir);
        }

        /// <inheritdoc/>
        [DebuggerHidden]
        protected override void OnRun()
        {
            var targetDir = _options.TargetDir;

            if (Directory.Exists(targetDir))
            {
                if (!_options.SkipExistingFilesInTargetDir)
                {
                    var n = 0;

                    while(true)
                    {
                        n++;

                        try
                        {
                            Directory.Delete(targetDir, true);

                            break;
                        }
                        catch(UnauthorizedAccessException e)
                        {
#if DEBUG
                            _logger.Info($"n = {n}");
                            _logger.Info($"e = {e}");
#endif
                        }

                        Thread.Sleep(5000);

                        if(n > 10)
                        {
                            break;
                        }
                    }
                    
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

            sb.AppendLine($"{spaces}Creates directory '{_options.TargetDir}' if It needs.");
            if(_options.SkipExistingFilesInTargetDir)
            {
                sb.AppendLine($"{spaces}Skips existing files in the directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Removes existing files in the directory.");
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
