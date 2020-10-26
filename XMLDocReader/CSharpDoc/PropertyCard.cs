using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class PropertyCard: MemberCard
    {
        public PropertyInfo PropertyInfo { get; set; }
        public NamedElementCard PropertyTypeCard { get; set; }
        public string Value { get; set; }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(PropertyInfo), PropertyInfo);
            sb.PrintBriefObjProp(n, nameof(PropertyTypeCard), PropertyTypeCard);
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(PropertyInfo), PropertyInfo);
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
