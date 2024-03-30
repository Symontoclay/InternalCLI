using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndBuildVSProjectOrSolution;
using Deployment.DevTasks.CreateExtendedDocFile;
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
    public class CoreToSiteSourceDevTask : OldBaseDeploymentTask
    {
        public CoreToSiteSourceDevTask()
            : this(0u)
        {
        }

        public CoreToSiteSourceDevTask(uint deep)
            : this(new CoreToSiteSourceDevTaskOptions() 
            {
                CoreCProjPath = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                SiteSourceDir = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).SourcePath
            }, deep)
        {
        }

        public CoreToSiteSourceDevTask(CoreToSiteSourceDevTaskOptions options, uint deep)
            : base(options, deep)
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

            deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = _options.CoreCProjPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, NextDeep));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = destDir,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            deploymentPipeline.Run();

            var xmlFileNamesList = Directory.GetFiles(tempDir.FullName).Where(p => p.EndsWith(".xml")/* && p.Contains("SymOntoClay.")*/);

            foreach (var xmlFileName in xmlFileNamesList)
            {
                var baseFileName = Path.GetFileName(xmlFileName);

                var destFileName = Path.Combine(destDir, baseFileName.Replace(".xml", ".json"));

                Exec(new CreateExtendedDocFileDevTask(new CreateExtendedDocFileDevTaskOptions()
                {
                    XmlDocFile = xmlFileName,
                    ExtendedDocFile = destFileName
                }));
            }
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
