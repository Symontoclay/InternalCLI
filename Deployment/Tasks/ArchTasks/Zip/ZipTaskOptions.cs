﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ArchTasks.Zip
{
    public class ZipTaskOptions : IObjectToString
    {
        public string SourceDir { get; set; }
        public string OutputFilePath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(SourceDir)} = {SourceDir}");
            sb.AppendLine($"{spaces}{nameof(OutputFilePath)} = {OutputFilePath}");

            return sb.ToString();
        }
    }
}
