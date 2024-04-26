using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Text;

namespace Deployment.Helpers
{
    public class ExampleCreatorResult : IObjectToString
    {
        public string ArchFileName { get; set; }
        public string ConsoleFileName { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(ArchFileName)} = {ArchFileName}");
            sb.AppendLine($"{spaces}{nameof(ConsoleFileName)} = {ConsoleFileName}");

            return sb.ToString();
        }
    }
}
