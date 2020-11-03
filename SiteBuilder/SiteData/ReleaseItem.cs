using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class ReleaseItem : IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public DateTime? Date { get; set; }
        public string Version { get; set; }
        public List<ReleaseAssetItem> AssetsList { get; set; } = new List<ReleaseAssetItem>();
        public string Description { get; set; }
        public bool IsMarkdown { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Date)} = {Date}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");

            sb.PrintObjListProp(n, nameof(AssetsList), AssetsList);

            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(IsMarkdown)} = {IsMarkdown}");

            return sb.ToString();
        }

        /// <inheritdoc/>
        public string ToShortString()
        {
            return ToShortString(0u);
        }

        /// <inheritdoc/>
        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToShortString.PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Date)} = {Date}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");

            sb.PrintShortObjListProp(n, nameof(AssetsList), AssetsList);

            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(IsMarkdown)} = {IsMarkdown}");

            return sb.ToString();
        }

        /// <inheritdoc/>
        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        /// <inheritdoc/>
        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToBriefString.PropertiesToBriefString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Date)} = {Date}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");

            return sb.ToString();
        }
    }
}
