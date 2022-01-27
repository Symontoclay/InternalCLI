using System;
using System.Collections.Generic;
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


    }
}
