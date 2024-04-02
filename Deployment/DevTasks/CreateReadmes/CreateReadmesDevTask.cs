using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.BuildReadme;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.CreateReadmes
{
    public class CreateReadmesDevTask : BaseDeploymentTask
    {
        public CreateReadmesDevTask()
            : this(null)
        {
        }

        public CreateReadmesDevTask(IDeploymentTask parentTask)
            : base("653DAA56-CC48-4C2D-95E2-8CFC23B9C498", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

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
                }, this));

                switch(targetSolution.Kind)
                {
                    case KindOfProject.ProjectSite:
                    case KindOfProject.Unity:
                        Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
                        {
                            DestDir = targetSolution.Path,
                            SaveSubDirs = false,
                            TargetFiles = new List<string>() { targetReadmeFileName }
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

            sb.AppendLine($"{spaces}Builds READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
