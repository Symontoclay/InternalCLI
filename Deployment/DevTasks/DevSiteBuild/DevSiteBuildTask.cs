using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.SiteTasks.SiteBuild;
using SiteBuilder;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.DevSiteBuild
{
    public class DevSiteBuildTask: BaseDeploymentTask
    {
        private static DevSiteBuildTaskOptions CreateDefaultOptions()
        {
            var solution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            return new DevSiteBuildTaskOptions()
            {
                SiteName = solution.RepositoryName,
                SourcePath = solution.SourcePath,
                DestPath = ProjectsDataSourceFactory.GetDevArtifact(KindOfArtifact.ProjectSite).Path,
                BrowserPath = settings.BrowserPath,
                FontPath = settings.FontPath
            };
        }

        public DevSiteBuildTask()
            : this(null)
        {
        }

        public DevSiteBuildTask(IDeploymentTask parentTask)
            : this(CreateDefaultOptions(), parentTask)
        {
        }
        
        public DevSiteBuildTask(DevSiteBuildTaskOptions options, IDeploymentTask parentTask)
            : base("FF122FDF-E11E-41E3-A14D-628A6F9523FD", false, options, parentTask)
        {
            _options = options;
        }

        private readonly DevSiteBuildTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.SiteName), _options.SiteName);
            ValidateDirectory(nameof(_options.SourcePath), _options.SourcePath);
            ValidateDirectory(nameof(_options.DestPath), _options.DestPath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);
            var deploymentPipeline = new DeploymentPipeline(_context);

#if DEBUG
            //_logger.Info($"_options.DestPath = {_options.DestPath}");
            //_logger.Info($"_options.SiteName = {_options.SiteName}");
            //_logger.Info($"_options.SourcePath = {_options.SourcePath}");
            //_logger.Info($"tempDir.FullName = {tempDir.FullName}");
            //_logger.Info($"_options.BrowserPath = {_options.BrowserPath}");
            //_logger.Info($"_options.FontPath = {_options.FontPath}");
#endif

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = _options.DestPath,
                SkipExistingFilesInTargetDir = false
            }, this));

            deploymentPipeline.Add(new SiteBuildTask(new SiteBuildTaskOptions()
            {
                KindOfTargetUrl = KindOfTargetUrl.Path,
                SiteName = _options.SiteName,
                SourcePath = _options.SourcePath,
                DestPath = _options.DestPath,
                TempPath = tempDir.FullName,
                BrowserPath = _options.BrowserPath,
                FontPath = _options.FontPath
            }, this));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds site '{_options.SiteName}' from '{_options.SourcePath}'.");
            sb.AppendLine($"{spaces}The built site will be put into '{_options.DestPath}'.");
            sb.AppendLine($"{spaces}Uses '{KindOfTargetUrl.Path}' as target url's strategy.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
