﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.GitTasks.CommitAllAndPush
{
    public class CommitAllAndPushTaskOptions : IObjectToString
    {
        public List<string> RepositoryPaths { get; set; }
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

            sb.PrintPODList(n, nameof(RepositoryPaths), RepositoryPaths);
            sb.AppendLine($"{spaces}{nameof(Message)} = {Message}");

            return sb.ToString();
        }
    }
}
