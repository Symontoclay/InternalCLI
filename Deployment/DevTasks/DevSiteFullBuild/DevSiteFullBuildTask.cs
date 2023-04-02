using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.BuildExamples;
using Deployment.DevTasks.CoreToCLIFolder;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.DevSiteBuild;
using Deployment.DevTasks.UnityToSiteSource;
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
        public DevSiteFullBuildTask()
            : this(0u)
        {
        }
        
        public DevSiteFullBuildTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CheckMasterBranch(KindOfProject.ProjectSite);
            CheckMasterBranch(KindOfProject.CoreSolution);
            CheckMasterBranch(KindOfProject.Unity);

            Exec(new UpdateReleaseNotesDevTask(NextDeep));
            Exec(new CoreToSiteSourceDevTask(NextDeep));
            Exec(new UnityToSiteSourceDevTask(NextDeep));
            Exec(new CoreToCLIFolderDevTask(NextDeep));
            Exec(new BuildExamplesDevTask(NextDeep));
            Exec(new DevSiteBuildTask(NextDeep));
        }

        private void CheckMasterBranch(KindOfProject kind)
        {
            var solution = ProjectsDataSourceFactory.GetSolution(kind);

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
