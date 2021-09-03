using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeadersInFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateCopyrightInFileHeaders
{
    public class UpdateCopyrightInFileHeadersDevTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            var license = ProjectsDataSource.GetLicense("MIT");

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            foreach (var targetSolution in targetSolutions)
            {
                var kind = targetSolution.Kind;

                switch (kind)
                {
                    case KindOfProject.CoreSolution:
                        Exec(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions()
                        {
                            Text = license.HeaderContent,
                            SourceDir = targetSolution.Path
                        }));
                        break;

                    case KindOfProject.Unity:
                        Exec(new UpdateCopyrightInFileHeadersInFolderTask(new UpdateCopyrightInFileHeadersInFolderTaskOptions()
                        {
                            Text = license.HeaderContent,
                            SourceDir = targetSolution.SourcePath
                        }));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
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
