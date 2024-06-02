using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace BaseDevPipeline.SourceData
{
    public class TempSettingsSource : IObjectToString
    {
        public string Dir { get; set; }
        public bool ClearOnDispose { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Dir)} = {Dir}");
            sb.AppendLine($"{spaces}{nameof(ClearOnDispose)} = {ClearOnDispose}");
            return sb.ToString();
        }
    }
}
