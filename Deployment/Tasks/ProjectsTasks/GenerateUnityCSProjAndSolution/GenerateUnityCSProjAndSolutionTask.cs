using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.UnityTasks.ExecuteMethod;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.GenerateUnityCSProjAndSolution
{
    public class GenerateUnityCSProjAndSolutionTask : BaseDeploymentTask
    {
        public GenerateUnityCSProjAndSolutionTask(GenerateUnityCSProjAndSolutionTaskOptions options)
            : this(options, null)
        {
        }

        public GenerateUnityCSProjAndSolutionTask(GenerateUnityCSProjAndSolutionTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RootDir), false, options, parentTask)
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
            }, this));
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
