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
            var indentation_1 = DisplayHelper.IndentationStep;
            var spaces_1 = DisplayHelper.Spaces(indentation_1);

            var sb = new StringBuilder();

            var targetVersion = new Version(_options.Version);

#if DEBUG
            //_logger.Info($"targetVersion = {targetVersion}");
#endif

            var frameworkVersion = (_options.KindOfTargetCSharpFramework, targetVersion);

#if DEBUG
            //_logger.Info($"frameworkVersion = {frameworkVersion}");
#endif

            if(_options.ReadOnlyMode)
            {
                sb.AppendLine("Just checking without real updating.");
                sb.AppendLine();
            }

            sb.AppendLine($"Target version: {frameworkVersion.KindOfTargetCSharpFramework} {frameworkVersion.targetVersion}");
            sb.AppendLine();

            var cSharpSolutions = ProjectsDataSourceFactory.GetCSharpSolutions();

            sb.AppendLine($"Solutions: {cSharpSolutions.Count}");
            sb.AppendLine();

            var updatedCount = 0;

            foreach (var solution in cSharpSolutions)
            {
#if DEBUG
                //_logger.Info($"solution.Name = {solution.Name}");
#endif

                sb.AppendLine($"Solution: {solution.Name}");

                foreach (var project in solution.Projects)
                {
#if DEBUG
                    //_logger.Info($"project.FolderName = {project.FolderName}");
                    //_logger.Info($"project.CsProjPath = {project.CsProjPath}");
#endif

                    sb.AppendLine($"{spaces_1}{project.FolderName}");
                    sb.AppendLine($"{spaces_1}Path: {project.CsProjPath}");

                    var currentFramework = CSharpProjectHelper.GetTargetFrameworkVersion(project.CsProjPath);

                    sb.AppendLine($"{spaces_1}Current version: {currentFramework.Kind} {currentFramework.Version}");

#if DEBUG
                    //_logger.Info($"currentFramework = {currentFramework}");
#endif

                    if (currentFramework.Kind != _options.KindOfTargetCSharpFramework)
                    {
                        sb.AppendLine($"{spaces_1}Not target framework. Skiped.");
                        sb.AppendLine();

                        continue;
                    }

                    if (currentFramework.Version >= targetVersion)
                    {
                        sb.AppendLine($"{spaces_1}Current version {currentFramework.Version} >= target version {targetVersion}. Skiped.");
                        sb.AppendLine();

                        continue;
                    }

#if DEBUG
                    //_logger.Info("NEXT");
#endif

                    if (_options.ReadOnlyMode)
                    {
                        sb.AppendLine($"{spaces_1}It would be updated with non readonly mode.");
                    }
                    else
                    {
                        sb.AppendLine($"{spaces_1}Updating.");
                        CSharpProjectHelper.SetTargetFramework(project.CsProjPath, frameworkVersion);
                        sb.AppendLine($"{spaces_1}Updated.");
                        updatedCount++;
                    }

                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            sb.AppendLine($"Updated: {updatedCount}");

            _logger.Info(sb);
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
