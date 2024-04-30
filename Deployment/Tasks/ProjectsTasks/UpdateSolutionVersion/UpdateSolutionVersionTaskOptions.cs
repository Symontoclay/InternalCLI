﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateSolutionVersion
{
    public class UpdateSolutionVersionTaskOptions : IObjectToString
    {
        public string SolutionFilePath { get; set; }
        public string Version { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(SolutionFilePath)} = {SolutionFilePath}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");

            return sb.ToString();
        }
    }
}
