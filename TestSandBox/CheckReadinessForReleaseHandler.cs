using BaseDevPipeline;
using Deployment.Helpers;
using NLog;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestSandBox
{
    public class CheckReadinessForReleaseHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var reportSb = new StringBuilder();
            reportSb.AppendLine($"Created: {DateTime.Now}");

            var warnings = new List<string>();
            var errors = new List<string>();

            var mayIMakeRelease = FutureReleaseGuard.CheckMayIMakeRelease(errors);

            _logger.Info($"mayIMakeRelease = {mayIMakeRelease}");

            reportSb.AppendLine();
            reportSb.AppendLine($"FutureReleaseGuard.MayIMakeRelease: {(mayIMakeRelease ? "Yes" : "No")}");

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var secretFilePath = settings.SecretFilePath;

            _logger.Info($"secretFilePath = {secretFilePath}");

            reportSb.AppendLine();
            reportSb.AppendLine("GitHubToken:");

            if (string.IsNullOrWhiteSpace(secretFilePath))
            {
                var errorMessage = "SecretFilePath is Empty";

                errors.Add(errorMessage);
                reportSb.AppendLine(errorMessage);
            }
            else
            {
                var secretFileLastWriteTime = File.GetLastWriteTime(secretFilePath);

                _logger.Info($"secretFileLastWriteTime = {secretFileLastWriteTime}");

                reportSb.AppendLine($"Checked: {secretFileLastWriteTime}");

                var secrets = settings.GetSecrets();

                reportSb.AppendLine($"Secrets count: {secrets.Count}");

                foreach (var secret in secrets)
                {
                    _logger.Info($"secret.Key = {secret.Key}");
                    _logger.Info($"secret.Value.ExpDate = {secret.Value.ExpDate}");

                    reportSb.AppendLine($"{secret.Key}: ExpDate: {secret.Value.ExpDate}");
                }

                var checkGitHubTokenResult = GitHubTokenHelper.CheckGitHubToken(errors);

                _logger.Info($"checkGitHubTokenResult = {checkGitHubTokenResult}");

                reportSb.AppendLine(checkGitHubTokenResult? "Valid" : "Invaid");
            }

            var futureReleaseInfoRepositoryName = FutureReleaseInfoReader.GetRepositoryName();

#if DEBUG
            _logger.Info($"futureReleaseInfoRepositoryName = {futureReleaseInfoRepositoryName}");
#endif

            {
                var futureReleaseBaseRepositoryPath = FutureReleaseInfoReader.GetBaseRepositoryPath();

                var currentBranch = GitRepositoryHelper.GetCurrentBranchName(futureReleaseBaseRepositoryPath);

#if DEBUG
                _logger.Info($"currentBranch = {currentBranch}");
#endif

                var uncommitedFilesList = GitRepositoryHelper.GetRepositoryFileInfoList(futureReleaseBaseRepositoryPath);

#if DEBUG
                _logger.Info($"uncommitedFilesList.Count = {uncommitedFilesList.Count}");
#endif

                foreach (var uncommitedFile in uncommitedFilesList)
                {
#if DEBUG
                    _logger.Info($"uncommitedFile = {uncommitedFile}");
#endif
                }

                var hasUncommitedFiles = uncommitedFilesList.Any();

#if DEBUG
                _logger.Info($"hasUncommitedFiles = {hasUncommitedFiles}");
#endif
            }

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");

            var currentVersion = futureReleaseInfo.Version;

            _logger.Info($"currentVersion = {currentVersion}");

            var futureReleaseNotesFullFileName = FutureReleaseInfoReader.GetFutureReleaseNotesFullFileName();

            _logger.Info($"futureReleaseNotesFullFileName = {futureReleaseNotesFullFileName}");

            var futureReleaseNotesFileLastWriteTime = File.GetLastWriteTime(futureReleaseNotesFullFileName);

            _logger.Info($"futureReleaseNotesFileLastWriteTime = {futureReleaseNotesFileLastWriteTime}");

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

            _logger.Info($"targetSolutions.Count = {targetSolutions.Count}");

            foreach (var targetSolution in targetSolutions)
            {
                _logger.Info($"targetSolution.Name = {targetSolution.Name}");
                _logger.Info($"targetSolution.Path = {targetSolution.Path}");

                var currentBranch = GitRepositoryHelper.GetCurrentBranchName(targetSolution.Path);

#if DEBUG
                _logger.Info($"currentBranch = {currentBranch}");
#endif

                var uncommitedFilesList = GitRepositoryHelper.GetRepositoryFileInfoList(targetSolution.Path);

#if DEBUG
                _logger.Info($"uncommitedFilesList.Count = {uncommitedFilesList.Count}");
#endif

                foreach (var uncommitedFile in uncommitedFilesList)
                {
#if DEBUG
                    _logger.Info($"uncommitedFile = {uncommitedFile}");
#endif
                }

                var hasUncommitedFiles = uncommitedFilesList.Any();

#if DEBUG
                _logger.Info($"hasUncommitedFiles = {hasUncommitedFiles}");
#endif
            }

            _logger.Info($"warnings = {warnings.WritePODListToString()}");
            _logger.Info($"errors = {errors.WritePODListToString()}");
            _logger.Info($"reportSb = {reportSb}");

            _logger.Info("End");
        }
    }
}
