using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class PackageCardReaderSettings : IObjectToString
    {
        public string XMLDocFileName { get; set; }
        public string AssemblyFileName { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(XMLDocFileName)} = {XMLDocFileName}");
            sb.AppendLine($"{spaces}{nameof(AssemblyFileName)} = {AssemblyFileName}");

            return sb.ToString();
        }
    }
}
