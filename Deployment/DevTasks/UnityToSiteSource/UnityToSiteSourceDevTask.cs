using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using Deployment.DevTasks.CreateExtendedDocFile;
using Deployment.Tasks.BuildTasks.Build;
using Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.ProjectsTasks.PrepareUnityCSProjAndSolution;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.UnityToSiteSource
{
    public class UnityToSiteSourceDevTask : BaseDeploymentTask
    {
        private static UnityToSiteSourceDevTaskOptions CreateDefaultOptions()
        {
            var result = new UnityToSiteSourceDevTaskOptions();

            var unitySolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.Unity);

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();
            result.UnitySlnPath = unitySolution.Path;
            result.SiteSourceDir = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).SourcePath;

            var unityExeInstance = settings.GetUtityExeInstance(unitySolution);

            result.UnityExeFilePath = unityExeInstance.Path;
            result.UnityEnginePath = unityExeInstance.UnityEnginePath;

            return result;
        }

        public UnityToSiteSourceDevTask()
            : this(null)
        {
        }

        public UnityToSiteSourceDevTask(IDeploymentTask parentTask)
            : this(CreateDefaultOptions(), parentTask)
        {
        }

        public UnityToSiteSourceDevTask(UnityToSiteSourceDevTaskOptions options, IDeploymentTask parentTask)
            : base("AAD7F02F-114D-49C2-AEB7-5E8412B043A3", false, options, parentTask)
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
            ValidateFileName(nameof(_options.UnityExeFilePath), _options.UnityExeFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var destDir = Path.Combine(_options.SiteSourceDir, "CSharpApiFiles");

            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);
            var deploymentPipeline = new DeploymentPipeline(_context);

            deploymentPipeline.Add(new PrepareUnityCSProjAndSolutionTask(new PrepareUnityCSProjAndSolutionTaskOptions()
            {
                UnityExeFilePath = _options.UnityExeFilePath,
                RootDir = _options.UnitySlnPath
            }, this));

            var unityCsProjectPath = Path.Combine(_options.UnitySlnPath, "Assembly-CSharp.csproj");

            deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
            {
                ProjectOrSoutionFileName = unityCsProjectPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, this));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = _options.UnitySlnPath,
                DestDir = tempDir.FullName,
                SaveSubDirs = false,
                ExistingFileStrategy = ExistingFileStrategy.Skip,
                OnlyFileExts = new List<string>() { "dll" }
            }, this));

            deploymentPipeline.Add(new CopyAllFromDirectoryTask(new CopyAllFromDirectoryTaskOptions()
            {
                SourceDir = _options.UnityEnginePath,
                DestDir = tempDir.FullName,
                SaveSubDirs = false,
                ExistingFileStrategy = ExistingFileStrategy.Skip,
                OnlyFileExts = new List<string>() { "dll" }
            }, this));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = destDir,
                SkipExistingFilesInTargetDir = true
            }, this));

            deploymentPipeline.Run();

            var xmlFileName = Path.Combine(tempDir.FullName, "Assembly-CSharp.xml");

            var baseFileName = Path.GetFileName(xmlFileName);

            var destFileName = Path.Combine(destDir, baseFileName.Replace(".xml", ".json"));

            Exec(new CreateExtendedDocFileDevTask(new CreateExtendedDocFileDevTaskOptions()
            {
                XmlDocFile = xmlFileName,
                ExtendedDocFile = destFileName
            }, this));
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
