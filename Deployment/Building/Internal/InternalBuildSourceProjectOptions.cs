using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building.Internal
{
    public class InternalBuildSourceProjectOptions : IObjectToString
    {
        public KindOfSourceProject Kind { get; set; } = KindOfSourceProject.Unknown;
        public string ProjectDir { get; set; }
        public string ProjectFullFileName { get; set; }
        public bool IsBuilt { get; set; }
        public string NuGetFullFileName { get; set; }
        public List<string> BuiltFileNamesList { get; set; } = new List<string>();

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
            sb.AppendLine($"{spaces}{nameof(ProjectDir)} = {ProjectDir}");
            sb.AppendLine($"{spaces}{nameof(ProjectFullFileName)} = {ProjectFullFileName}");
            sb.AppendLine($"{spaces}{nameof(IsBuilt)} = {IsBuilt}");
            sb.AppendLine($"{spaces}{nameof(NuGetFullFileName)} = {NuGetFullFileName}");
            sb.PrintPODList(n, nameof(BuiltFileNamesList), BuiltFileNamesList);

            return sb.ToString();
        }
    }
}
