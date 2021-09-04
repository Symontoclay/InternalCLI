using BaseDevPipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class FutureReleaseGuard
    {
        public static bool MayIMakeRelease()
        {
            return FutureReleaseInfoReader.Read().Status == FutureReleaseStatus.Started;
        }
    }
}
