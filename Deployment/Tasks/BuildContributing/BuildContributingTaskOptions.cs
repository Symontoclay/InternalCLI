using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildContributing
{
    public class BuildContributingTaskOptions : IObjectToString
    {
        public string SiteSourcePath { get; set; }
        public string SiteDestPath { get; set; }
        public string SiteName { get; set; }
        public string SourceFileName { get; set; }
        public string TargetFileName { get; set; }

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

            return sb.ToString();
        }
    }
}
