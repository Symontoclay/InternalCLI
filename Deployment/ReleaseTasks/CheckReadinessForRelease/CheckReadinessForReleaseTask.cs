using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.ReleaseTasks.CheckReadinessForRelease
{
    public class CheckReadinessForReleaseTask : BaseDeploymentTask
    {
        public CheckReadinessForReleaseTask(CheckReadinessForReleaseTaskOptions options)
            : this(options, null)
        {
        }

        public CheckReadinessForReleaseTask(CheckReadinessForReleaseTaskOptions options, IDeploymentTask parentTask)
            : base("5B732A8F-B586-4D0B-9BBE-764262AA2B6A", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CheckReadinessForReleaseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.OutputFileName), _options.OutputFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info($"_options.OutputFileName = {_options.OutputFileName}");
#endif

            _options.OutputFileName = EVPath.Normalize(_options.OutputFileName);

#if DEBUG
            _logger.Info($"_options.OutputFileName (after) = {_options.OutputFileName}");
#endif

            var reportSb = new StringBuilder();
            reportSb.AppendLine($"Report created: {DateTime.Now}");

            var warnings = new List<string>();
            var errors = new List<string>();

            var mayIMakeRelease = FutureReleaseGuard.CheckMayIMakeRelease(errors);

#if DEBUG
            _logger.Info($"mayIMakeRelease = {mayIMakeRelease}");
#endif

            reportSb.AppendLine();
            reportSb.AppendLine($"FutureReleaseGuard.MayIMakeRelease: {(mayIMakeRelease ? "Yes" : "No")}");

            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var secretFilePath = settings.SecretFilePath;

#if DEBUG
            _logger.Info($"secretFilePath = {secretFilePath}");
#endif

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

#if DEBUG
                _logger.Info($"secretFileLastWriteTime = {secretFileLastWriteTime}");
#endif

                reportSb.AppendLine($"Checked: {secretFileLastWriteTime}");

                var secrets = settings.GetSecrets();

                reportSb.AppendLine($"Secrets count: {secrets.Count}");

                foreach (var secret in secrets)
                {
#if DEBUG
                    _logger.Info($"secret.Key = {secret.Key}");
                    _logger.Info($"secret.Value.ExpDate = {secret.Value.ExpDate}");
#endif

                    reportSb.AppendLine($"{secret.Key}: ExpDate: {secret.Value.ExpDate}");
                }

                var checkGitHubTokenResult = GitHubTokenHelper.CheckGitHubToken(errors);

#if DEBUG
                _logger.Info($"checkGitHubTokenResult = {checkGitHubTokenResult}");
#endif

                reportSb.AppendLine(checkGitHubTokenResult ? "Valid" : "Invaid");
            }

            var futureReleaseInfoRepositoryName = FutureReleaseInfoReader.GetRepositoryName();

#if DEBUG
            _logger.Info($"futureReleaseInfoRepositoryName = {futureReleaseInfoRepositoryName}");
#endif

            var futureReleaseBaseRepositoryPath = FutureReleaseInfoReader.GetBaseRepositoryPath();

            CheckRepository(futureReleaseInfoRepositoryName, futureReleaseBaseRepositoryPath, "master", reportSb, warnings, errors);

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

#if DEBUG
            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");
#endif

            var currentVersion = futureReleaseInfo.Version;

#if DEBUG
            _logger.Info($"currentVersion = {currentVersion}");
#endif

            reportSb.AppendLine();
            reportSb.AppendLine($"CurrentVersion: {currentVersion}");
            reportSb.AppendLine($"Started: {futureReleaseInfo.StartDate}");

            reportSb.AppendLine();
            reportSb.AppendLine($"Release notes:");
            reportSb.AppendLine(futureReleaseInfo.Description);

            var futureReleaseNotesFullFileName = FutureReleaseInfoReader.GetFutureReleaseNotesFullFileName();

#if DEBUG
            _logger.Info($"futureReleaseNotesFullFileName = {futureReleaseNotesFullFileName}");
#endif

            var futureReleaseNotesFileLastWriteTime = File.GetLastWriteTime(futureReleaseNotesFullFileName);

#if DEBUG
            _logger.Info($"futureReleaseNotesFileLastWriteTime = {futureReleaseNotesFileLastWriteTime}");
#endif

            reportSb.AppendLine($"Release notes checked: {futureReleaseNotesFileLastWriteTime} {(futureReleaseNotesFileLastWriteTime < futureReleaseInfo.StartDate ? "!!!!Release notes too old" : string.Empty)}");

            if (futureReleaseNotesFileLastWriteTime < futureReleaseInfo.StartDate)
            {
                errors.Add("!!!!Release notes too old");
            }

            var targetSolutions = ProjectsDataSourceFactory.GetSolutionsWithMaintainedReleases();

#if DEBUG
            _logger.Info($"targetSolutions.Count = {targetSolutions.Count}");
#endif

            reportSb.AppendLine();
            reportSb.AppendLine($"Repositories count: {targetSolutions.Count}");

            foreach (var targetSolution in targetSolutions)
            {
#if DEBUG
                _logger.Info($"targetSolution.Name = {targetSolution.Name}");
                _logger.Info($"targetSolution.Path = {targetSolution.Path}");
#endif

                CheckRepository(targetSolution.Name, targetSolution.Path, currentVersion, reportSb, warnings, errors);
            }

#if DEBUG
            _logger.Info($"warnings = {warnings.WritePODListToString()}");
            _logger.Info($"errors = {errors.WritePODListToString()}");
#endif

            if (warnings.Any())
            {
                reportSb.AppendLine();
                reportSb.AppendLine("Warnings:");

                foreach (var warning in warnings)
                {
                    reportSb.AppendLine(warning);
                }
            }

            if (errors.Any())
            {
                reportSb.AppendLine();
                reportSb.AppendLine("Errors:");

                foreach (var error in errors)
                {
                    reportSb.AppendLine(error);
                }
            }

            var resolution = errors.Any() ? "Error!!!!. Release forbiden!" : (warnings.Any() ? "Warning! You can release, but It would be better to check all warnings firstly." : "All ok.");

            reportSb.AppendLine();
            reportSb.AppendLine($"Resolution: {resolution}");

#if DEBUG
            _logger.Info($"reportSb = {reportSb}");
#endif

            File.WriteAllText(_options.OutputFileName, reportSb.ToString());
        }

        private void CheckRepository(string repositoryName, string repositoryPath, string targetBranchName, StringBuilder reportSb, List<string> warnings, List<string> errors)
        {
#if DEBUG
            _logger.Info($"repositoryName = {repositoryName}");
            _logger.Info($"repositoryPath = {repositoryPath}");
            _logger.Info($"targetBranchName = {targetBranchName}");
#endif

            reportSb.AppendLine();
            reportSb.AppendLine($"{repositoryName}:");

            var checkRepositoryResult = GetRepositoryInfo(repositoryPath);

            var currentBranch = checkRepositoryResult.CurrentBranchName;

#if DEBUG
            _logger.Info($"currentBranch = {currentBranch}");
#endif

            reportSb.AppendLine($"CurrentBranch: {currentBranch} {(currentBranch == targetBranchName ? string.Empty : $"!!!!! Should be {targetBranchName}")}");

            if (currentBranch != targetBranchName)
            {
                errors.Add($"{repositoryName}: CurrentBranch: {currentBranch} !!! But should be {targetBranchName}");
            }

            var hasUncommitedFiles = checkRepositoryResult.HasUncommitedFiles;

#if DEBUG
            _logger.Info($"hasUncommitedFiles = {hasUncommitedFiles}");
#endif

            reportSb.AppendLine($"Files: {(hasUncommitedFiles ? "!!!!Has uncommited files" : "All files are commited")}");

            if (hasUncommitedFiles)
            {
                errors.Add($"{repositoryName}: !!!!Has uncommited files");
            }
        }

        private (string CurrentBranchName, bool HasUncommitedFiles) GetRepositoryInfo(string repositoryPath)
        {
#if DEBUG
            _logger.Info($"repositoryPath = {repositoryPath}");
#endif

            var currentBranch = GitRepositoryHelper.GetCurrentBranchName(repositoryPath);

#if DEBUG
            _logger.Info($"currentBranch = {currentBranch}");
#endif

            var uncommitedFilesList = GitRepositoryHelper.GetRepositoryFileInfoList(repositoryPath);

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

            return (currentBranch, hasUncommitedFiles);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Checks readiness for release.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
