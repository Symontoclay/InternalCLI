using BaseDevPipeline;
using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CopyAndBuildVSProjectOrSolution;
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
    public class CoreToAssetDevTask : OldBaseDeploymentTask
    {
        public CoreToAssetDevTask()
            : this(0u)
        {
        }

        public CoreToAssetDevTask(uint deep)
            : this(new CoreToAssetDevTaskOptions()
            {
                CoreCProjPath = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                DestDir = ProjectsDataSourceFactory.GetSolution(KindOfProject.Unity).SourcePath,
                Plugins = ProjectsDataSourceFactory.GetProjects(KindOfProject.CorePlugin).Select(p => p.CsProjPath).ToList()
            }, deep)
        {
        }

        public CoreToAssetDevTask(CoreToAssetDevTaskOptions options, uint deep)
            : base(options, deep)
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

            var deploymentPipeline = new OldDeploymentPipeline();

            deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = _options.CoreCProjPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, NextDeep));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = tempDir.FullName,
                DestDir = _options.DestDir,
                SaveSubDirs = false,
                OnlyFileExts = new List<string>() { "dll" },
                FileNameShouldContain = new List<string>() { "SymOntoClay." }
            }, NextDeep));

            if(!_options.Plugins.IsNullOrEmpty())
            {
                foreach(var pluginCProjPath in _options.Plugins)
                {
                    using var tempDir_2 = new TempDirectory();

                    deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions()
                    {
                        ProjectOrSoutionFileName = pluginCProjPath,
                        //BuildConfiguration = KindOfBuildConfiguration.Release,
                        OutputDir = tempDir_2.FullName,
                        NoLogo = true
                    }, NextDeep));

                    deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
                    {
                        SourceDir = tempDir_2.FullName,
                        DestDir = _options.DestDir,
                        SaveSubDirs = false,
                        OnlyFileExts = new List<string>() { "dll" },
                        FileNameShouldContain = new List<string>() { "SymOntoClay." }
                    }, NextDeep));
                }
            }

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
