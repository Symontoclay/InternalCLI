using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildCodeOfConduct;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateCodeOfConducts
{
    public class CreateCodeOfConductsDevTask : OldBaseDeploymentTask
    {
        public CreateCodeOfConductsDevTask()
            : this(0u)
        {
        }

        public CreateCodeOfConductsDevTask(uint deep)
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
                        targetFileName = Path.Combine(targetSolution.SourcePath, "CODE_OF_CONDUCT.md");
                        break;

                    default:
                        targetFileName = Path.Combine(targetSolution.Path, "CODE_OF_CONDUCT.md");
                        break;
                }

                Exec(new BuildCodeOfConductTask(new BuildCodeOfConductTaskOptions()
                {
                    SiteSourcePath = siteSolution.SourcePath,
                    SiteDestPath = siteSolution.Path,
                    SiteName = siteSolution.RepositoryName,
                    SourceFileName = settings.CodeOfConductSource,
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

            sb.AppendLine($"{spaces}Builds CODE_OF_CONDUCTs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
