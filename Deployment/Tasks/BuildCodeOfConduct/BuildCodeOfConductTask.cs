using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildCodeOfConduct
{
    public class BuildCodeOfConductTask : BaseDeploymentTask
    {
        public BuildCodeOfConductTask(BuildCodeOfConductTaskOptions options)
            : this(options, 0u)
        {
        }

        public BuildCodeOfConductTask(BuildCodeOfConductTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly BuildCodeOfConductTaskOptions _options;

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
