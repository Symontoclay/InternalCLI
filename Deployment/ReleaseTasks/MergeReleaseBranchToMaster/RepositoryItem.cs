using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.ReleaseTasks.MergeReleaseBranchToMaster
{
    public class RepositoryItem : IObjectToString
    {
        public string RepositoryPath { get; set; }
        public string TestedProjPath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(RepositoryPath)} = {RepositoryPath}");
            sb.AppendLine($"{spaces}{nameof(TestedProjPath)} = {TestedProjPath}");

            return sb.ToString();
        }
    }
}
