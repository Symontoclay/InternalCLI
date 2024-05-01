using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution
{
    public class UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions : IObjectToString
    {
        public string SourceDir { get; set; }
        public string Text { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(SourceDir)} = {SourceDir}");

            return sb.ToString();
        }
    }
}
