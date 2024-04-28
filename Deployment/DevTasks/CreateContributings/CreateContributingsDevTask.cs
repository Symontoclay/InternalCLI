using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.BuildContributing;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.CreateContributings
{
    public class CreateContributingsDevTask : BaseDeploymentTask
    {
        public CreateContributingsDevTask()
            : this(null)
        {
        }

        public CreateContributingsDevTask(IDeploymentTask parentTask)
            : base("55272410-E9DD-47B1-A950-53708F316AF8", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var siteSolution = settings.GetSolution(KindOfProject.ProjectSite);

            var targetSolutions = settings.GetSolutionsWithMaintainedReleases();

            foreach (var targetSolution in targetSolutions)
            {
                var targetFileName = string.Empty;

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        targetFileName = Path.Combine(targetSolution.SourcePath, "CONTRIBUTING.md");
                        break;

                    default:
                        targetFileName = Path.Combine(targetSolution.Path, "CONTRIBUTING.md");
                        break;
                }

                Exec(new BuildContributingTask(new BuildContributingTaskOptions()
                {
                    SiteSourcePath = siteSolution.SourcePath,
                    SiteDestPath = siteSolution.Path,
                    SiteName = siteSolution.RepositoryName,
                    SourceFileName = settings.ContributingSource,
                    TargetFileName = targetFileName
                }, this));

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
                        {
                            DestDir = targetSolution.Path,
                            SaveSubDirs = false,
                            TargetFiles = new List<string>() { targetFileName }
                        }, this));
                        break;
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds CONTRIBUTINGs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
