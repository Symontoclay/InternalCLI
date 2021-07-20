using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.SiteTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateReleaseNotesTask(UpdateReleaseNotesTaskOptions options)
        {
            _options = options;
        }

        private readonly UpdateReleaseNotesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.FutureReleaseFilePath), _options.FutureReleaseFilePath);
            ValidateFileName(nameof(_options.ReleaseNotesFilePath), _options.ReleaseNotesFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif

            var futureReleaseInfo = FutureReleaseInfo.ReadFile(_options.FutureReleaseFilePath);

#if DEBUG
            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");
#endif

            var version = futureReleaseInfo.Version;

            var releaseNotesList = ReadReleaseItems();

#if DEBUG
            _logger.Info($"releaseNotesList = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");
#endif

            var targetReleaseNote = releaseNotesList.SingleOrDefault(p => p.Version == version);

            if(targetReleaseNote == null)
            {
                targetReleaseNote = new ReleaseItem()
                {
                    Version = version
                };

                releaseNotesList.Add(targetReleaseNote);
            }

#if DEBUG
            _logger.Info($"targetReleaseNote = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            targetReleaseNote.Date = DateTime.Now.Date;
            targetReleaseNote.IsMarkdown = true;
            targetReleaseNote.Description = futureReleaseInfo.Description;
            targetReleaseNote.AssetsList = new List<ReleaseAssetItem>();

#if DEBUG
            _logger.Info($"targetReleaseNote (2) = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            var targetAtrifactsList = KindOfArtifactHelper.GetOrderedListForDeployment().Intersect(_options.ArtifactsForDeployment);

#if DEBUG
            _logger.Info($"targetAtrifactsList = {targetAtrifactsList.WritePODList()}");
#endif

            foreach(var targetArtifact in targetAtrifactsList)
            {
#if DEBUG
                _logger.Info($"targetArtifact = {targetArtifact}");
#endif

                switch (targetArtifact)
                {
                    case KindOfArtifact.UnityPackage:
                        {
                            var assetItem = new ReleaseAssetItem();

                            assetItem.Kind = KindOfReleaseAssetItem.Unity3DAsset;
                            assetItem.Title = "Unity package";
                            assetItem.Href = $"{_options.BaseHref}releases/download/{version}/SymOntoClay-{version}.unitypackage";

#if DEBUG
                            _logger.Info($"assetItem = {assetItem}");
#endif

                            targetReleaseNote.AssetsList.Add(assetItem);
                        }
                        break;

                    case KindOfArtifact.CLIArch:
                        {
                            var assetItem = new ReleaseAssetItem();

                            assetItem.Kind = KindOfReleaseAssetItem.CLIArch;
                            assetItem.Title = "Portable CLI (.zip)";
                            assetItem.Href = $"{_options.BaseHref}releases/download/{version}/SymOntoClay-CLI-{version}-x64.zip";

#if DEBUG
                            _logger.Info($"assetItem = {assetItem}");
#endif

                            targetReleaseNote.AssetsList.Add(assetItem);
                        }
                        break;

                    case KindOfArtifact.SourceArch:
                        {
                            var assetItem = new ReleaseAssetItem();

                            assetItem.Kind = KindOfReleaseAssetItem.SourceCodeZip;
                            assetItem.Title = "Source code (.zip)";
                            assetItem.Href = $"{_options.BaseHref}archive/refs/tags/{version}.zip";

#if DEBUG
                            _logger.Info($"assetItem = {assetItem}");
#endif

                            targetReleaseNote.AssetsList.Add(assetItem);

                            assetItem = new ReleaseAssetItem();

                            assetItem.Kind = KindOfReleaseAssetItem.SourceCodeZip;
                            assetItem.Title = "Source code (.tar.gz)";
                            assetItem.Href = $"{_options.BaseHref}archive/refs/tags/{version}.tar.gz";

#if DEBUG
                            _logger.Info($"assetItem = {assetItem}");
#endif

                            targetReleaseNote.AssetsList.Add(assetItem);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(targetArtifact), targetArtifact, null);
                }
            }

#if DEBUG
            _logger.Info($"targetReleaseNote (3) = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            releaseNotesList = releaseNotesList.OrderByDescending(p => p.Date).ToList();

            _logger.Info($"releaseNotesList (2) = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");

            SaveReleaseItems(releaseNotesList);
        }

        private void SaveReleaseItems(List<ReleaseItem> releaseNotesList)
        {
            File.WriteAllText(_options.ReleaseNotesFilePath, JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented));
        }

        private List<ReleaseItem> ReadReleaseItems()
        {
            return JsonConvert.DeserializeObject<List<ReleaseItem>>(File.ReadAllText(_options.ReleaseNotesFilePath));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.AppendLine($"{spaces}Exports directory '{_options.SourceDir}' of '{_options.RootDir}' to Unity package '{_options.OutputPackageName}'");
            //sb.AppendLine($"{spaces}'{_options.UnityExeFilePath}' will be used as exe.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
