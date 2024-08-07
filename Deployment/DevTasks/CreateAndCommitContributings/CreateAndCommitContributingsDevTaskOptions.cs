﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.CreateAndCommitContributings
{
    public class CreateAndCommitContributingsDevTaskOptions : IObjectToString
    {
        public string Message { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Message)} = {Message}");

            return sb.ToString();
        }
    }
}
