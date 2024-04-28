using Deployment.Tasks.BuildTasks;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.CopyAndTest
{
    public class CopyAndTestDevTaskOptions : IObjectToString
    {
        public string ProjectOrSoutionFileName { get; set; }
        public KindOfBuildConfiguration BuildConfiguration { get; set; } = KindOfBuildConfiguration.Debug;
        public string OutputDir { get; set; }
        public bool NoLogo { get; set; }
        //https://docs.microsoft.com/ru-ru/dotnet/core/rid-catalog
        public string RuntimeIdentifier { get; set; } = "win-x64";

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

            sb.AppendLine($"{spaces}{nameof(ProjectOrSoutionFileName)} = {ProjectOrSoutionFileName}");
            sb.AppendLine($"{spaces}{nameof(BuildConfiguration)} = {BuildConfiguration}");
            sb.AppendLine($"{spaces}{nameof(OutputDir)} = {OutputDir}");
            sb.AppendLine($"{spaces}{nameof(NoLogo)} = {NoLogo}");
            sb.AppendLine($"{spaces}{nameof(RuntimeIdentifier)} = {RuntimeIdentifier}");

            return sb.ToString();
        }
    }
}
