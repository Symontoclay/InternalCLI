using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateProjectsVersion;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateAndCommitProjectsVersion
{
    public class UpdateAndCommitProjectsVersionDevTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateProjectsVersionDevTask());

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Version has been updated",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates and commits Version and Copyright for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
