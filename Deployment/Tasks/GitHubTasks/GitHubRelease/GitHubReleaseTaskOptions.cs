using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.GitHubTasks.GitHubRelease
{
    public class GitHubReleaseTaskOptions : IObjectToString
    {
        public string Token { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
        public string Version { get; set; }
        public string NotesText { get; set; }
        public bool Draft { get; set; }
        public bool Prerelease { get; set; }
        public List<GitHubReleaseAssetOptions> Assets { get; set; }

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

            sb.PrintExistingStr(n, nameof(Token), Token);
            sb.AppendLine($"{spaces}{nameof(RepositoryOwner)} = {RepositoryOwner}");
            sb.AppendLine($"{spaces}{nameof(RepositoryName)} = {RepositoryName}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");            
            sb.PrintPODProp(n, nameof(NotesText), NotesText);
            sb.AppendLine($"{spaces}{nameof(Draft)} = {Draft}");
            sb.AppendLine($"{spaces}{nameof(Prerelease)} = {Prerelease}");
            sb.PrintObjListProp(n, nameof(Assets), Assets);

            return sb.ToString();
        }
    }
}
