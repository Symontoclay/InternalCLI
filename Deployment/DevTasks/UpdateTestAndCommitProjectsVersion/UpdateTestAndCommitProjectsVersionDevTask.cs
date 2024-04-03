using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.CopyAndTest;
using Deployment.DevTasks.UpdateProjectsVersion;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.UpdateTestAndCommitProjectsVersion
{
    public class UpdateTestAndCommitProjectsVersionDevTask : BaseDeploymentTask
    {
        public UpdateTestAndCommitProjectsVersionDevTask()
            : this(null)
        {
        }

        public UpdateTestAndCommitProjectsVersionDevTask(IDeploymentTask parentTask)
            : base("9BE131BA-3F46-4241-BC36-B0434256BDCA", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateProjectsVersionDevTask(this));

            var coreSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.CoreSolution);

            Exec(new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
            {
                ProjectOrSoutionFileName = coreSolution.SlnPath
            }, this));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Version has been updated",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));
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
