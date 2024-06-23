using BaseDevPipeline;
using NLog;
using System;

namespace Deployment.Helpers
{
    public static class GitHubTokenHelper
    {
        public const string GitHubTokenKey = "GitHub";

        /// <summary>
        /// Checks GitHub token.
        /// Writes errors to an logger, if It needs.
        /// </summary>
        /// <param name="logger">An logger for writing errors.</param>
        /// <returns><b>true</b> if the GitHub token is valid, otherwise returns <b>false</b>.</returns>
        public static bool CheckGitHubToken(Logger logger)
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var token = settings.GetSecret(GitHubTokenKey);

            if (string.IsNullOrEmpty(token.Value))
            {
                logger?.Error($"Making release is forbiden! {GitHubTokenKey} token is empty!");

                return false;
            }

            if (!token.ExpDate.HasValue)
            {
                logger?.Error($"Making release is forbiden! ExpDate of {GitHubTokenKey} token is empty!");

                return false;
            }

            if (token.ExpDate <= DateTime.Now)
            {
                logger?.Error($"Making release is forbiden! {GitHubTokenKey} token is expired!");

                return false;
            }

            return true;
        }
    }
}
