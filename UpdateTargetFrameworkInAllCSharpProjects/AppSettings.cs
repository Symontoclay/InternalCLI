using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    public class AppSettings : IObjectToString
    {
        /// <summary>
        /// true - just shows information without real changes.
        /// false - makes real changes.
        /// </summary>
        public bool ReadOnlyMode { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(ReadOnlyMode)} = {ReadOnlyMode}");

            return sb.ToString();
        }
    }
}
