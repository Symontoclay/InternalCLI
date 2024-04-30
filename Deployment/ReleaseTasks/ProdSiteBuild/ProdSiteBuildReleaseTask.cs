using BaseDevPipeline;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.SiteTasks.SiteBuild;
using SiteBuilder;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.ReleaseTasks.ProdSiteBuild
{
    public class ProdSiteBuildReleaseTask : BaseDeploymentTask
    {
        public ProdSiteBuildReleaseTask()
            : this(null)
        {
        }

        public ProdSiteBuildReleaseTask(IDeploymentTask parentTask)
            : base("45F89561-9558-4A34-BDBC-C08F8C4BB850", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            using var tempDir = new TempDirectory();

            Exec(new SiteBuildTask(new SiteBuildTaskOptions()
            {
                KindOfTargetUrl = KindOfTargetUrl.Domain,
                SiteName = siteSolution.RepositoryName,
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                TempPath = tempDir.FullName
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds and commits READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
