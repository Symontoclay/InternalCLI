using BaseDevPipeline;
using BaseDevPipeline.Data.Implementation;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.BuildTasks.Test;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.CopyAndTest
{
    public class CopyAndTestDevTask : BaseDeploymentTask
    {
        public CopyAndTestDevTask(CopyAndTestDevTaskOptions options)
            : this(options, null)
        {
        }

        public CopyAndTestDevTask(CopyAndTestDevTaskOptions options, IDeploymentTask parentTask)
             : base(MD5Helper.GetHash(options.ProjectOrSoutionFileName), false, options, parentTask)
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
            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);

            var slnFolder = PathsHelper.GetSlnFolder(_options.ProjectOrSoutionFileName);

            var tempProjectOrSoutionFileName = PathsHelper.Normalize(_options.ProjectOrSoutionFileName).Replace(PathsHelper.Normalize(slnFolder), PathsHelper.Normalize(tempDir.FullName));

            var deploymentPipeline = new DeploymentPipeline(_context);

            deploymentPipeline.Add(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions()
            {
                SourceDir = slnFolder,
                DestDir = tempDir.FullName
            }, this));

            deploymentPipeline.Add(new TestTask(new TestTaskOptions()
            {
                ProjectOrSoutionFileName = tempProjectOrSoutionFileName,
                BuildConfiguration = _options.BuildConfiguration,
                OutputDir = _options.OutputDir,
                NoLogo = _options.NoLogo,
                RuntimeIdentifier = _options.RuntimeIdentifier
            }, this));

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
