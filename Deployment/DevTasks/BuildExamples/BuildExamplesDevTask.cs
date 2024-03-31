using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.ExamplesCreator;
using SiteBuilder;
using System.Text;

namespace Deployment.DevTasks.BuildExamples
{
    public class BuildExamplesDevTask : BaseDeploymentTask
    {
        public BuildExamplesDevTask()
            : this(null)
        {
        }

        public BuildExamplesDevTask(IDeploymentTask parentTask)
            : base("7D323BB6-2957-4A01-B93B-75904A866E9B", true, null, parentTask)
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
            }, this));
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
