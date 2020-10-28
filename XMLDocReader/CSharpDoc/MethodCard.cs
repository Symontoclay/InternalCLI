using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class MethodCard : MemberCard
    {
        public MethodInfo MethodInfo { get; set; }
        public string Returns { get; set; }
        public NamedElementCard ReturnsTypeCard { get; set; }
        public MemberName ReturnsTypeName { get; set; }
        public List<NamedElementCard> UsedTypesList { get; set; } = new List<NamedElementCard>();
        public List<MethodParamCard> ParamsList { get; set; } = new List<MethodParamCard>();

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(MethodInfo), MethodInfo);
            sb.AppendLine($"{spaces}{nameof(Returns)} = {Returns}");
            sb.PrintBriefObjProp(n, nameof(ReturnsTypeCard), ReturnsTypeCard);
            sb.PrintObjProp(n, nameof(ReturnsTypeName), ReturnsTypeName);
            sb.PrintObjListProp(n, nameof(UsedTypesList), UsedTypesList);

            //sb.PrintObjListProp(n, nameof(TypeParamsList), TypeParamsList);
            sb.PrintObjListProp(n, nameof(ParamsList), ParamsList);

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

            //sb.PrintShortObjListProp(n, nameof(TypeParamsList), TypeParamsList);
            sb.PrintShortObjListProp(n, nameof(ParamsList), ParamsList);

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
