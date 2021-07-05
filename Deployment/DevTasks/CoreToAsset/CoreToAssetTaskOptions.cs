using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToAsset
{
    public class CoreToAssetTaskOptions : IObjectToString
    {
        public string CoreCProjPath { get; set; }
        public string DestDir { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(CoreCProjPath)} = {CoreCProjPath}");
            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");

            return sb.ToString();
        }
    }
}
