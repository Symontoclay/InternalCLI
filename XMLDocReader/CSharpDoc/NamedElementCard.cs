using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class NamedElementCard : IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public MemberName Name { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public List<string> ExamplesList { get; set; } = new List<string>();
        public XMLMemberCard XMLMemberCard { get; set; }
        public List<string> ErrorsList { get; set; } = new List<string>();

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
            return PropertiesToString(n);
        }

        /// <inheritdoc/>
        protected virtual string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintObjProp(n, nameof(Name), Name);
            sb.PrintObjProp(n, nameof(XMLMemberCard), XMLMemberCard);
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintPODList(n, nameof(ExamplesList), ExamplesList);
            sb.PrintPODList(n, nameof(ErrorsList), ErrorsList);

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
            return PropertiesToShortString(n);
        }

        /// <inheritdoc/>
        protected virtual string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintObjProp(n, nameof(Name), Name);
            sb.PrintObjProp(n, nameof(XMLMemberCard), XMLMemberCard);
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintPODList(n, nameof(ExamplesList), ExamplesList);

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
            return PropertiesToBriefString(n);
        }

        /// <inheritdoc/>
        protected virtual string PropertiesToBriefString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintObjProp(n, nameof(Name), Name);

            return sb.ToString();
        }
    }
}
