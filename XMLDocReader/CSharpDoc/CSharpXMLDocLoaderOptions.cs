using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class CSharpXMLDocLoaderOptions : IObjectToString
    {
        public List<string> XmlFileNamesList { get; set; } = new List<string>();
        public List<string> TargetRootTypeNamesList { get; set; } = new List<string>();
        public bool PublicMembersOnly { get; set; }
        public bool IgnoreErrors { get; set; }
        public string BaseHref { get; set; }
        public string BasePath { get; set; }
        public string SourceDir { get; set; }
        public string DestDir { get; set; }

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

            sb.PrintPODList(n, nameof(XmlFileNamesList), XmlFileNamesList);
            sb.PrintPODList(n, nameof(TargetRootTypeNamesList), TargetRootTypeNamesList);
            sb.AppendLine($"{spaces}{nameof(PublicMembersOnly)} = {PublicMembersOnly}");
            sb.AppendLine($"{spaces}{nameof(IgnoreErrors)} = {IgnoreErrors}");
            sb.AppendLine($"{spaces}{nameof(BaseHref)} = {BaseHref}");
            sb.AppendLine($"{spaces}{nameof(BasePath)} = {BasePath}");
            sb.AppendLine($"{spaces}{nameof(SourceDir)} = {SourceDir}");
            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");

            return sb.ToString();
        }
    }
}
