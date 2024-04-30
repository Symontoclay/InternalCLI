using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.ExamplesCreator
{
    public class BuildExamplesTaskOptions : IObjectToString
    {
        public List<string> LngExamplesPages { get; set; }
        public string DestDir { get; set; }
        public string CacheDir { get; set; }
        public string SocExePath { get; set; }

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

            sb.PrintPODList(n, nameof(LngExamplesPages), LngExamplesPages);
            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");
            sb.AppendLine($"{spaces}{nameof(CacheDir)} = {CacheDir}");
            sb.AppendLine($"{spaces}{nameof(SocExePath)} = {SocExePath}");

            return sb.ToString();
        }
    }
}
