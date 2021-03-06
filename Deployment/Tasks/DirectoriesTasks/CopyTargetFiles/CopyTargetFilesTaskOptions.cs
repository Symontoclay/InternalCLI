﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopyTargetFiles
{
    public class CopyTargetFilesTaskOptions : IObjectToString
    {
        public string DestDir { get; set; }
        public bool SaveSubDirs { get; set; } = true;
        public List<string> TargetFiles { get; set; }
        public string BaseSourceDir { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");
            sb.AppendLine($"{spaces}{nameof(SaveSubDirs)} = {SaveSubDirs}");
            sb.AppendLine($"{spaces}{nameof(BaseSourceDir)} = {BaseSourceDir}");
            sb.PrintPODList(n, nameof(TargetFiles), TargetFiles);

            return sb.ToString();
        }
    }
}
