﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class RoadMapItem : IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public string Title { get; set; }
        public string PeriodOfCompletion { get; set; }
        public string Description { get; set; }
        public bool IsMarkDown { get; set; }
        public string HrefWithDetailedInfomation { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(PeriodOfCompletion)} = {PeriodOfCompletion}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(IsMarkDown)} = {IsMarkDown}");
            sb.AppendLine($"{spaces}{nameof(HrefWithDetailedInfomation)} = {HrefWithDetailedInfomation}");

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

            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(PeriodOfCompletion)} = {PeriodOfCompletion}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(IsMarkDown)} = {IsMarkDown}");
            sb.AppendLine($"{spaces}{nameof(HrefWithDetailedInfomation)} = {HrefWithDetailedInfomation}");

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

            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(PeriodOfCompletion)} = {PeriodOfCompletion}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(IsMarkDown)} = {IsMarkDown}");
            sb.AppendLine($"{spaces}{nameof(HrefWithDetailedInfomation)} = {HrefWithDetailedInfomation}");

            return sb.ToString();
        }
    }
}
