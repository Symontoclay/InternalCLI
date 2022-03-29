using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ExamplesCreator
{
    public class BuildExamplesTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public BuildExamplesTask(BuildExamplesTaskOptions options)
            : this(options, 0u)
        {
        }

        public BuildExamplesTask(BuildExamplesTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly BuildExamplesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateList(nameof(_options.LngExamplesPages), _options.LngExamplesPages);
            ValidateFileName(nameof(_options.SocExePath), _options.SocExePath);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {

        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = DisplayHelper.Spaces(nextN);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds examples to '{_options.DestDir}'.");
            sb.AppendLine($"{spaces}Source pages:");

            foreach(var lngExamplesPage in _options.LngExamplesPages)
            {
                sb.AppendLine($"{nextNSpaces}{lngExamplesPage}");
            }

            sb.AppendLine($"{spaces}Using CLI on '{_options.SocExePath}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
