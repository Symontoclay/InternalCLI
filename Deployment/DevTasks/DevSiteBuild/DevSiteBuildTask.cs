using BaseDevPipeline;
using CommonUtils.DebugHelpers;
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
    public class DevSiteBuildTask: BaseDeploymentTask
    {
        public DevSiteBuildTask()
            : this(new DevSiteBuildTaskOptions() 
            { 
                SiteName = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).RepositoryName,
                SourcePath = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).SourcePath,
                DestPath = ProjectsDataSource.GetDevArtifact(KindOfArtifact.ProjectSite).Path
            })
        {
        }

        public DevSiteBuildTask(DevSiteBuildTaskOptions options)
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
            }));

            deploymentPipeline.Add(new SiteBuildTask(new SiteBuildTaskOptions()
            {
                KindOfTargetUrl = KindOfTargetUrl.Path,
                SiteName = _options.SiteName,
                SourcePath = _options.SourcePath,
                DestPath = _options.DestPath,
                TempPath = tempDir.FullName
            }));

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
