using BaseDevPipeline;
using BaseDevPipeline.Data.Implementation;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndBuildVSProjectOrSolution;
using Deployment.DevTasks.CreateExtendedDocFile;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using SymOntoClay.Common.DebugHelpers;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CoreToSiteSource
{
    public class CoreToSiteSourceDevTask : BaseDeploymentTask
    {
        public CoreToSiteSourceDevTask()
            : this(null)
        {
        }

        public CoreToSiteSourceDevTask(IDeploymentTask parentTask)
            : this(new CoreToSiteSourceDevTaskOptions() 
            {
                CoreCProjPath = ProjectsDataSourceFactory.GetProject(KindOfProject.CoreAssetLib).CsProjPath,
                SiteSourceDir = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).SourcePath
            }, parentTask)
        {
        }

        public CoreToSiteSourceDevTask(CoreToSiteSourceDevTaskOptions options, IDeploymentTask parentTask)
            : base("CC1F3739-4159-4706-AC08-E7E7DACD7145", false, options, parentTask)
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

            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);
            var deploymentPipeline = new DeploymentPipeline(_context);

            deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = _options.CoreCProjPath,
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = tempDir.FullName,
                NoLogo = true
            }, this));

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = destDir,
                SkipExistingFilesInTargetDir = false
            }, this));

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
                }, this));
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
