using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader
{
    public class ClassCard: ParentElementCard, ITypeCard
    {
        /// <inheritdoc/>
        public KindOfType KindOfType { get; set; } = KindOfType.Unknown;
        public Type Type { get; set; }
        public bool IsPublic { get; set; }

        public List<PropertyCard> PropertiesList { get; set; } = new List<PropertyCard>();
        public List<MethodCard> MethodsList { get; set; } = new List<MethodCard>();

        public bool HasIsInheritdoc
        {
            get
            {
                if(XMLMemberCard != null && XMLMemberCard.IsInheritdoc)
                {
                    return true;
                }

                if(PropertiesList != null && PropertiesList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInheritdoc))
                {
                    return true;
                }

                if(MethodsList != null && MethodsList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInheritdoc))
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

                if (PropertiesList != null && PropertiesList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInclude))
                {
                    return true;
                }

                if (MethodsList != null && MethodsList.Any(p => p.XMLMemberCard != null && p.XMLMemberCard.IsInclude))
                {
                    return true;
                }

                return false;
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(KindOfType)} = {KindOfType}");
            sb.AppendLine($"{spaces}{nameof(Type)} = {Type.FullName}");
            sb.AppendLine($"{spaces}{nameof(IsPublic)} = {IsPublic}");

            sb.PrintObjListProp(n, nameof(PropertiesList), PropertiesList);
            sb.PrintObjListProp(n, nameof(MethodsList), MethodsList);

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(KindOfType)} = {KindOfType}");
            sb.AppendLine($"{spaces}{nameof(Type)} = {Type.FullName}");
            sb.AppendLine($"{spaces}{nameof(IsPublic)} = {IsPublic}");

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
