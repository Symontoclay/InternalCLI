using BaseDevPipeline;
using BaseDevPipeline.Data;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.DevTasks.CoreToAsset
{
    public class CoreToAssetDevTaskOptions : IObjectToString
    {
        public string CoreCProjPath { get; set; }
        public string DestDir { get; set; }
        public List<IProjectSettings> Plugins { get; set; }
        public List<IProjectSettings> CommonPackages { get; set; }

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
            sb.PrintObjListProp(n, nameof(Plugins), Plugins);
            sb.PrintObjListProp(n, nameof(CommonPackages), CommonPackages);

            return sb.ToString();
        }
    }
}
