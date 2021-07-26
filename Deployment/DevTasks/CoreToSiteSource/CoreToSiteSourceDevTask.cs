using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Build;
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
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

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
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif

            var generalSiteBuilderSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions() { 
                SourcePath = _options.SiteSourceDir
            });



//            using (var tempDir = new TempDirectory())
//            {
//#if DEBUG
//                _logger.Info($"tempDir.FullName = {tempDir.FullName}");
//#endif

//                var deploymentPipeline = new DeploymentPipeline();

//                _logger.Info($"deploymentPipeline = {deploymentPipeline}");

//                deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
//                {
//                    TargetDir = "a",
//                    SkipExistingFilesInTargetDir = false
//                }));

//                deploymentPipeline.Add(new BuildTask(new BuildTaskOptions()
//                {
//                    ProjectOrSoutionFileName = _options.CoreCProjPath,
//                    //BuildConfiguration = KindOfBuildConfiguration.Release,
//                    OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
//                    NoLogo = true
//                }));

//#if DEBUG
//                _logger.Info($"deploymentPipeline = {deploymentPipeline}");
//#endif

//                deploymentPipeline.Run();
//            }
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
