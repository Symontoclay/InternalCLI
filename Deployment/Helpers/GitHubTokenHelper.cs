using BaseDevPipeline;
using NLog;
using System;
using System.Collections.Generic;

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
            return CheckGitHubToken(errorMessage => { logger?.Error(errorMessage); });
        }

        /// <summary>
        /// Checks GitHub token.
        /// Writes error to a string list, if It needs.
        /// </summary>
        /// <param name="errors">The string list for writing error.</param>
        /// <returns><b>true</b> if the GitHub token is valid, otherwise returns <b>false</b>.</returns>
        public static bool CheckGitHubToken(List<string> errors)
        {
            return CheckGitHubToken(errorMessage => { errors?.Add(errorMessage); });
        }

        private static bool CheckGitHubToken(Action<string> onErrorHandler)
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var token = settings.GetSecret(GitHubTokenKey);

            if (string.IsNullOrEmpty(token.Value))
            {
                onErrorHandler?.Invoke($"Making release is forbiden! {GitHubTokenKey} token is empty!");

                return false;
            }

            if (!token.ExpDate.HasValue)
            {
                onErrorHandler?.Invoke($"Making release is forbiden! ExpDate of {GitHubTokenKey} token is empty!");

                return false;
            }

            if (token.ExpDate <= DateTime.Now)
            {
                onErrorHandler?.Invoke($"Making release is forbiden! {GitHubTokenKey} token is expired!");

                return false;
            }

            return true;
        }
    }
}
