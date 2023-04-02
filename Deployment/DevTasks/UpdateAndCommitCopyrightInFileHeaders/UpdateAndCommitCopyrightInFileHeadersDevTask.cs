using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateCopyrightInFileHeaders;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders
{
    public class UpdateAndCommitCopyrightInFileHeadersDevTask : BaseDeploymentTask
    {
        public UpdateAndCommitCopyrightInFileHeadersDevTask()
            : this(0u)
        {
        }

        public UpdateAndCommitCopyrightInFileHeadersDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new UpdateCopyrightInFileHeadersDevTask(NextDeep));

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "File headers has been updated",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, NextDeep));
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
