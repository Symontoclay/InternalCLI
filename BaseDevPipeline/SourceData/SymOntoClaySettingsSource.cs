using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.SourceData
{
    public class SymOntoClaySettingsSource : IObjectToString
    {
        public List<string> BasePaths { get; set; }
        public List<string> SecretsFilePaths { get; set; }
        public List<string> UnityPaths { get; set; }
        public List<string> ArtifactsForDeployment { get; set; }
        public List<SolutionSource> Solutions { get; set; }
        public List<ArtifactDest> Artifacts { get; set; }
        public List<LicenseSource> Licenses { get; set; }

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

            sb.PrintPODList(n, nameof(BasePaths), BasePaths);
            sb.PrintPODList(n, nameof(SecretsFilePaths), SecretsFilePaths);
            sb.PrintPODList(n, nameof(UnityPaths), UnityPaths);
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(Artifacts), Artifacts);
            sb.PrintObjListProp(n, nameof(Licenses), Licenses);

            return sb.ToString();
        }
    }
}
