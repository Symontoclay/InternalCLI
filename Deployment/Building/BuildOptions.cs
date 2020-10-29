using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public class BuildOptions : IObjectToString
    {
        public List<BuildSourceSolutionOptions> SolutionOptions { get; set; } = new List<BuildSourceSolutionOptions>();

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

            sb.PrintObjListProp(n, nameof(SolutionOptions), SolutionOptions);

            return sb.ToString();
        }
    }
}
