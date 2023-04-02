using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.SiteTasks.SiteBuild;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.ProdSiteBuild
{
    public class ProdSiteBuildReleaseTask : BaseDeploymentTask
    {
        public ProdSiteBuildReleaseTask()
            : this(0u)
        {
        }

        public ProdSiteBuildReleaseTask(uint deep)
            : base(null, deep)
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
            }, NextDeep));
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
