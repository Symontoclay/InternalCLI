﻿using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment.Tasks.ProjectsTasks.UpdateProjectCopyright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateSolutionCopyright
{
    public class UpdateSolutionCopyrightTask : BaseDeploymentTask
    {
        public UpdateSolutionCopyrightTask(UpdateSolutionCopyrightTaskOptions options)
            : this(options, 0u)
        {
        }

        public UpdateSolutionCopyrightTask(UpdateSolutionCopyrightTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UpdateSolutionCopyrightTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.SolutionFilePath), _options.SolutionFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var fileInfo = new FileInfo(_options.SolutionFilePath);

            var projectNamesList = SolutionHelper.GetProjectsNames(fileInfo.DirectoryName);

            foreach (var projectName in projectNamesList)
            {
                Exec(new UpdateProjectCopyrightTask(new UpdateProjectCopyrightTaskOptions() {
                    ProjectFilePath = projectName,
                    Copyright = _options.Copyright
                }, NextDeep));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in solution '{_options.SolutionFilePath}'.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Copyright);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
