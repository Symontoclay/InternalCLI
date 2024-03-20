using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.RemoveAndCommitSingleLineComments;
using Deployment.DevTasks.UpdateAndCommitCopyrightInFileHeaders;
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
        public DevFullMaintainingDevTask()
            : this(0u)
        {
        }

        public DevFullMaintainingDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "snapshot",
                RepositoryPaths = targetSolutions.Select(p => p.Path).ToList()
            }, NextDeep));

            Exec(new UpdateAndCommitProjectsVersionDevTask(NextDeep));

            Exec(new UpdateAndCommitCopyrightInFileHeadersDevTask(NextDeep));

            //Exec(new RemoveAndCommitSingleLineCommentsDevTask(NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds and commits versions and headers for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
