using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.ReleaseTasks.GitHubRelease
{
    public class GitHubRepositoryInfo : IObjectToString
    {
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(RepositoryOwner)} = {RepositoryOwner}");
            sb.AppendLine($"{spaces}{nameof(RepositoryName)} = {RepositoryName}");

            return sb.ToString();
        }
    }
}
