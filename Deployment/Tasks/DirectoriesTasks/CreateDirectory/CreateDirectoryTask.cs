using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.DirectoriesTasks.DeleteDirectory;
using SymOntoClay.Common.DebugHelpers;
using System.Diagnostics;
using System.IO;
using System.Text;

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
                    Exec(new DeleteDirectoryTask(new DeleteDirectoryTaskOptions()
                    {
                        TargetDir = targetDir
                    }, this));

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
