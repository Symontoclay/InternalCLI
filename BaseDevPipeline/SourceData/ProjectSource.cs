using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace BaseDevPipeline.SourceData
{
    public class ProjectSource : IObjectToString
    {
        public string Kind { get; set; }
        public string Path { get; set; }
        /// <summary>
        /// This filed should be used if CsProj has non standard name and can not be detected automatically.
        /// </summary>
        public string CsProjPath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(CsProjPath)} = {CsProjPath}");

            return sb.ToString();
        }
    }
}
