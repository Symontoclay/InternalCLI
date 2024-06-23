using BaseDevPipeline;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class FutureReleaseGuard
    {
        /// <summary>
        /// Checks the possibility of making a release.
        /// </summary>
        /// <returns><b>true</b> if the release is possible, otherwise returns <b>false</b>.</returns>
        public static bool MayIMakeRelease()
        {
            return FutureReleaseInfoReader.Read().Status == FutureReleaseStatus.Started;
        }

        /// <summary>
        /// Checks the possibility of making a release.
        /// Writes error to an logger, if It needs.
        /// </summary>
        /// <param name="logger">An logger for writing error.</param>
        /// <returns><b>true</b> if the release is possible, otherwise returns <b>false</b>.</returns>
        public static bool CheckMayIMakeRelease(Logger logger)
        {
            if(MayIMakeRelease())
            {
                return true;
            }

            logger?.Error("Starting new version is forbiden! New version has already been started!");

            return false;
        }
    }
}
