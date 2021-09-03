using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateAndCommitProjectsVersion;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.DevFullMaintaining
{
    public class DevFullMaintainingDevTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "snapshot",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }));

            Exec(new UpdateAndCommitProjectsVersionDevTask());

            //

            throw new NotImplementedException();
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
