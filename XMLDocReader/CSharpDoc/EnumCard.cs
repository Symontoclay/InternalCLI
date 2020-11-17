using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class EnumCard: ParentElementCard, ITypeCard
    {
        /// <inheritdoc/>
        public KindOfType KindOfType => KindOfType.Enum;
        public Type Type { get; set; }
        public bool IsPublic { get; set; }

        public List<EnumFieldCard> FieldsList { get; set; } = new List<EnumFieldCard>();

        public bool HasIsInheritdoc
        {
            get
            {
                if (XMLMemberCard != null && XMLMemberCard.IsInheritdoc)
                {
                    return true;
                }

                if (FieldsList != null && FieldsList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInheritdoc))
                {
                    return true;
                }

                return false;
            }
        }

        public bool HasIsInclude
        {
            get
            {
                if (XMLMemberCard != null && XMLMemberCard.IsInclude)
                {
                    return true;
                }

                if (FieldsList != null && FieldsList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInclude))
                {
                    return true;
                }

                return false;
            }
        }

        public string Href { get; set; }
        public string TargetFullFileName { get; set; }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(KindOfType)} = {KindOfType}");
            sb.AppendLine($"{spaces}{nameof(Type)} = {Type?.FullName}");
            sb.AppendLine($"{spaces}{nameof(IsPublic)} = {IsPublic}");
            sb.PrintObjListProp(n, nameof(FieldsList), FieldsList);

            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");
            sb.AppendLine($"{spaces}{nameof(TargetFullFileName)} = {TargetFullFileName}");

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(KindOfType)} = {KindOfType}");
            sb.AppendLine($"{spaces}{nameof(Type)} = {Type?.FullName}");
            sb.AppendLine($"{spaces}{nameof(IsPublic)} = {IsPublic}");

            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");
            sb.AppendLine($"{spaces}{nameof(TargetFullFileName)} = {TargetFullFileName}");

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
