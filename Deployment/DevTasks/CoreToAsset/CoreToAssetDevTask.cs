using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToAsset
{
    public class CoreToAssetDevTask : BaseDeploymentTask
    {
        public CoreToAssetDevTask()
            : this(new CoreToAssetDevTaskOptions() 
            {
                CoreCProjPath = ProjectsDataSource.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                DestDir = ProjectsDataSource.GetSolution(KindOfProject.Unity).SourcePath
            })
        {
        }

        public CoreToAssetDevTask(CoreToAssetDevTaskOptions options)
        {
            _options = options;
        }

        private readonly CoreToAssetDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.CoreCProjPath), _options.CoreCProjPath);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            using var tempDir = new TempDirectory();

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
            {
                ProjectOrSoutionFileName = _options.CoreCProjPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = tempDir.FullName,
                DestDir = _options.DestDir,
                SaveSubDirs = false,
                OnlyFileExts = new List<string>() { "dll" },
                FileNameShouldContain = new List<string>() { "SymOntoClay." }
            }));

            deploymentPipeline.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds c# project '{_options.CoreCProjPath}' and copies core-dll files to directory '{_options.DestDir}'");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
