using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Helpers
{
    public class AuthorOfUnityPackageManifestModel : IObjectToString
    {
#pragma warning disable IDE1006 // Naming Styles
        public string name { get; set; }
        public string email { get; set; }
        public string url { get; set; }
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
            sb.AppendLine($"{spaces}{nameof(email)} = {email}");
            sb.AppendLine($"{spaces}{nameof(url)} = {url}");

            return sb.ToString();
        }
    }
}
