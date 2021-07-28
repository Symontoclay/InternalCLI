using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using NLog;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToSiteSource
{
    public class CoreToSiteSourceDevTask : BaseDeploymentTask
    {
        public CoreToSiteSourceDevTask()
            : this(new CoreToSiteSourceDevTaskOptions() 
            {
                CoreCProjPath = ProjectsDataSource.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                SiteSourceDir = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).SourcePath
            })
        {
        }

        public CoreToSiteSourceDevTask(CoreToSiteSourceDevTaskOptions options)
        {
            _options = options;
        }

        private readonly CoreToSiteSourceDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);

            ValidateFileName(nameof(_options.CoreCProjPath), _options.CoreCProjPath);
            ValidateDirectory(nameof(_options.SiteSourceDir), _options.SiteSourceDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var destDir = Path.Combine(_options.SiteSourceDir, "CSharpApiFiles");

            using var tempDir = new TempDirectory();
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
            {
                ProjectOrSoutionFileName = _options.CoreCProjPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = destDir,
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = tempDir.FullName,
                DestDir = destDir,
                SaveSubDirs = false,
                OnlyFileExts = new List<string>() { "dll", "xml" },
                FileNameShouldContain = new List<string>() { "SymOntoClay." }
            }));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds c# project '{_options.CoreCProjPath}' and copies core-dll and xml files to directory '{_options.SiteSourceDir}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
