using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.BuildTasks.Publish;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using System.Text;

namespace Deployment.DevTasks.CopyAndPublishVSProjectOrSolution
{
    public class CopyAndPublishVSProjectOrSolutionDevTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public CopyAndPublishVSProjectOrSolutionDevTask(CopyAndPublishVSProjectOrSolutionDevTaskOptions options)
            : this(options, null)
        {
        }

        public CopyAndPublishVSProjectOrSolutionDevTask(CopyAndPublishVSProjectOrSolutionDevTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.ProjectOrSoutionFileName), false, options, parentTask)
        {
            _options = options;
        }

        private readonly CopyAndPublishVSProjectOrSolutionDevTaskOptions _options;

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

#if DEBUG
            //_logger.Info($"tempDir.FullName = {tempDir.FullName}");
#endif

            var slnFolder = PathsHelper.GetSlnFolder(_options.ProjectOrSoutionFileName);

#if DEBUG
            //_logger.Info($"slnFolder = {slnFolder}");
#endif

            var tempProjectOrSoutionFileName = PathsHelper.Normalize(_options.ProjectOrSoutionFileName).Replace(PathsHelper.Normalize(slnFolder), PathsHelper.Normalize(tempDir.FullName));

#if DEBUG
            //_logger.Info($"tempProjectOrSoutionFileName = {tempProjectOrSoutionFileName}");
#endif

            var deploymentPipeline = new DeploymentPipeline(_context);

            deploymentPipeline.Add(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions()
            {
                SourceDir = slnFolder,
                DestDir = tempDir.FullName
            }, NextDeep));

            deploymentPipeline.Add(new PublishTask(new PublishTaskOptions()
            {
                ProjectOrSoutionFileName = tempProjectOrSoutionFileName,
                BuildConfiguration = _options.BuildConfiguration,
                OutputDir = _options.OutputDir,
                NoLogo = _options.NoLogo,
                RuntimeIdentifier = _options.RuntimeIdentifier,
                SelfContained = _options.SelfContained
            }, NextDeep));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Publish project '{_options.ProjectOrSoutionFileName}' in temp directory with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Published files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Published files will be put into '{_options.OutputDir}'.");
            }
            sb.AppendLine($"{spaces}With --self-contained {_options.SelfContained.ToString().ToLower()}");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
