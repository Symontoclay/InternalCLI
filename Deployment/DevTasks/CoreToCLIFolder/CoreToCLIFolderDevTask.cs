using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Publish;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToCLIFolder
{
    public class CoreToCLIFolderDevTask : BaseDeploymentTask
    {
        public CoreToCLIFolderDevTask()
            : this(0u)
        {
        }

        public CoreToCLIFolderDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var cliProject = ProjectsDataSource.GetProject(KindOfProject.CLI);

            var cliFolderTargetPath = ProjectsDataSource.GetDevArtifact(KindOfArtifact.CLIFolder).Path;

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = cliFolderTargetPath,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            Exec(new PublishTask(new PublishTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliFolderTargetPath,
                NoLogo = true
            }, NextDeep));
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
