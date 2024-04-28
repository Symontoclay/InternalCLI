using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class MenuItem : IObjectToString
    {
        public string Href { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");
            sb.AppendLine($"{spaces}{nameof(Label)} = {Label}");

            sb.PrintObjListProp(n, nameof(Items), Items);

            return sb.ToString();
        }
    }
}
