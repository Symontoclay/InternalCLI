using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public class BuildSourceProjectOptions : IObjectToString
    {
        public KindOfSourceProject Kind { get; set; } = KindOfSourceProject.Unknown;
        public string ProjectDir { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(ProjectDir)} = {ProjectDir}");

            return sb.ToString();
        }
    }
}
