using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.ReleaseTasks.GitHubRelease
{
    public class GitHubReleaseReleaseTask : BaseDeploymentTask
    {//GitHubReleaseTask
        public GitHubReleaseReleaseTask()
            : this(new GitHubReleaseReleaseTaskOptions() { 
                Repositories = ProjectsDataSource.GetSolutionsWithMaintainedReleases().Select(p => p.Path).ToList()
            })
        {
        }

        public GitHubReleaseReleaseTask(GitHubReleaseReleaseTaskOptions options)
        {
            _options = options;
        }

        private readonly GitHubReleaseReleaseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);

            ValidateList(nameof(_options.Repositories), _options.Repositories);
            _options.Repositories.ForEach(item => ValidateFileName(nameof(item), item));
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
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
