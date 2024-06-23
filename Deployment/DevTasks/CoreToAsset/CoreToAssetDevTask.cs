using BaseDevPipeline;
using BaseDevPipeline.Data;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndBuildVSProjectOrSolution;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CoreToAsset
{
    public class CoreToAssetDevTask : BaseDeploymentTask
    {
        public CoreToAssetDevTask()
            : this(null)
        {
        }

        public CoreToAssetDevTask(IDeploymentTask parentTask)
            : this(new CoreToAssetDevTaskOptions()
            {
                CoreCProjPath = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                DestDir = ProjectsDataSourceFactory.GetSolution(KindOfProject.Unity).SourcePath,
                Plugins = ProjectsDataSourceFactory.GetProjects(KindOfProject.CorePlugin).Select(p => p.CsProjPath).ToList(),
                CommonPackages = ProjectsDataSourceFactory.GetCSharpSolutions().Where(p => p.Kind == KindOfProject.CommonPackagesSolution).SelectMany(p => p.Projects.Where(p => p.Kind == KindOfProject.Library)).Select(p => p.CsProjPath).ToList()
            }, parentTask)
        {
        }

        public CoreToAssetDevTask(CoreToAssetDevTaskOptions options, IDeploymentTask parentTask)
            : base("3E0E19A6-2BB8-47EE-85D4-09C9F6937B45", false, options, parentTask)
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
            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);

            var deploymentPipeline = new DeploymentPipeline(_context);

            ProcessProject(tempSettings, deploymentPipeline, _options.CoreCProjPath);

            if(!_options.Plugins.IsNullOrEmpty())
            {
                foreach(var pluginCProjPath in _options.Plugins)
                {
                    ProcessProject(tempSettings, deploymentPipeline, pluginCProjPath);
                }
            }

            if(!_options.CommonPackages.IsNullOrEmpty())
            {
                foreach (var commonPackageCProjPath in _options.CommonPackages)
                {
                    ProcessProject(tempSettings, deploymentPipeline, commonPackageCProjPath);
                }
            }

            deploymentPipeline.Run();
        }

        private void ProcessProject(ITempSettings tempSettings, DeploymentPipeline deploymentPipeline, string csProjectPath)
        {
            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);

            deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = csProjectPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, this));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = tempDir.FullName,
                DestDir = _options.DestDir,
                SaveSubDirs = false,
                OnlyFileExts = new List<string>() { "dll" },
                FileNameShouldContain = new List<string>() { "SymOntoClay." }
            }, this));
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
