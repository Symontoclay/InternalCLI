using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.DevTasks.TargetFrameworks.UpdateTargetFrameworkInAllCSharpProjects
{
    public class UpdateTargetFrameworkInAllCSharpProjectsDevTask : BaseDeploymentTask
    {
        public UpdateTargetFrameworkInAllCSharpProjectsDevTask(UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateTargetFrameworkInAllCSharpProjectsDevTask(UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions options, IDeploymentTask parentTask)
            : base("0BA492F3-CF6A-4F4C-A962-4966344B3082", false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateTargetFrameworkInAllCSharpProjectsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetVersion = new Version(_options.Version);

#if DEBUG
            //_logger.Info($"targetVersion = {targetVersion}");
#endif

            var frameworkVersion = (_options.KindOfTargetCSharpFramework, targetVersion);

            var cSharpSolutions = ProjectsDataSourceFactory.GetCSharpSolutions();

            foreach (var solution in cSharpSolutions)
            {
#if DEBUG
                //_logger.Info($"solution.Name = {solution.Name}");
#endif

                foreach (var project in solution.Projects)
                {
#if DEBUG
                    //_logger.Info($"project.FolderName = {project.FolderName}");
                    //_logger.Info($"project.CsProjPath = {project.CsProjPath}");
#endif

                    var currentFramework = CSharpProjectHelper.GetTargetFrameworkVersion(project.CsProjPath);

#if DEBUG
                    //_logger.Info($"currentFramework = {currentFramework}");
#endif

                    if (currentFramework.Kind != _options.KindOfTargetCSharpFramework)
                    {
                        continue;
                    }

                    if (currentFramework.Version >= targetVersion)
                    {
                        continue;
                    }

#if DEBUG
                    //_logger.Info("NEXT");
#endif

                    CSharpProjectHelper.SetTargetFramework(project.CsProjPath, frameworkVersion);
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates target framework's '{_options.KindOfTargetCSharpFramework}' version to '{_options.Version}' for all C# projects in organization.");
            sb.AppendLine($"{spaces}This only increases version without change target framework.");
            sb.AppendLine($"{spaces}If target framework has equal or higher version, It will be ignored.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
