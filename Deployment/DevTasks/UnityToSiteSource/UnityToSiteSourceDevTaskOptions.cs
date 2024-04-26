using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Text;

namespace Deployment.DevTasks.UnityToSiteSource
{
    public class UnityToSiteSourceDevTaskOptions : IObjectToString
    {
        public string UnitySlnPath { get; set; }
        public string SiteSourceDir { get; set; }
        public string UnityExeFilePath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(UnitySlnPath)} = {UnitySlnPath}");
            sb.AppendLine($"{spaces}{nameof(SiteSourceDir)} = {SiteSourceDir}");
            sb.AppendLine($"{spaces}{nameof(UnityExeFilePath)} = {UnityExeFilePath}");

            return sb.ToString();
        }
    }
}
