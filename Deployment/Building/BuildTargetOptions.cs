﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public class BuildTargetOptions : IObjectToString
    {
        public KindOfBuildTarget Kind { get; set; } = KindOfBuildTarget.Unknown;
        public string TargetDir { get; set; }
        public bool SkipExistingFilesInTargetDir { get; set; } = true;

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(TargetDir)} = {TargetDir}");
            sb.AppendLine($"{spaces}{nameof(SkipExistingFilesInTargetDir)} = {SkipExistingFilesInTargetDir}");

            return sb.ToString();
        }
    }
}
