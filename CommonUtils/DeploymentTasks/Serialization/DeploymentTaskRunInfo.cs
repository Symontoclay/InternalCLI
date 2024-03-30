using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DeploymentTasks.Serialization
{
    public class DeploymentTaskRunInfo : IObjectToString
    {
        public string Key { get; set; }
        public bool? IsFinished { get; set; }
        public List<DeploymentTaskRunInfo> SubTaks { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsFinished)} = {IsFinished}");
            sb.PrintObjListProp(n, nameof(SubTaks), SubTaks);
            return sb.ToString();
        }
    }
}
