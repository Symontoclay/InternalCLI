using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateSolutionCopyright
{
    public class UpdateSolutionCopyrightTaskOptions : IObjectToString
    {
        public string SolutionFilePath { get; set; }
        public string Copyright { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Copyright)} = {Copyright}");

            return sb.ToString();
        }
    }
}
