﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace SiteBuilder
{
    public class SiteBuilderOptions : IObjectToString
    {
        public KindOfTargetUrl KindOfTargetUrl { get; set; }
        public string SiteName { get; set; }
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
        public string TempPath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(KindOfTargetUrl)} = {KindOfTargetUrl}");
            sb.AppendLine($"{spaces}{nameof(SiteName)} = {SiteName}");
            sb.AppendLine($"{spaces}{nameof(SourcePath)} = {SourcePath}");
            sb.AppendLine($"{spaces}{nameof(DestPath)} = {DestPath}");
            sb.AppendLine($"{spaces}{nameof(TempPath)} = {TempPath}");

            return sb.ToString();
        }
    }
}
