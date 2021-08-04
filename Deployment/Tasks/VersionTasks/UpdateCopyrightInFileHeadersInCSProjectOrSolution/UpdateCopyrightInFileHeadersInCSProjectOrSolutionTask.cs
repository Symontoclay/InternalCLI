﻿using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeaders;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.VersionTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution
{
    public class UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask : BaseDeploymentTask
    {
        public UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions options)
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
            }));
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