using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    public class ConsoleWrapperSettings : IObjectToString
    {
        public bool WriteOutputToTextFileAsParallel { get; set; }
        public bool UseNLogLogger { get; set; }
        public bool WriteCopyright { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(WriteOutputToTextFileAsParallel)} = {WriteOutputToTextFileAsParallel}");
            sb.AppendLine($"{spaces}{nameof(UseNLogLogger)} = {UseNLogLogger}");
            sb.AppendLine($"{spaces}{nameof(WriteCopyright)} = {WriteCopyright}");

            return sb.ToString();
        }
    }
}
