using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.ProjectsTasks.GenerateUnityCSProjAndSolution;
using Deployment.Tasks.ProjectsTasks.SetDocumentationFileInUnityProjectIfEmpty;
using System.IO;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.PrepareUnityCSProjAndSolution
{
    public class PrepareUnityCSProjAndSolutionTask : BaseDeploymentTask
    {
        public PrepareUnityCSProjAndSolutionTask(PrepareUnityCSProjAndSolutionTaskOptions options)
            : this(options, null)
        {
        }

        public PrepareUnityCSProjAndSolutionTask(PrepareUnityCSProjAndSolutionTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RootDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly PrepareUnityCSProjAndSolutionTaskOptions _options;

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
            DeleteExistingSolutions();

            Exec(new GenerateUnityCSProjAndSolutionTask(new GenerateUnityCSProjAndSolutionTaskOptions() {
                UnityExeFilePath = _options.UnityExeFilePath,
                RootDir = _options.RootDir
            }, this));

            var unityCsProjectPath = Path.Combine(_options.RootDir, "Assembly-CSharp.csproj");

            Exec(new SetDocumentationFileInUnityProjectIfEmptyTask(
                new SetDocumentationFileInUnityProjectIfEmptyTaskOptions()
                {
                    ProjectFilePath = unityCsProjectPath
                }, this));
        }

        private void DeleteExistingSolutions()
        {
            var files = Directory.GetFiles(_options.RootDir);

            foreach (var file in files)
            {
                if (file.EndsWith(".sln"))
                {
                    File.Delete(file);

                    continue;
                }

                if (file.EndsWith(".csproj"))
                {
                    File.Delete(file);

                    continue;
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Prepares .csproj and .sln of '{_options.RootDir}'.");
            sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
