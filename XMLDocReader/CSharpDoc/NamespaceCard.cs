using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class NamespaceCard: ParentElementCard
    {
        public List<NamespaceCard> NamespacesList { get; set; } = new List<NamespaceCard>();

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();
         
            sb.PrintObjListProp(n, nameof(NamespacesList), NamespacesList);

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();
            
            sb.PrintShortObjListProp(n, nameof(NamespacesList), NamespacesList);

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
