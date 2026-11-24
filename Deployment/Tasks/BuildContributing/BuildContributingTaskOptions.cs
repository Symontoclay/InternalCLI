using CSharpUtils;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.Tasks.BuildContributing
{
    public class BuildContributingTaskOptions : IObjectToString
    {
        public string SiteSourcePath { get; set; }
        public string SiteDestPath { get; set; }
        public string SiteName { get; set; }
        public string SourceFileName { get; set; }
        public string TargetFileName { get; set; }
        public string FullUnityVersion { get; set; }
        public (KindOfTargetCSharpFramework Kind, Version Version) DotNetVersion { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(SiteSourcePath)} = {SiteSourcePath}");
            sb.AppendLine($"{spaces}{nameof(SiteDestPath)} = {SiteDestPath}");
            sb.AppendLine($"{spaces}{nameof(SiteName)} = {SiteName}");
            sb.AppendLine($"{spaces}{nameof(SourceFileName)} = {SourceFileName}");
            sb.AppendLine($"{spaces}{nameof(TargetFileName)} = {TargetFileName}");
            sb.AppendLine($"{spaces}{nameof(FullUnityVersion)} = {FullUnityVersion}");
            sb.AppendLine($"{spaces}{nameof(DotNetVersion)} = {DotNetVersion}");

            return sb.ToString();
        }
    }
}
