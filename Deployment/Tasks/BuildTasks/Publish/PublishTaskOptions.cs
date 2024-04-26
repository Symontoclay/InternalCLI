using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildTasks.Publish
{
    public class PublishTaskOptions : IObjectToString
    {
        public string ProjectOrSoutionFileName { get; set; }
        public KindOfBuildConfiguration BuildConfiguration { get; set; } = KindOfBuildConfiguration.Debug;
        public string OutputDir { get; set; }
        public bool NoLogo { get; set; }
        //https://docs.microsoft.com/ru-ru/dotnet/core/rid-catalog
        public string RuntimeIdentifier { get; set; } = "win-x64";
        public bool SelfContained { get; set; } = true;

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
            sb.AppendLine($"{spaces}{nameof(SelfContained)} = {SelfContained}");

            return sb.ToString();
        }
    }
}
