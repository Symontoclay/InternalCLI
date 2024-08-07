﻿using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Newtonsoft.Json;
using SiteBuilder.SiteData;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.SiteTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateReleaseNotesTask(UpdateReleaseNotesTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateReleaseNotesTask(UpdateReleaseNotesTaskOptions options, IDeploymentTask parentTask)
            : base("4A489A50-C0FB-4281-BC0E-CD3F25374486", false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateReleaseNotesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateValueAsNonNull(nameof(_options.FutureReleaseInfo), _options.FutureReleaseInfo);
            ValidateList(nameof(_options.ArtifactsForDeployment), _options.ArtifactsForDeployment);
            ValidateFileName(nameof(_options.ReleaseNotesFilePath), _options.ReleaseNotesFilePath);
            ValidateValueAsNonNull(nameof(_options.BaseHref), _options.BaseHref);
        }
        
        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            //_logger.Info($"_options = {_options}");
#endif

            var futureReleaseInfo = _options.FutureReleaseInfo;

#if DEBUG
            //_logger.Info($"futureReleaseInfo = {futureReleaseInfo}");
#endif

            var version = futureReleaseInfo.Version;

            var releaseNotesList = ReadReleaseItems();

#if DEBUG
            //_logger.Info($"releaseNotesList = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");
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
            //_logger.Info($"targetReleaseNote = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            targetReleaseNote.Date = DateTime.Now.Date;
            targetReleaseNote.IsMarkdown = true;
            targetReleaseNote.Description = futureReleaseInfo.Description;
            targetReleaseNote.AssetsList = new List<ReleaseAssetItem>();

#if DEBUG
            //_logger.Info($"targetReleaseNote (2) = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            var deployedItemsList = DeployedItemsFactory.Create(version, _options.ArtifactsForDeployment, _options.BaseHref, string.Empty);

            //_logger.Info($"deployedItemsList = {deployedItemsList.WriteListToString()}");

            foreach(var deployedItem in deployedItemsList)
            {
#if DEBUG
                //_logger.Info($"deployedItem = {deployedItem}");
#endif

                var kindOfReleaseAssetItem = KindOfArtifactHelper.ConvertToKindOfReleaseAssetItem(deployedItem.Kind);

#if DEBUG
                //_logger.Info($"kindOfReleaseAssetItem = {kindOfReleaseAssetItem}");
#endif

                var assetItem = new ReleaseAssetItem();
                assetItem.Kind = kindOfReleaseAssetItem;
                assetItem.Href = deployedItem.Href;
                assetItem.Title = KindOfReleaseAssetItemHelper.GetAssetTitle(kindOfReleaseAssetItem, deployedItem.Href);

#if DEBUG
                //_logger.Info($"assetItem = {assetItem}");
#endif

                targetReleaseNote.AssetsList.Add(assetItem);
            }

#if DEBUG
            //_logger.Info($"targetReleaseNote (3) = {JsonConvert.SerializeObject(targetReleaseNote, Formatting.Indented)}");
#endif

            releaseNotesList = releaseNotesList.OrderByDescending(p => p.Date).ToList();

            //_logger.Info($"releaseNotesList (2) = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");

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
            var next_N = n + DisplayHelper.IndentationStep;
            var nextSpaces = DisplayHelper.Spaces(next_N);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Adds or updates future release note using base url '{_options.BaseHref}'.");
            sb.PrintObjProp(n, nameof(_options.FutureReleaseInfo), _options.FutureReleaseInfo);
            sb.AppendLine($"{spaces}Updated release notes are at '{_options.ReleaseNotesFilePath}'.");
            sb.AppendLine($"{spaces}The artefactes for deployment:");
            foreach (var item in _options.ArtifactsForDeployment)
            {
                sb.AppendLine($"{nextSpaces}{item}");
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
