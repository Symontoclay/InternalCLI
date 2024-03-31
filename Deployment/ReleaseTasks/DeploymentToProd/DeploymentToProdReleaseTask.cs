using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.BuildExamples;
using Deployment.DevTasks.CoreToAsset;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.CreateAndCommitChangeLogs;
using Deployment.DevTasks.CreateAndCommitCodeOfConducts;
using Deployment.DevTasks.CreateAndCommitContributings;
using Deployment.DevTasks.CreateAndCommitLicenses;
using Deployment.DevTasks.CreateAndCommitReadmes;
using Deployment.DevTasks.UnityToSiteSource;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.ReleaseTasks.GitHubRelease;
using Deployment.ReleaseTasks.ProdSiteBuildAndCommit;
using System.Text;

namespace Deployment.ReleaseTasks.DeploymentToProd
{
    public class DeploymentToProdReleaseTask : BaseDeploymentTask
    {
        public DeploymentToProdReleaseTask()
            : this(null)
        {
        }

        public DeploymentToProdReleaseTask(IDeploymentTask parentTask)
            : base("0DF7B93E-A09E-4160-9F3A-217D83574C11", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            //Core
            Exec(new CoreToAssetDevTask(this));

            Exec(new CoreToSiteSourceDevTask(this));
            
            //Site
            Exec(new UnityToSiteSourceDevTask(this));

            Exec(new UpdateReleaseNotesDevTask(this));

            //Exec(new CoreToCLIFolderDevTask(this));
            Exec(new BuildExamplesDevTask(this));

            Exec(new ProdSiteBuildAndCommitReleaseTask(this));

            //Readmies
            Exec(new CreateAndCommitReadmesDevTask(this));

            //CHANGELOG.md
            Exec(new CreateAndCommitChangeLogsDevTask(this));

            //CODE_OF_CONDUCT.md
            Exec(new CreateAndCommitCodeOfConductsDevTask(this));

            //CONTRIBUTING.md
            Exec(new CreateAndCommitContributingsDevTask(this));

            //LICENSEs
            Exec(new CreateAndCommitLicensesDevTask(this));

            //Release to GitHub
            Exec(new GitHubReleaseReleaseTask(this));

            //Update unity examples repositories
            //Exec(new UpdateAndCommitUnityExampleRepositoriesDevTask(this));
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
