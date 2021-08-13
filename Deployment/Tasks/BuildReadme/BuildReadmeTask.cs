using CommonUtils.DebugHelpers;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildReadme
{
    public class BuildReadmeTask : BaseDeploymentTask
    {
        public BuildReadmeTask(BuildReadmeTaskOptions options)
        {
            _options = options;
        }

        private readonly BuildReadmeTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SiteSourcePath), _options.SiteSourcePath);
            ValidateDirectory(nameof(_options.SiteDestPath), _options.SiteDestPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.SiteName), _options.SiteName);
            ValidateFileName(nameof(_options.TargetReadmeFileName), _options.TargetReadmeFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = _options.SiteSourcePath,
                DestPath = _options.SiteDestPath,
                SiteName = _options.SiteName,
            });

            var sb = new StringBuilder();

            var wasBadges = false;

            if (!string.IsNullOrWhiteSpace(_options.CommonBadgesFileName))
            {
                var content = File.ReadAllText(_options.CommonBadgesFileName);

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.Append(content);

                    wasBadges = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(_options.RepositorySpecificBadgesFileName))
            {
                var content = File.ReadAllText(_options.RepositorySpecificBadgesFileName);

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.Append(content);

                    wasBadges = true;
                }
            }

            if (wasBadges)
            {
                sb.AppendLine();
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(_options.RepositorySpecificReadmeFileName))
            {
                var content = File.ReadAllText(_options.RepositorySpecificReadmeFileName);

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.AppendLine(content);
                    sb.AppendLine();
                }
            }

            if (!string.IsNullOrWhiteSpace(_options.CommonReadmeFileName))
            {
                var content = File.ReadAllText(_options.CommonReadmeFileName);

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.AppendLine(content);
                    sb.AppendLine();
                }
            }

            File.WriteAllText(_options.TargetReadmeFileName, sb.ToString());
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds README '{_options.TargetReadmeFileName}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
