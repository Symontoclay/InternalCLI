using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class MethodParamCard: IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public string Name { get; set; }
        public ParameterInfo ParameterInfo { get; set; }
        public string Summary { get; set; }
        public XMLParamCard XMLParamCard { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.PrintExisting(n, nameof(ParameterInfo), ParameterInfo);
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.PrintObjProp(n, nameof(XMLParamCard), XMLParamCard);

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.PrintExisting(n, nameof(ParameterInfo), ParameterInfo);
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.PrintObjProp(n, nameof(XMLParamCard), XMLParamCard);

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.PrintExisting(n, nameof(ParameterInfo), ParameterInfo);
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.PrintObjProp(n, nameof(XMLParamCard), XMLParamCard);

            return sb.ToString();
        }
    }
}
