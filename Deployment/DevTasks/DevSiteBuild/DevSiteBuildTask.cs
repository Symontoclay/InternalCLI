using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.SiteTasks.SiteBuild;
using NLog;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.DevSiteBuild
{
    public class DevSiteBuildTask: OldBaseDeploymentTask
    {
        public DevSiteBuildTask()
            : this(0u)
        {
        }

        public DevSiteBuildTask(uint deep)
            : this(new DevSiteBuildTaskOptions() 
            { 
                SiteName = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).RepositoryName,
                SourcePath = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).SourcePath,
                DestPath = ProjectsDataSourceFactory.GetDevArtifact(KindOfArtifact.ProjectSite).Path
            }, deep)
        {
        }

        public DevSiteBuildTask(DevSiteBuildTaskOptions options, uint deep)
            : base(options, deep)
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
            using var tempDir = new TempDirectory();
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = _options.DestPath,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            deploymentPipeline.Add(new SiteBuildTask(new SiteBuildTaskOptions()
            {
                KindOfTargetUrl = KindOfTargetUrl.Path,
                SiteName = _options.SiteName,
                SourcePath = _options.SourcePath,
                DestPath = _options.DestPath,
                TempPath = tempDir.FullName
            }, NextDeep));

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
