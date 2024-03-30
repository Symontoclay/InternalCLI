using CommonUtils.DebugHelpers;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildLicense
{
    public class BuildLicenseTask : OldBaseDeploymentTask
    {
        public BuildLicenseTask(BuildLicenseTaskOptions options)
            : this(options, 0u)
        {
        }

        public BuildLicenseTask(BuildLicenseTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly BuildLicenseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SiteSourcePath), _options.SiteSourcePath);
            ValidateDirectory(nameof(_options.SiteDestPath), _options.SiteDestPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.SiteName), _options.SiteName);
            ValidateFileName(nameof(_options.TargetFileName), _options.TargetFileName);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Content), _options.Content);            
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

            File.WriteAllText(_options.TargetFileName, ContentPreprocessor.Run(_options.Content, MarkdownStrategy.GenerateMarkdown, siteSettings));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds LICENSE '{_options.TargetFileName}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
