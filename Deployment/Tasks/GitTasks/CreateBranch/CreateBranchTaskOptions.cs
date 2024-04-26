using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.CreateBranch
{
    public class CreateBranchTaskOptions : IObjectToString
    {
        public string RepositoryPath { get; set; }
        public string BranchName { get; set; }

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

            return sb.ToString();
        }
    }
}
