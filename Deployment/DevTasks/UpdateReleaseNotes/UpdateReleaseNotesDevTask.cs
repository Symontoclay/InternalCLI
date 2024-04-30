using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.SiteTasks.UpdateReleaseNotes;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesDevTask: BaseDeploymentTask
    {
        public UpdateReleaseNotesDevTask()
            : this(null)
        {
        }

        public UpdateReleaseNotesDevTask(IDeploymentTask parentTask)
            : this(new UpdateReleaseNotesDevTaskOptions() 
            {
                ReleaseMngrRepositoryPath = ProjectsDataSourceFactory.GetSolution(KindOfProject.ReleaseMngrSolution).Path,
                ArtifactsForDeployment = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).ArtifactsForDeployment.ToList(),
                ReleaseNotesFilePath = CommonFileNamesHelper.BuildReleaseNotesPath(),
                BaseHref = ProjectsDataSourceFactory.GetSolution(KindOfProject.CoreSolution).Href
            }, parentTask)
        {
        }

        public UpdateReleaseNotesDevTask(UpdateReleaseNotesDevTaskOptions options, IDeploymentTask parentTask)
            : base("D50BA290-8F96-4310-82DB-30190559F253", false, options, parentTask)
        {
            _options = options;
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
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + DisplayHelper.IndentationStep;
            var nextSpaces = DisplayHelper.Spaces(next_N);
            var sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
