using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using NLog;
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
            : this(options, 0u)
        {
        }

        public BuildReadmeTask(BuildReadmeTaskOptions options, uint deep)
            : base(options, deep)
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
            ValidateFileName(nameof(_options.CommonReadmeFileName),_options.CommonReadmeFileName);
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

            var content = File.ReadAllText(_options.CommonReadmeFileName);

            content = GeneralReadmeContentPreprocessor.Run(content, _options.CommonBadgesFileName, _options.RepositorySpecificBadgesFileName, _options.RepositorySpecificReadmeFileName);

            content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

            content = HrefsNormalizer.FillAppDomainNameInHrefs(content, siteSettings);

            File.WriteAllText(_options.TargetReadmeFileName, content);
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
