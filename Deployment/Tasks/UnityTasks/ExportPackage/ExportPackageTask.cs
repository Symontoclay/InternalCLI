using CommonUtils;
using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.UnityTasks.ExportPackage
{
    public class ExportPackageTask : OldBaseDeploymentTask
    {
        public ExportPackageTask(ExportPackageTaskOptions options)
            : this(options, 0u)
        {
        }

        public ExportPackageTask(ExportPackageTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly ExportPackageTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.UnityExeFilePath), _options.UnityExeFilePath);
            ValidateFileName(nameof(_options.OutputPackageName), _options.OutputPackageName);
            ValidateDirectory(nameof(_options.RootDir), _options.RootDir);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var commandLine = $"-quit -batchmode -projectPath \"{_options.RootDir.Replace("\\", "/")}\" -exportPackage \"{_options.SourceDir.Replace("\\", "/")}\" \"{_options.OutputPackageName.Replace("\\", "/")}\"";

            var execPath = $"\"{_options.UnityExeFilePath.Replace("\\", "/")}\"";

            var processWrapper = new ProcessSyncWrapper(execPath, commandLine);

            var exitCode = processWrapper.Run();

            if (exitCode != 0)
            {
                throw new Exception($"Export of {_options.SourceDir} has been failed. | {string.Join(' ', processWrapper.Output)} | {string.Join(' ', processWrapper.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Exports directory '{_options.SourceDir}' of '{_options.RootDir}' to Unity package '{_options.OutputPackageName}'");
            sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
