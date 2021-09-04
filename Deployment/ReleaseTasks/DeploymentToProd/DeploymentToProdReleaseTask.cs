using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CoreToAsset;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.CreateAndCommitReadmes;
using Deployment.DevTasks.UnityToSiteSource;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.ReleaseTasks.GitHubRelease;
using Deployment.ReleaseTasks.ProdSiteBuildAndCommit;
using Deployment.Tasks;
using System;
using System.Text;

namespace Deployment.ReleaseTasks.DeploymentToProd
{
    public class DeploymentToProdReleaseTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            //Core
            Exec(new CoreToAssetDevTask());

            Exec(new CoreToSiteSourceDevTask());
            
            //Site
            Exec(new UnityToSiteSourceDevTask());

            Exec(new UpdateReleaseNotesDevTask());

            Exec(new ProdSiteBuildAndCommitReleaseTask());

            //Readmies
            Exec(new CreateAndCommitReadmesDevTask());

            //Release to GitHub
            Exec(new GitHubReleaseReleaseTask());
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
