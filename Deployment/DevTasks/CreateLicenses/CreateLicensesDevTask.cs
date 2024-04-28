﻿using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.BuildLicense;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.CreateLicenses
{
    public class CreateLicensesDevTask : BaseDeploymentTask
    {
        public CreateLicensesDevTask()
            : this(null)
        {
        }

        public CreateLicensesDevTask(IDeploymentTask parentTask)
            : base("E2081DDF-2F5A-4EB7-B841-D9F867F22DC5", false, null, parentTask)
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

            sb.AppendLine($"{spaces}Builds LICENSEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
