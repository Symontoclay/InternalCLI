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
using System.Collections.Generic;
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
            Exec(new DeploymentTasksGroup("A940C01A-3C9E-4821-983B-08C9C9AE0A57", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CoreToAssetDevTask(this)
                }
            });

            Exec(new DeploymentTasksGroup("6F99275C-8CB4-41BE-94F5-584E2ACB12C8", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CoreToSiteSourceDevTask(this)
                }
            });

            //Site
            Exec(new DeploymentTasksGroup("F946FFFE-9057-4616-820C-1E3AAAA67E8F", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new UnityToSiteSourceDevTask(this)
                }
            });

            Exec(new DeploymentTasksGroup("7A61B088-E454-48C9-89F4-8E689326B1C9", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new UpdateReleaseNotesDevTask(this)
                }
            });

            //Exec(new CoreToCLIFolderDevTask(this));

            Exec(new DeploymentTasksGroup("DFFB459B-97C9-4F65-9430-5A5B3393C590", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new BuildExamplesDevTask(this)
                }
            });

            Exec(new DeploymentTasksGroup("2DAB7232-B135-4481-AF38-6BFA526A0ABA", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new ProdSiteBuildAndCommitReleaseTask(this)
                }
            });

            //Readmies
            Exec(new DeploymentTasksGroup("F0646126-9772-48E9-B981-A528BCF62FAB", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CreateAndCommitReadmesDevTask(this)
                }
            });

            //CHANGELOG.md
            Exec(new DeploymentTasksGroup("38084343-F3EC-41A9-A5C9-C7AA6D33B10A", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CreateAndCommitChangeLogsDevTask(this)
                }
            });

            //CODE_OF_CONDUCT.md
            Exec(new DeploymentTasksGroup("874683E8-4209-4377-82EC-9F201860FF0D", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CreateAndCommitCodeOfConductsDevTask(this)
                }
            });

            //CONTRIBUTING.md
            Exec(new DeploymentTasksGroup("17061854-C0A4-4C44-8AD2-2A3B3B95AC10", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CreateAndCommitContributingsDevTask(this)
                }
            });

            //LICENSEs
            Exec(new DeploymentTasksGroup("CFA99218-AE8B-4645-B870-1D7445E65E73", true, this)
            {
                SubItems = new List<IDeploymentTask>()
                {
                    new CreateAndCommitLicensesDevTask(this)
                }
            });

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
