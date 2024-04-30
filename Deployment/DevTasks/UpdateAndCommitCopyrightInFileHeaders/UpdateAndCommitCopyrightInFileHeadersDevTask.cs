using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.UpdateCopyrightInFileHeaders;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders
{
    public class UpdateAndCommitCopyrightInFileHeadersDevTask : BaseDeploymentTask
    {
        public UpdateAndCommitCopyrightInFileHeadersDevTask()
            : this(null)
        {
        }

        public UpdateAndCommitCopyrightInFileHeadersDevTask(IDeploymentTask parentTask)
            : base("1C627CC1-F693-460E-AF83-30E264D18B4A", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateCopyrightInFileHeadersDevTask(this));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "File headers has been updated",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, this));
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
