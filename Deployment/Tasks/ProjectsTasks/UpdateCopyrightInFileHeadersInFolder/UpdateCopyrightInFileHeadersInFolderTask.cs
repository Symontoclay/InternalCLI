﻿using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInFolder
{
    public class UpdateCopyrightInFileHeadersInFolderTask : BaseDeploymentTask
    {
        public UpdateCopyrightInFileHeadersInFolderTask(UpdateCopyrightInFileHeadersInFolderTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateCopyrightInFileHeadersInFolderTask(UpdateCopyrightInFileHeadersInFolderTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.SourceDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateCopyrightInFileHeadersInFolderTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var fileNamesGetterOptions = new FileNamesGetterOptions();

            fileNamesGetterOptions.SourceDir = _options.SourceDir;
            fileNamesGetterOptions.OnlyFileExts = new List<string>()
            {
                "cs"
            };

            var fileNamesGetter = new FileNamesGetter(fileNamesGetterOptions);

            var sourceFullFileNamesList = fileNamesGetter.GetFileNames();

            if (!sourceFullFileNamesList.Any())
            {
                return;
            }

            Exec(new UpdateCopyrightInFileHeadersTask(new UpdateCopyrightInFileHeadersTaskOptions()
            {
                Text = _options.Text,
                TargetFiles = sourceFullFileNamesList
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in files' headers of directory '{_options.SourceDir}'.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Text);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
