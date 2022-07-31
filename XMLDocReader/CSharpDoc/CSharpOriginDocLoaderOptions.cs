using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDocReader.CSharpDoc
{
    public class CSharpOriginDocLoaderOptions : IObjectToString
    {
        public List<string> FileNamesList { get; set; } = new List<string>();
        public bool IgnoreErrors { get; set; }

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

            sb.PrintPODList(n, nameof(FileNamesList), FileNamesList);
            //sb.PrintPODList(n, nameof(TargetRootTypeNamesList), TargetRootTypeNamesList);
            //sb.AppendLine($"{spaces}{nameof(PublicMembersOnly)} = {PublicMembersOnly}");
            sb.AppendLine($"{spaces}{nameof(IgnoreErrors)} = {IgnoreErrors}");

            return sb.ToString();
        }
    }
}
