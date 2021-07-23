using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitHubTasks.GitHubRelease
{
    public class GitHubReleaseAssetOptions : IObjectToString
    {
        public string DisplayedName { get; set; }
        public string UploadedFilePath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(DisplayedName)} = {DisplayedName}");
            sb.AppendLine($"{spaces}{nameof(UploadedFilePath)} = {UploadedFilePath}");

            return sb.ToString();
        }
    }
}
