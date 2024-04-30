using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.Helpers
{
    public class GitRepositoryFileInfo : IObjectToString
    {
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
        public GitRepositoryFileStatus Status { get; set; }
        public bool IsNew => Status == GitRepositoryFileStatus.Added || Status == GitRepositoryFileStatus.Untracked;

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

            sb.AppendLine($"{spaces}{nameof(AbsolutePath)} = {AbsolutePath}");
            sb.AppendLine($"{spaces}{nameof(RelativePath)} = {RelativePath}");
            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(IsNew)} = {IsNew}");

            return sb.ToString();
        }
    }
}
