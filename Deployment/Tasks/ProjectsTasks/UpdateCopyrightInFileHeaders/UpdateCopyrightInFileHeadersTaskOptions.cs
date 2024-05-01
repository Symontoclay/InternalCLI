using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders
{
    public class UpdateCopyrightInFileHeadersTaskOptions : IObjectToString
    {
        public string Text { get; set; }
        public List<string> TargetFiles { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Text)} = {Text}");
            sb.PrintPODList(n, nameof(TargetFiles), TargetFiles);

            return sb.ToString();
        }
    }
}
