using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInFolder;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateCopyrightInFileHeaders
{
    public class UpdateCopyrightInFileHeadersDevTask : OldBaseDeploymentTask
    {
        public UpdateCopyrightInFileHeadersDevTask(uint deep)
            : base(null, deep)
        {
        }
        
        /// <inheritdoc/>
        protected override void OnRun()
        {
            var license = ProjectsDataSourceFactory.GetLicense("MIT");

            var siteSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite);

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var headerText = ContentPreprocessor.Run(license.HeaderContent, MarkdownStrategy.GenerateMarkdown, siteSettings);

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            foreach (var targetSolution in targetSolutions)
            {
                var kind = targetSolution.Kind;

                switch (kind)
                {
                    case KindOfProject.CoreSolution:
                        Exec(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(new UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions()
                        {
                            Text = headerText,
                            SourceDir = targetSolution.Path
                        }, NextDeep));
                        break;

                    case KindOfProject.Unity:
                        Exec(new UpdateCopyrightInFileHeadersInFolderTask(new UpdateCopyrightInFileHeadersInFolderTaskOptions()
                        {
                            Text = headerText,
                            SourceDir = targetSolution.SourcePath
                        }, NextDeep));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Builds and commits READMEs for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
