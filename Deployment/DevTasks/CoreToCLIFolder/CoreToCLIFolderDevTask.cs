using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndPublishVSProjectOrSolution;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.CoreToCLIFolder
{
    public class CoreToCLIFolderDevTask : BaseDeploymentTask
    {
        public CoreToCLIFolderDevTask()
            : this(null)
        {
        }

        public CoreToCLIFolderDevTask(IDeploymentTask parentTask)
            : base("9AED3948-9616-46D7-A19A-DB12EDE46FAA", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var cliProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CLI);

            var cliFolderTargetPath = ProjectsDataSourceFactory.GetDevArtifact(KindOfArtifact.CLIFolder).Path;

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = cliFolderTargetPath,
                SkipExistingFilesInTargetDir = false
            }, this));

            Exec(new CopyAndPublishVSProjectOrSolutionDevTask(new CopyAndPublishVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliFolderTargetPath,
                NoLogo = true,
                SelfContained = false
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Exports CLI to dev destanation.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
