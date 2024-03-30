﻿using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution
{
    public class UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask : OldBaseDeploymentTask
    {
        public UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions options)
            : this(options, 0u)
        {
        }

        public UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions _options;

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

            fileNamesGetterOptions.ExceptSubDirs = new List<string>() 
            {
                ".git",
                ".vs",
                "bin",
                "obj"
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
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in files' headers of project or solution '{_options.SourceDir}'.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Text);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
