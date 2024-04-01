using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildChangeLog;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateChangeLogs
{
    public class CreateChangeLogsDevTask : OldBaseDeploymentTask
    {
        public CreateChangeLogsDevTask()
            : this(0u)
        {
        }

        public CreateChangeLogsDevTask(uint deep)
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
                var targetChangeLogFileName = string.Empty;

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        targetChangeLogFileName = Path.Combine(targetSolution.SourcePath, "CHANGELOG.md");
                        break;

                    default:
                        targetChangeLogFileName = Path.Combine(targetSolution.Path, "CHANGELOG.md");
                        break;
                }

                Exec(new BuildChangeLogTask(new BuildChangeLogTaskOptions()
                {
                    ReleaseNotesFilePath = CommonFileNamesHelper.BuildReleaseNotesPath(),
                    TargetChangeLogFileName = targetChangeLogFileName
                }, this));

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
                        {
                            DestDir = targetSolution.Path,
                            SaveSubDirs = false,
                            TargetFiles = new List<string>() { targetChangeLogFileName }
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

            sb.AppendLine($"{spaces}Builds CHANGELOGs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
