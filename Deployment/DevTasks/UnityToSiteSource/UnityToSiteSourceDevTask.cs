using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.ProjectsTasks.SetDocumentationFileInUnityProjectIfEmpty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UnityToSiteSource
{
    public class UnityToSiteSourceDevTask : BaseDeploymentTask
    {
        public UnityToSiteSourceDevTask()
            : this(0u)
        {
        }

        public UnityToSiteSourceDevTask(uint deep)
            : this(new UnityToSiteSourceDevTaskOptions() { 
                UnitySlnPath = ProjectsDataSource.GetSolution(KindOfProject.Unity).Path,
                SiteSourceDir = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).SourcePath
            }, deep)
        {
        }

        public UnityToSiteSourceDevTask(UnityToSiteSourceDevTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UnityToSiteSourceDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);

            ValidateFileName(nameof(_options.UnitySlnPath), _options.UnitySlnPath);
            ValidateDirectory(nameof(_options.SiteSourceDir), _options.SiteSourceDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var destDir = Path.Combine(_options.SiteSourceDir, "CSharpApiFiles");

            using var tempDir = new TempDirectory();
            var deploymentPipeline = new DeploymentPipeline();

            var unityCsProjectPath = Path.Combine(_options.UnitySlnPath, "Assembly-CSharp.csproj");
            
            deploymentPipeline.Add(new SetDocumentationFileInUnityProjectIfEmptyTask(
                new SetDocumentationFileInUnityProjectIfEmptyTaskOptions() {
                     ProjectFilePath = unityCsProjectPath
                }, NextDeep));

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
            {
                ProjectOrSoutionFileName = unityCsProjectPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, NextDeep));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = destDir,
                SkipExistingFilesInTargetDir = true
            }, NextDeep));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = tempDir.FullName,
                DestDir = destDir,
                SaveSubDirs = false,
                OnlyFileExts = new List<string>() { "dll", "xml" }//,
                //FileNameShouldContain = new List<string>() { "Assembly-CSharp" }
            }, NextDeep));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds Unity c# project '{_options.UnitySlnPath}' and copies core-dll and xml files to directory '{_options.SiteSourceDir}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
