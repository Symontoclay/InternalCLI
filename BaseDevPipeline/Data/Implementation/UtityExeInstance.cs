using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace BaseDevPipeline.Data.Implementation
{
    public class UtityExeInstance: IUtityExeInstance
    {
        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public string Path { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");

            return sb.ToString();
        }
    }
}
