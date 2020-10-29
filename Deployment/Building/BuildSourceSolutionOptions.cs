using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public class BuildSourceSolutionOptions : IObjectToString
    {
        public string SolutionDir { get; set; }
        public List<BuildSourceProjectOptions> ProjectsOptions { get; set; } = new List<BuildSourceProjectOptions>();

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

            sb.AppendLine($"{spaces}{nameof(SolutionDir)} = {SolutionDir}");
            sb.PrintObjListProp(n, nameof(ProjectsOptions), ProjectsOptions);

            return sb.ToString();
        }
    }
}
