using CommonUtils.DebugHelpers;
using Deployment.DevTasks.BuildExamples;
using Deployment.DevTasks.CoreToAsset;
using Deployment.DevTasks.CoreToCLIFolder;
using Deployment.DevTasks.CoreToSiteSource;
using Deployment.DevTasks.CreateAndCommitChangeLogs;
using Deployment.DevTasks.CreateAndCommitCodeOfConducts;
using Deployment.DevTasks.CreateAndCommitContributings;
using Deployment.DevTasks.CreateAndCommitLicenses;
using Deployment.DevTasks.CreateAndCommitReadmes;
using Deployment.DevTasks.UnityToSiteSource;
using Deployment.DevTasks.UpdateAndCommitUnityExampleRepositories;
using Deployment.DevTasks.UpdateReleaseNotes;
using Deployment.ReleaseTasks.GitHubRelease;
using Deployment.ReleaseTasks.ProdSiteBuildAndCommit;
using Deployment.Tasks;
using System;
using System.Text;

namespace Deployment.ReleaseTasks.DeploymentToProd
{
    public class DeploymentToProdReleaseTask : OldBaseDeploymentTask
    {
        public DeploymentToProdReleaseTask()
            : this(0u)
        {
        }

        public DeploymentToProdReleaseTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            //Core
            Exec(new CoreToAssetDevTask(NextDeep));

            Exec(new CoreToSiteSourceDevTask(NextDeep));
            
            //Site
            Exec(new UnityToSiteSourceDevTask(NextDeep));

            Exec(new UpdateReleaseNotesDevTask(NextDeep));

            //Exec(new CoreToCLIFolderDevTask(NextDeep));
            Exec(new BuildExamplesDevTask(NextDeep));

            Exec(new ProdSiteBuildAndCommitReleaseTask(NextDeep));

            //Readmies
            Exec(new CreateAndCommitReadmesDevTask(NextDeep));

            //CHANGELOG.md
            Exec(new CreateAndCommitChangeLogsDevTask(NextDeep));

            //CODE_OF_CONDUCT.md
            Exec(new CreateAndCommitCodeOfConductsDevTask(NextDeep));

            //CONTRIBUTING.md
            Exec(new CreateAndCommitContributingsDevTask(NextDeep));

            //LICENSEs
            Exec(new CreateAndCommitLicensesDevTask(NextDeep));

            //Release to GitHub
            Exec(new GitHubReleaseReleaseTask(NextDeep));

            //Update unity examples repositories
            //Exec(new UpdateAndCommitUnityExampleRepositoriesDevTask(NextDeep));
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
