using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateUnityPackageVersion
{
    public class UpdateUnityPackageVersionTaskOptions : IObjectToString
    {
        public string PackageSourcePath { get; set; }
        public string Version { get; set; }
        public string UnityVersion { get; set; }
        public string UnityRelease { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(PackageSourcePath)} = {PackageSourcePath}");
            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(UnityVersion)} = {UnityVersion}");
            sb.AppendLine($"{spaces}{nameof(UnityRelease)} = {UnityRelease}");

            return sb.ToString();
        }
    }
}
