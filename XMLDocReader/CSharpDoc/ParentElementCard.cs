using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class ParentElementCard: NamedElementCard
    {
        public PackageCard Package { get; set; }
        public ParentElementCard Parent { get; set; }

        public List<ClassCard> ClassesList { get; set; } = new List<ClassCard>();
        public List<ClassCard> InterfacesList { get; set; } = new List<ClassCard>();
        public List<EnumCard> EnumsList { get; set; } = new List<EnumCard>();

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintBriefObjProp(n, nameof(Package), Package);
            sb.PrintBriefObjProp(n, nameof(Parent), Parent);
            sb.PrintObjListProp(n, nameof(ClassesList), ClassesList);
            sb.PrintObjListProp(n, nameof(InterfacesList), InterfacesList);
            sb.PrintObjListProp(n, nameof(EnumsList), EnumsList);

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(Package), Package);
            sb.PrintExisting(n, nameof(Parent), Parent);
            sb.PrintShortObjListProp(n, nameof(ClassesList), ClassesList);
            sb.PrintShortObjListProp(n, nameof(InterfacesList), InterfacesList);
            sb.PrintShortObjListProp(n, nameof(EnumsList), EnumsList);

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}

/*
        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

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
*/