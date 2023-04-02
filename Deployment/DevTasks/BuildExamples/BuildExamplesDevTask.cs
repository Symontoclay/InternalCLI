using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.ExamplesCreator;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.BuildExamples
{
    public class BuildExamplesDevTask : BaseDeploymentTask
    {
        public BuildExamplesDevTask()
            : this(0u)
        {
        }

        public BuildExamplesDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var siteSolution = settings.GetSolution(KindOfProject.ProjectSite);

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var lngExamplesPagesList = siteSettings.SiteSettings.LngExamplesPages;

            Exec(new BuildExamplesTask(new BuildExamplesTaskOptions()
            {
                LngExamplesPages = lngExamplesPagesList,
                DestDir = siteSettings.SiteSettings.LngExamplesPath,
                SocExePath = settings.SocExePath,
                CacheDir = siteSettings.SiteSettings.LngExamplesCachePath
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds examples.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
