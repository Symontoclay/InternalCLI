using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CopyAndPublishVSProjectOrSolution;
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
    public class CoreToCLIFolderDevTask : OldBaseDeploymentTask
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
            var cliProject = ProjectsDataSourceFactory.GetProject(KindOfProject.CLI);

            var cliFolderTargetPath = ProjectsDataSourceFactory.GetDevArtifact(KindOfArtifact.CLIFolder).Path;

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = cliFolderTargetPath,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            Exec(new CopyAndPublishVSProjectOrSolutionDevTask(new CopyAndPublishVSProjectOrSolutionDevTaskOptions()
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
