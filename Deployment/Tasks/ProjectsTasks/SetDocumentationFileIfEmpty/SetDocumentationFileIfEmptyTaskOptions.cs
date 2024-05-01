﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.SetDocumentationFileIfEmpty
{
    public class SetDocumentationFileIfEmptyTaskOptions : IObjectToString
    {
        public string ProjectFilePath { get; set; }
        public string DocumentationFilePath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(ProjectFilePath)} = {ProjectFilePath}");
            sb.AppendLine($"{spaces}{nameof(DocumentationFilePath)} = {DocumentationFilePath}");

            return sb.ToString();
        }
    }
}
