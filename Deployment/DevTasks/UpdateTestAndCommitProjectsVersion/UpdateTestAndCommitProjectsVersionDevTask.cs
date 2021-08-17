using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateProjectsVersion;
using Deployment.Tasks;
using Deployment.Tasks.BuildTasks.Test;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateTestAndCommitProjectsVersion
{
    public class UpdateTestAndCommitProjectsVersionDevTask : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateProjectsVersionDevTask());

            var coreSolution = ProjectsDataSource.GetSolution(KindOfProject.CoreSolution);

            Exec(new TestTask(new TestTaskOptions()
            {
                ProjectOrSoutionFileName = coreSolution.SlnPath
            }));

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

            sb.AppendLine($"{spaces}Updates, test and commits Version and Copyright for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
