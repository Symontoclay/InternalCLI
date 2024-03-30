using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildLicense;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateLicenses
{
    public class CreateLicensesDevTask : OldBaseDeploymentTask
    {
        public CreateLicensesDevTask()
            : this(0u)
        {
        }

        public CreateLicensesDevTask(uint deep)
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
                if(targetSolution.License == null || string.IsNullOrWhiteSpace(targetSolution.License.Content))
                {
                    continue;
                }

                var targetFileName = string.Empty;

                switch (targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        targetFileName = Path.Combine(targetSolution.SourcePath, "LICENSE");
                        break;

                    default:
                        targetFileName = Path.Combine(targetSolution.Path, "LICENSE");
                        break;
                }

                Exec(new BuildLicenseTask(new BuildLicenseTaskOptions()
                {
                    SiteSourcePath = siteSolution.SourcePath,
                    SiteDestPath = siteSolution.Path,
                    SiteName = siteSolution.RepositoryName,
                    Content = targetSolution.License.Content,
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

            sb.AppendLine($"{spaces}Builds LICENSEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
