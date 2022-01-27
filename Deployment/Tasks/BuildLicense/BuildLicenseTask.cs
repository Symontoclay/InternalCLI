using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildLicense
{
    public class BuildLicenseTask : BaseDeploymentTask
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
    }
}
