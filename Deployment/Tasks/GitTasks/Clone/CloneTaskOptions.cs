using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.Clone
{
    public class CloneTaskOptions : IObjectToString
    {
        public string RepositoryHref { get; set; }
        public string RepositoryPath { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(RepositoryHref)} = {RepositoryHref}");
            sb.AppendLine($"{spaces}{nameof(RepositoryPath)} = {RepositoryPath}");

            return sb.ToString();
        }
    }
}
