using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public class DeployedItem : IObjectToString
    {
        public KindOfArtifact Kind { get; set; }
        public string FileName { get; set; }
        public bool IsAutomatic { get; set; }
        public string FullFileName { get; set; }
        public string Href { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(FileName)} = {FileName}");
            sb.AppendLine($"{spaces}{nameof(IsAutomatic)} = {IsAutomatic}");
            sb.AppendLine($"{spaces}{nameof(FullFileName)} = {FullFileName}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");

            return sb.ToString();
        }
    }
}
