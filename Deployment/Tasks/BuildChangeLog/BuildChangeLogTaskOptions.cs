using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Tasks.BuildChangeLog
{
    public class BuildChangeLogTaskOptions : IObjectToString
    {
        public string ReleaseNotesFilePath { get; set; }
        public string TargetChangeLogFileName { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(ReleaseNotesFilePath)} = {ReleaseNotesFilePath}");
            sb.AppendLine($"{spaces}{nameof(TargetChangeLogFileName)} = {TargetChangeLogFileName}");

            return sb.ToString();
        }
    }
}
