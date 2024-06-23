using BaseDevPipeline;
using Deployment.Helpers;
using NLog;
using System;
using System.IO;

namespace TestSandBox
{
    public class CheckReadinessForReleaseHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var secretFilePath = settings.SecretFilePath;

            _logger.Info($"secretFilePath = {secretFilePath}");

            var secretFileLastWriteTime = File.GetLastWriteTime(secretFilePath);

            _logger.Info($"secretFileLastWriteTime = {secretFileLastWriteTime}");

            var secrets = settings.GetSecrets();

            foreach(var secret in secrets)
            {
                _logger.Info($"secret.Key = {secret.Key}");
                _logger.Info($"secret.Value.ExpDate = {secret.Value.ExpDate}");
            }

            var token = settings.GetSecret("GitHub");

            if (string.IsNullOrEmpty(token.Value))
            {
                _logger.Info("Making release is forbiden! GitHub token is empty!");
            }

            if (!token.ExpDate.HasValue)
            {
                _logger.Info("Making release is forbiden! ExpDate of GitHub token is empty!");
            }

            if (token.ExpDate <= DateTime.Now)
            {
                _logger.Info("Making release is forbiden! GitHub token is expired!");
            }

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");

            var futureReleaseNotesFullFileName = FutureReleaseInfoReader.GetFutureReleaseNotesFullFileName();

            _logger.Info($"futureReleaseNotesFullFileName = {futureReleaseNotesFullFileName}");

            var futureReleaseNotesFileLastWriteTime = File.GetLastWriteTime(futureReleaseNotesFullFileName);

            _logger.Info($"futureReleaseNotesFileLastWriteTime = {futureReleaseNotesFileLastWriteTime}");

            //var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            _logger.Info("End");
        }
    }
}
