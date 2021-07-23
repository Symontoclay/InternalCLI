using CommonUtils.DebugHelpers;
using NLog;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitHubTasks.GitHubRelease
{
    public class GitHubReleaseTask : BaseDeploymentTask
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public GitHubReleaseTask(GitHubReleaseTaskOptions options)
        {
            _options = options;

#if DEBUG
            _logger.Info($"_options = {_options}");
#endif
        }

        private readonly GitHubReleaseTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Token), _options.Token);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.RepositoryOwner), _options.RepositoryOwner);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.RepositoryName), _options.RepositoryName);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.NotesText), _options.NotesText);
            ValidateList(nameof(_options.Assets), _options.Assets);

            _options.Assets.ForEach(p => 
            {
                ValidateStringValueAsNonNullOrEmpty(nameof(p.DisplayedName), p.DisplayedName);
                ValidateFileName(nameof(p.UploadedFilePath), p.UploadedFilePath);
            });
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var notExistingAssetsFiles = _options.Assets.Where(p => File.Exists(p.UploadedFilePath));

            if (notExistingAssetsFiles.Any())
            {
                var errorSb = new StringBuilder();
                errorSb.AppendLine("These asset's files are not exist:");
                foreach(var file in notExistingAssetsFiles)
                {
                    errorSb.AppendLine(file.UploadedFilePath);
                }

                throw new FileNotFoundException(errorSb.ToString());
            }

            var client = new GitHubClient(new ProductHeaderValue("SymOntoClay-InternalCLI"));

            var tokenAuth = new Credentials(_options.Token);
            client.Credentials = tokenAuth;

            var newRelease = new NewRelease(_options.Version);
            newRelease.Name = _options.Version;
            newRelease.Body = _options.NotesText;
            newRelease.Draft = _options.Draft;
            newRelease.Prerelease = _options.Prerelease;

            var resultTask = client.Repository.Release.Create(_options.RepositoryOwner, _options.RepositoryName, newRelease);

            resultTask.Wait();

            var result = resultTask.Result;

#if DEBUG
            _logger.Info($"result.Id = {result.Id}");
#endif

            var releaseTask = client.Repository.Release.Get(_options.RepositoryOwner, _options.RepositoryName, result.Id);

            var releaseInfo = releaseTask.Result;

            foreach (var item in _options.Assets)
            {
                using var archiveContents = File.OpenRead(item.UploadedFilePath);

                var assetUpload = new ReleaseAssetUpload()
                {
                    FileName = item.DisplayedName,
                    ContentType = "application/zip",
                    RawData = archiveContents
                };

                var assetTask = client.Repository.Release.UploadAsset(releaseInfo, assetUpload);

                assetTask.Wait();
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Makes github {(_options.Prerelease ? "pre": string.Empty)}release {(_options.Draft ? "draft " : string.Empty)}the version '{_options.Version}' of repository '{_options.RepositoryName}' of '{_options.RepositoryOwner}'.");
            sb.AppendLine($"{spaces}Release notes:");
            sb.PrintPODValue(next_N, _options.NotesText);
            sb.AppendLine($"{spaces}Assets:");            
            foreach(var item in _options.Assets)
            {
                sb.AppendLine($"{nextSpaces}'{item.DisplayedName}' from '{item.UploadedFilePath}'.");
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
