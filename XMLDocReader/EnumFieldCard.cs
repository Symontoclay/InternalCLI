using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader
{
    public class EnumFieldCard : NamedElementCard
    {
        public EnumCard Parent { get; set; }
        public KindOfMemberAccess KindOfMemberAccess { get; set; } = KindOfMemberAccess.Unknown;
        public FieldInfo FieldInfo { get; set; }
        public string Value { get; set; }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.AppendLine($"{spaces}{nameof(KindOfMemberAccess)} = {KindOfMemberAccess}");
            sb.PrintExisting(n, nameof(FieldInfo), FieldInfo);
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(KindOfMemberAccess)} = {KindOfMemberAccess}");
            sb.PrintExisting(n, nameof(FieldInfo), FieldInfo);
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
