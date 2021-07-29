using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.DevSiteBuild;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.Helpers;
using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.DevSiteFullBuild
{
    public class DevSiteFullBuildTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            CheckMasterBranch(KindOfProject.ProjectSite);
            CheckMasterBranch(KindOfProject.CoreSolution);
            CheckMasterBranch(KindOfProject.Unity);

            Exec(new UpdateReleaseNotesDevTask());
            Exec(new CoreToSiteSourceDevTask());
            Exec(new DevSiteBuildTask());
        }

        private void CheckMasterBranch(KindOfProject kind)
        {
            var solution = ProjectsDataSource.GetSolution(kind);

            if (GitRepositoryHelper.IsCurrentBranchMaster(solution.Path))
            {
                throw new Exception($"The repository '{solution.Path}' is in master branch. This task can not be run in master branch!");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            return string.Empty;
        }
    }
}
