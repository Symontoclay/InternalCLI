using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader
{
    public class MethodCard : MemberCard
    {
        public MethodInfo MethodInfo { get; set; }
        public string Returns { get; set; }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(MethodInfo), MethodInfo);
            sb.AppendLine($"{spaces}{nameof(Returns)} = {Returns}");

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(MethodInfo), MethodInfo);
            sb.AppendLine($"{spaces}{nameof(Returns)} = {Returns}");

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
