﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace CommonUtils.DeploymentTasks.Serialization
{
    public class PipelineInfo : IObjectToString
    {
        public bool? IsFinished { get; set; }
        public string LastRunInfo { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(IsFinished)} = {IsFinished}");
            sb.AppendLine($"{spaces}{nameof(LastRunInfo)} = {LastRunInfo}");
            return sb.ToString();
        }
    }
}
