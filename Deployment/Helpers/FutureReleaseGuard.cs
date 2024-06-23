using BaseDevPipeline;
using NLog;
using System;
using System.Collections.Generic;

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
            return CheckMayIMakeRelease(errorMessage => { logger?.Error(errorMessage); });
        }

        /// <summary>
        /// Checks the possibility of making a release.
        /// Writes error to a string list, if It needs.
        /// </summary>
        /// <param name="errors">The string list for writing error.</param>
        /// <returns><b>true</b> if the release is possible, otherwise returns <b>false</b>.</returns>
        public static bool CheckMayIMakeRelease(List<string> errors)
        {
            return CheckMayIMakeRelease(errorMessage => { errors?.Add(errorMessage); });
        }

        private static bool CheckMayIMakeRelease(Action<string> onErrorHandler)
        {
            if (MayIMakeRelease())
            {
                return true;
            }

            onErrorHandler?.Invoke("Starting new version is forbiden! New version has already been started!");

            return false;
        }
    }
}
