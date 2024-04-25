using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class BreadcrumbInThePage : IObjectToString
    {
        public string Title { get; set; }
        public string Href { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");

            return sb.ToString();
        }
    }
}
