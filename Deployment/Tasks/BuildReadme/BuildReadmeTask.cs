using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Html2Markdown;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using System.IO;
using System.Text;

namespace Deployment.Tasks.BuildReadme
{
    public class BuildReadmeTask : BaseDeploymentTask
    {
        public BuildReadmeTask(BuildReadmeTaskOptions options)
            : this(options, null)
        {
        }

        public BuildReadmeTask(BuildReadmeTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.SiteSourcePath), false, options, parentTask)
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

#if DEBUG
            //_logger.Info($"content (1) = {content}");
#endif

            content = GeneralReadmeContentPreprocessor.Run(content, _options.CommonBadgesFileName, _options.RepositorySpecificBadgesFileName, _options.RepositorySpecificReadmeFileName);

#if DEBUG
            //_logger.Info($"content (2) = {content}");
#endif

            content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

#if DEBUG
            //_logger.Info($"content (3) = {content}");
#endif

            content = HrefsNormalizer.FillAppDomainNameInHrefs(content, siteSettings);

            var converter = new Converter();
            content = converter.Convert(content);

#if DEBUG
            //_logger.Info($"content (4) = {content}");
#endif

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
