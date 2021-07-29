using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.SiteTasks.UpdateReleaseNotes;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesDevTask: BaseDeploymentTask
    {
        public UpdateReleaseNotesDevTask()
            : this(new UpdateReleaseNotesDevTaskOptions() 
            {
                ReleaseMngrRepositoryPath = ProjectsDataSource.GetSolution(KindOfProject.ReleaseMngrSolution).Path,
                ArtifactsForDeployment = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).ArtifactsForDeployment.ToList(),
                ReleaseNotesFilePath = BuildReleaseNotesPath(ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).SourcePath),
                BaseHref = ProjectsDataSource.GetSolution(KindOfProject.CoreSolution).Href
            })
        {
        }

        public UpdateReleaseNotesDevTask(UpdateReleaseNotesDevTaskOptions options)
        {
            _options = options;
        }

        private static string BuildReleaseNotesPath(string basePath)
        {
            return Path.Combine(basePath, "ReleaseNotes.json");
        }

        private readonly UpdateReleaseNotesDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.ReleaseMngrRepositoryPath), _options.ReleaseMngrRepositoryPath);
            ValidateList(nameof(_options.ArtifactsForDeployment), _options.ArtifactsForDeployment);
            ValidateFileName(nameof(_options.ReleaseNotesFilePath), _options.ReleaseNotesFilePath);
            ValidateValueAsNonNull(nameof(_options.BaseHref), _options.BaseHref);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var futureReleaseInfo = FutureReleaseInfoReader.Read(_options.ReleaseMngrRepositoryPath);

            Exec(new UpdateReleaseNotesTask(new UpdateReleaseNotesTaskOptions()
            {
                FutureReleaseInfo = futureReleaseInfo,
                ArtifactsForDeployment = _options.ArtifactsForDeployment,
                ReleaseNotesFilePath = _options.ReleaseNotesFilePath,
                BaseHref = _options.BaseHref
            }));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
            var nextSpaces = DisplayHelper.Spaces(next_N);
            var sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
