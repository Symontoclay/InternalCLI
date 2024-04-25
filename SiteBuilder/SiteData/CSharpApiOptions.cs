using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class CSharpApiOptions : IObjectToString
    {
        public List<string> DocFiles { get; set; } = new List<string>();
        public List<string> UnityAssetCoreRootTypes { get; set; } = new List<string>();
        public bool PublicMembersOnly { get; set; }
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

            sb.PrintPODList(n, nameof(DocFiles), DocFiles);
            sb.PrintPODList(n, nameof(UnityAssetCoreRootTypes), UnityAssetCoreRootTypes);
            sb.AppendLine($"{spaces}{nameof(PublicMembersOnly)} = {PublicMembersOnly}");
            sb.AppendLine($"{spaces}{nameof(IgnoreErrors)} = {IgnoreErrors}");

            return sb.ToString();
        }
        
        public static CSharpApiOptions LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<CSharpApiOptions>(File.ReadAllText(path));
        }
    }
}
