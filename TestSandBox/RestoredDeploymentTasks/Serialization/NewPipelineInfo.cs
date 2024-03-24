using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks.Serialization
{
    public class NewPipelineInfo : IObjectToString
    {
        public bool? IsFinished { get; set; }
        public string LastRunInfo { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(IsFinished)} = {IsFinished}");
            sb.AppendLine($"{spaces}{nameof(LastRunInfo)} = {LastRunInfo}");
            return sb.ToString();
        }
    }
}
