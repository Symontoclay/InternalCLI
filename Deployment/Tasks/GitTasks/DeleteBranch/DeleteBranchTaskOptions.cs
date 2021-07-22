using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.DeleteBranch
{
    public class DeleteBranchTaskOptions : IObjectToString
    {
        public string RepositoryPath { get; set; }
        public string BranchName { get; set; }
        public bool IsOrigin { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(BranchName)} = {BranchName}");
            sb.AppendLine($"{spaces}{nameof(IsOrigin)} = {IsOrigin}");

            return sb.ToString();
        }
    }
}
