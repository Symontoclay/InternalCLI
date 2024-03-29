﻿using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment.Tasks.ProjectsTasks.UpdateProjectVersion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateSolutionVersion
{
    public class UpdateSolutionVersionTask : BaseDeploymentTask
    {
        public UpdateSolutionVersionTask(UpdateSolutionVersionTaskOptions options)
            : this(options, 0u)
        {
        }

        public UpdateSolutionVersionTask(UpdateSolutionVersionTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UpdateSolutionVersionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.SolutionFilePath), _options.SolutionFilePath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var fileInfo = new FileInfo(_options.SolutionFilePath);

            var projectNamesList = SolutionHelper.GetProjectsNames(fileInfo.DirectoryName);

            foreach (var projectName in projectNamesList)
            {
                Exec(new UpdateProjectVersionTask(new UpdateProjectVersionTaskOptions() { 
                   ProjectFilePath = projectName,
                   Version = _options.Version
                }, NextDeep));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates version in solution '{_options.SolutionFilePath}'.");
            sb.AppendLine($"{spaces}The version: {_options.Version}");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
