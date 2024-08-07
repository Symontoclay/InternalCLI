﻿using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.UnityTasks.ExecuteMethod
{
    public class ExecuteMethodTask : BaseDeploymentTask
    {
        public ExecuteMethodTask(ExecuteMethodTaskOptions options)
            : this(options, null)
        {
        }

        public ExecuteMethodTask(ExecuteMethodTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RootDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly ExecuteMethodTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.UnityExeFilePath), _options.UnityExeFilePath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.MethodName), _options.MethodName);
            ValidateDirectory(nameof(_options.RootDir), _options.RootDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var execPath = $"\"{_options.UnityExeFilePath.Replace("\\", "/")}\"";

            var commandLine = $"-quit -batchmode -projectPath \"{_options.RootDir.Replace("\\", "/")}\" -executeMethod {_options.MethodName}";

            var processWrapper = new ProcessSyncWrapper(execPath, commandLine);

            var exitCode = processWrapper.Run();

            if (exitCode != 0)
            {
                //throw new Exception($"Executing method '{_options.MethodName}' has been failed. | {string.Join(' ', processWrapper.Output)} | {string.Join(' ', processWrapper.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Executes method '{_options.MethodName}' of '{_options.RootDir}'.");
            sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
