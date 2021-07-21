using CommonUtils.DebugHelpers;
using NLog;
using SiteBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.SiteTasks.SiteBuild
{
    public class SiteBuildTask: BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public SiteBuildTask(SiteBuildTaskOptions options)
        {
            _options = options;
        }

        private readonly SiteBuildTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.SiteName), _options.SiteName);
            ValidateDirectory(nameof(_options.SourcePath), _options.SourcePath);
            ValidateDirectory(nameof(_options.DestPath), _options.DestPath);
            ValidateDirectory(nameof(_options.TempPath), _options.TempPath);            
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            //_logger.Info($"_options = {_options}");
#endif

            var options = new SiteBuilderOptions();

            options.KindOfTargetUrl = _options.KindOfTargetUrl;

            options.SiteName = _options.SiteName;

            options.SourcePath = _options.SourcePath;

            options.DestPath = _options.DestPath;

            options.TempPath = _options.TempPath;

            var siteBuilder = new SiteBuilder.SiteBuilder(options);
            siteBuilder.Run();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds site '{_options.SiteName}' from '{_options.SourcePath}' using temp directory '{_options.TempPath}'.");
            sb.AppendLine($"{spaces}The built site will be put into '{_options.DestPath}'.");
            sb.AppendLine($"{spaces}Uses '{_options.KindOfTargetUrl}' as target url's strategy.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
