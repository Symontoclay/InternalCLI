﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.UpdateUnityExampleRepository
{
    public class UpdateUnityExampleRepositoryDevTaskOptions : IObjectToString
    {
        public string SourceRepository { get; set; }
        public string DestinationRepository { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(SourceRepository)} = {SourceRepository}");
            sb.AppendLine($"{spaces}{nameof(DestinationRepository)} = {DestinationRepository}");

            return sb.ToString();
        }
    }
}
