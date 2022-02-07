using CommonUtils.DebugHelpers;
using Deployment.Tasks.UnityTasks.ExecuteMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.GenerateUnityCSProjAndSolution
{
    public class GenerateUnityCSProjAndSolutionTask : BaseDeploymentTask
    {
        public GenerateUnityCSProjAndSolutionTask(GenerateUnityCSProjAndSolutionTaskOptions options)
            : this(options, 0u)
        {
        }

        public GenerateUnityCSProjAndSolutionTask(GenerateUnityCSProjAndSolutionTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly GenerateUnityCSProjAndSolutionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.UnityExeFilePath), _options.UnityExeFilePath);
            ValidateDirectory(nameof(_options.RootDir), _options.RootDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new ExecuteMethodTask(new ExecuteMethodTaskOptions() {
                UnityExeFilePath = _options.UnityExeFilePath,
                RootDir = _options.RootDir,
                MethodName = "SymOntoClay.UnityAsset.Editors.EmptyScript.Run"
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Generates .csproj and .sln of '{_options.RootDir}'.");
            sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
