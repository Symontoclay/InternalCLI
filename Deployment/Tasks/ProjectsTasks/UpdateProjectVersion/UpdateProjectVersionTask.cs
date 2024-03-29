﻿using CommonUtils.DebugHelpers;
using CSharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateProjectVersion
{
    public class UpdateProjectVersionTask : BaseDeploymentTask
    {
        public UpdateProjectVersionTask(UpdateProjectVersionTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UpdateProjectVersionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectFilePath), _options.ProjectFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            CSharpProjectHelper.SetVersion(_options.ProjectFilePath, _options.Version);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates version in project '{_options.ProjectFilePath}'.");
            sb.AppendLine($"{spaces}The version:");
            sb.AppendLine(_options.Version);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
