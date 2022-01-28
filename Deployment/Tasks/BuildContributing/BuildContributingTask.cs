using CommonUtils.DebugHelpers;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildContributing
{
    public class BuildContributingTask : BaseDeploymentTask
    {
        public BuildContributingTask(BuildContributingTaskOptions options)
            : this(options, 0u)
        {
        }

        public BuildContributingTask(BuildContributingTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly BuildContributingTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SiteSourcePath), _options.SiteSourcePath);
            ValidateDirectory(nameof(_options.SiteDestPath), _options.SiteDestPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.SiteName), _options.SiteName);
            ValidateFileName(nameof(_options.SourceFileName), _options.SourceFileName);
            ValidateFileName(nameof(_options.TargetFileName), _options.TargetFileName);
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

            var content = File.ReadAllText(_options.SourceFileName);

            content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

            File.WriteAllText(_options.TargetFileName, content);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds CONTRIBUTING.md '{_options.TargetFileName}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
