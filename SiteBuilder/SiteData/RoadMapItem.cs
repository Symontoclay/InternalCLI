using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class RoadMapItem : IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public KindOfRoadMapItem Kind { get; set; } = KindOfRoadMapItem.Unknown;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? ExpectedDuration { get; set; }
        public KindOfDuration KindOfExpectedDuration { get; set; } = KindOfDuration.Unknown;
        public KindOfRoadMapItemCompeltion KindOfCompeltion { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SeeMoreHref { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsMarkdown { get; set; }
        public RoadMapItem Parent { get; set; }
        public List<RoadMapItem> SubItemsList { get; set; } = new List<RoadMapItem>();

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
            sb.AppendLine($"{spaces}{nameof(Start)} = {Start}");
            sb.AppendLine($"{spaces}{nameof(End)} = {End}");
            sb.AppendLine($"{spaces}{nameof(ExpectedDuration)} = {ExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfExpectedDuration)} = {KindOfExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfCompeltion)} = {KindOfCompeltion}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(ShortDescription)} = {ShortDescription}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(SeeMoreHref)} = {SeeMoreHref}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintPODList(n, nameof(Tags), Tags);
            sb.AppendLine($"{spaces}{nameof(IsMarkdown)} = {IsMarkdown}");

            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.PrintObjListProp(n, nameof(SubItemsList), SubItemsList);

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

            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Start)} = {Start}");
            sb.AppendLine($"{spaces}{nameof(End)} = {End}");
            sb.AppendLine($"{spaces}{nameof(ExpectedDuration)} = {ExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfExpectedDuration)} = {KindOfExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfCompeltion)} = {KindOfCompeltion}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(ShortDescription)} = {ShortDescription}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(SeeMoreHref)} = {SeeMoreHref}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintPODList(n, nameof(Tags), Tags);
            sb.AppendLine($"{spaces}{nameof(IsMarkdown)} = {IsMarkdown}");

            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.PrintShortObjListProp(n, nameof(SubItemsList), SubItemsList);

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

            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Start)} = {Start}");
            sb.AppendLine($"{spaces}{nameof(End)} = {End}");
            sb.AppendLine($"{spaces}{nameof(ExpectedDuration)} = {ExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfExpectedDuration)} = {KindOfExpectedDuration}");
            sb.AppendLine($"{spaces}{nameof(KindOfCompeltion)} = {KindOfCompeltion}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(ShortDescription)} = {ShortDescription}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(SeeMoreHref)} = {SeeMoreHref}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintPODList(n, nameof(Tags), Tags);
            sb.AppendLine($"{spaces}{nameof(IsMarkdown)} = {IsMarkdown}");

            sb.PrintExisting(n, nameof(Parent), Parent);
            sb.PrintExistingList(n, nameof(SubItemsList), SubItemsList);

            return sb.ToString();
        }
    }
}
