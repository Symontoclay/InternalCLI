using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader
{
    public class MemberCard : NamedElementCard
    {
        public ClassCard Parent { get; set; }
        public KindOfMemberAccess KindOfMemberAccess { get; set; } = KindOfMemberAccess.Unknown;

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintObjProp(n, nameof(Parent), Parent);
            sb.AppendLine($"{spaces}{nameof(KindOfMemberAccess)} = {KindOfMemberAccess}");

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
