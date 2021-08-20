using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.PullSolutionsWithMaintainedReleases
{
    public class PullSolutionsWithMaintainedReleasesDevTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

            foreach(var targetSolution in targetSolutions)
            {
                Exec(new PullTask(new PullTaskOptions()
                {
                    RepositoryPath = targetSolution.Path
                }));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Pulls commits of all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
