using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using dotless.Core.Parser.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CopyAndBuildVSProjectOrSolution
{
    public class CopyAndBuildVSProjectOrSolutionDevTask : OldBaseDeploymentTask
    {
        public CopyAndBuildVSProjectOrSolutionDevTask(CopyAndBuildVSProjectOrSolutionDevTaskOptions options)
            : this(options, 0u)
        {
        }

        public CopyAndBuildVSProjectOrSolutionDevTask(CopyAndBuildVSProjectOrSolutionDevTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly CopyAndBuildVSProjectOrSolutionDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectOrSoutionFileName), _options.ProjectOrSoutionFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            using var tempDir = new TempDirectory();

            var slnFolder = PathsHelper.GetSlnFolder(_options.ProjectOrSoutionFileName);

            var tempProjectOrSoutionFileName = PathsHelper.Normalize(_options.ProjectOrSoutionFileName).Replace(PathsHelper.Normalize(slnFolder), PathsHelper.Normalize(tempDir.FullName));

            var deploymentPipeline = new DeploymentPipeline(_context);

            deploymentPipeline.Add(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions()
            {
                SourceDir = slnFolder,
                DestDir = tempDir.FullName
            }, NextDeep));

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
            {
                ProjectOrSoutionFileName = tempProjectOrSoutionFileName,
                BuildConfiguration = _options.BuildConfiguration,
                OutputDir = _options.OutputDir,
                NoLogo = _options.NoLogo,
                RuntimeIdentifier = _options.RuntimeIdentifier
            }, NextDeep));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Buils project '{_options.ProjectOrSoutionFileName}' in temp directory with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Built files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Built files will be put into '{_options.OutputDir}'.");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
