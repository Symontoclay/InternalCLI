using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndPublishVSProjectOrSolution;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using System.Text;

namespace Deployment.DevTasks.CoreToInternalCLIDist
{
    public class CoreToInternalCLIDistDevTask : BaseDeploymentTask
    {
        public CoreToInternalCLIDistDevTask()
            : this(null)
        {
        }

        public CoreToInternalCLIDistDevTask(IDeploymentTask parentTask)
            : base("FDFE4391-E96E-4346-839F-BD5627417CEF", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var cliProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CLI);

            var cliFolderTargetPath = settings.InternalCLIDist;

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = cliFolderTargetPath,
                SkipExistingFilesInTargetDir = false
            }, this));

            Exec(new CopyAndPublishVSProjectOrSolutionDevTask(new CopyAndPublishVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliFolderTargetPath,
                NoLogo = true
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Exports CLI to internal CLI destanation.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
