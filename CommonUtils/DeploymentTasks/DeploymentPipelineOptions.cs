using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace CommonUtils.DeploymentTasks
{
    public class DeploymentPipelineOptions : IObjectToString
    {
        public bool UseAutorestoring { get; set; }
        public string DirectoryForAutorestoring { get; set; }
        public bool StartFromBeginning { get; set; }
        public string Prefix { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Prefix)} = {Prefix}");
            return sb.ToString();
        }
    }
}
