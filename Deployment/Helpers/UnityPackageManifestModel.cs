using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public class UnityPackageManifestModel : IObjectToString
    {
#pragma warning disable IDE1006 // Naming Styles
        public string name { get; set; }
        public string version { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string unity { get; set; }
        public string documentationUrl { get; set; }
        public string changelogUrl { get; set; }
        public string licensesUrl { get; set; }
        public Dictionary<string, string> dependencies { get; set; }
        public List<string> keywords { get; set; }
        public AuthorOfUnityPackageManifestModel author { get; set; }
#pragma warning restore IDE1006 // Naming Styles
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

            sb.AppendLine($"{spaces}{nameof(name)} = {name}");
            sb.AppendLine($"{spaces}{nameof(version)} = {version}");
            sb.AppendLine($"{spaces}{nameof(displayName)} = {displayName}");
            sb.AppendLine($"{spaces}{nameof(description)} = {description}");
            sb.AppendLine($"{spaces}{nameof(unity)} = {unity}");
            sb.AppendLine($"{spaces}{nameof(documentationUrl)} = {documentationUrl}");
            sb.AppendLine($"{spaces}{nameof(changelogUrl)} = {changelogUrl}");
            sb.AppendLine($"{spaces}{nameof(licensesUrl)} = {licensesUrl}");
            sb.PrintPODDictProp(n, nameof(dependencies), dependencies);
            sb.PrintPODList(n, nameof(keywords), keywords);
            sb.PrintObjProp(n, nameof(author), author);

            return sb.ToString();
        }
    }
}
