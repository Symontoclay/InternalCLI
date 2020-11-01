using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class SiteElementInfo: IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public KindOfSiteElement Kind { get; set; } = KindOfSiteElement.Unknown;
        public string InitialFullFileName { get; set; }
        public string THtmlFullFileName { get; set; }
        public string TargetFullFileName { get; set; }
        public string Href { get; set; }
        public SiteElementInfo Parent { get; set; }
        public List<SiteElementInfo> SubItemsList { get; set; } = new List<SiteElementInfo>();

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

            sb.AppendLine($"{spaces}{nameof(InitialFullFileName)} = {InitialFullFileName}");
            sb.AppendLine($"{spaces}{nameof(THtmlFullFileName)} = {THtmlFullFileName}");
            sb.AppendLine($"{spaces}{nameof(TargetFullFileName)} = {TargetFullFileName}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");

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

            sb.AppendLine($"{spaces}{nameof(InitialFullFileName)} = {InitialFullFileName}");
            sb.AppendLine($"{spaces}{nameof(THtmlFullFileName)} = {THtmlFullFileName}");
            sb.AppendLine($"{spaces}{nameof(TargetFullFileName)} = {TargetFullFileName}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");

            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.PrintBriefObjListProp(n, nameof(SubItemsList), SubItemsList);

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

            sb.AppendLine($"{spaces}{nameof(InitialFullFileName)} = {InitialFullFileName}");
            sb.AppendLine($"{spaces}{nameof(THtmlFullFileName)} = {THtmlFullFileName}");
            sb.AppendLine($"{spaces}{nameof(TargetFullFileName)} = {TargetFullFileName}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");

            sb.PrintExisting(n, nameof(Parent), Parent);
            sb.PrintExistingList(n, nameof(SubItemsList), SubItemsList);

            return sb.ToString();
        }
    }
}
