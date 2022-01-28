using CommonUtils;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Test;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CopyAndTest
{
    public class CopyAndTestDevTask : BaseDeploymentTask
    {
        public CopyAndTestDevTask(CopyAndTestDevTaskOptions options)
            : this(options, 0u)
        {
        }

        public CopyAndTestDevTask(CopyAndTestDevTaskOptions options, uint deep)
             : base(options, deep)
        {
            _options = options;
        }

        private readonly CopyAndTestDevTaskOptions _options;

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

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions()
            {
                SourceDir = slnFolder,
                DestDir = tempDir.FullName
            }, NextDeep));

            deploymentPipeline.Add(new TestTask(new TestTaskOptions()
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

            sb.AppendLine($"{spaces}Tests project '{_options.ProjectOrSoutionFileName}' in temp directory with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Tested files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Tested files will be put into '{_options.OutputDir}'.");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
