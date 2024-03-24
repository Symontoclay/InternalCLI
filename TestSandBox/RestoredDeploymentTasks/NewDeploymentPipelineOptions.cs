using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class NewDeploymentPipelineOptions : IObjectToString
    {
        public bool UseAutorestoring { get; set; }
        public string DirectoryForAutorestoring { get; set; }
        public bool StartFromBeginning { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(UseAutorestoring)} = {UseAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(DirectoryForAutorestoring)} = {DirectoryForAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(StartFromBeginning)} = {StartFromBeginning}");
            return sb.ToString();
        }
    }
}
