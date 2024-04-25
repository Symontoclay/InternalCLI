using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.CodeHighlighting
{
    public class CodeExample: IObjectToString
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool UseNLP { get; set; }
        public List<string> SharedLibs { get; set; } = new List<string>();

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Code)} = {Code}");
            sb.AppendLine($"{spaces}{nameof(UseNLP)} = {UseNLP}");
            sb.PrintPODList(n, nameof(SharedLibs), SharedLibs);

            return sb.ToString();
        }
    }
}
