using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.BuildReadme;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateReadmes
{
    public class CreateReadmesDevTask : BaseDeploymentTask
    {
        public CreateReadmesDevTask()
            : this(0u)
        {
        }

        public CreateReadmesDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var siteSolution = settings.GetSolution(KindOfProject.ProjectSite);

            var targetSolutions = settings.GetSolutionsWithMaintainedReleases();

            foreach(var targetSolution in targetSolutions)
            {
                var targetReadmeFileName = string.Empty;

                switch(targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        targetReadmeFileName = Path.Combine(targetSolution.SourcePath, "README.md");
                        break;

                    default:
                        targetReadmeFileName = Path.Combine(targetSolution.Path, "README.md");
                        break;
                }

                Exec(new BuildReadmeTask(new BuildReadmeTaskOptions()
                {
                    SiteSourcePath = siteSolution.SourcePath,
                    SiteDestPath = siteSolution.Path,
                    SiteName = siteSolution.RepositoryName,
                    CommonBadgesFileName = settings.CommonBadgesSource,
                    CommonReadmeFileName = settings.CommonReadmeSource,
                    RepositorySpecificBadgesFileName = targetSolution.BadgesSource,
                    RepositorySpecificReadmeFileName = targetSolution.ReadmeSource,
                    TargetReadmeFileName = targetReadmeFileName
                }, NextDeep));

                switch(targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
                        {
                            DestDir = targetSolution.Path,
                            SaveSubDirs = false,
                            TargetFiles = new List<string>() { targetReadmeFileName }
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

            sb.AppendLine($"{spaces}Builds READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
