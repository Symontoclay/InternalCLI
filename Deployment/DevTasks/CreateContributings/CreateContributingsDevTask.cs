using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildContributing;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateContributings
{
    public class CreateContributingsDevTask : BaseDeploymentTask
    {
        public CreateContributingsDevTask()
            : this(0u)
        {
        }

        public CreateContributingsDevTask(uint deep)
            : base(null, deep)
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
                }, NextDeep));

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
                        {
                            DestDir = targetSolution.Path,
                            SaveSubDirs = false,
                            TargetFiles = new List<string>() { targetFileName }
                        }, NextDeep));
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
