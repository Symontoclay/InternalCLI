using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.BuildExamples;
using Deployment.DevTasks.CoreToCLIFolder;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.DevSiteBuild;
using Deployment.DevTasks.UnityToSiteSource;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.Helpers;
using System;

namespace Deployment.DevTasks.DevSiteFullBuild
{
    public class DevSiteFullBuildTask : BaseDeploymentTask
    {
        public DevSiteFullBuildTask()
            : this(null)
        {
        }
        
        public DevSiteFullBuildTask(IDeploymentTask parentTask)
            : base("E5A7E089-3913-4A26-A43F-45A0034A2C2F", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CheckMasterBranch(KindOfProject.ProjectSite);
            CheckMasterBranch(KindOfProject.CoreSolution);
            CheckMasterBranch(KindOfProject.Unity);

            Exec(new UpdateReleaseNotesDevTask(this));
            Exec(new CoreToSiteSourceDevTask(this));
            Exec(new UnityToSiteSourceDevTask(this));
            Exec(new CoreToCLIFolderDevTask(this));
            Exec(new BuildExamplesDevTask(this));
            Exec(new DevSiteBuildTask(this));
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
