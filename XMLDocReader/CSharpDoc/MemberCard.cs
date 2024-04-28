using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class MemberCard : NamedElementCard, ICodeDocument, IDocFileEditeblePaths
    {
        public ClassCard Parent { get; set; }
        public KindOfMemberAccess KindOfMemberAccess { get; set; } = KindOfMemberAccess.Unknown;

        public string Href { get; set; }
        public string TargetFullFileName { get; set; }

        /// <inheritdoc/>
        public string InitialName => Name.InitialName;

        /// <inheritdoc/>
        public string DisplayedName => Name.DisplayedName;

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.AppendLine($"{spaces}{nameof(KindOfMemberAccess)} = {KindOfMemberAccess}");

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

            sb.AppendLine($"{spaces}{nameof(KindOfMemberAccess)} = {KindOfMemberAccess}");

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
