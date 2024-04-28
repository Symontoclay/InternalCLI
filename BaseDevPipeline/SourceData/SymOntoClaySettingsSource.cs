using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace BaseDevPipeline.SourceData
{
    public class SymOntoClaySettingsSource : IObjectToString
    {
        public List<string> BasePaths { get; set; }
        public List<string> SecretsFilePaths { get; set; }        
        public List<string> ArtifactsForDeployment { get; set; }
        public string RepositoryReadmeSource { get; set; }
        public string RepositoryBadgesSource { get; set; }
        public string InternalCLIDist { get; set; }
        public List<SolutionSource> Solutions { get; set; }
        public List<ArtifactDest> DevArtifacts { get; set; }
        public List<LicenseSource> Licenses { get; set; }
        public string Copyright { get; set; }

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
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.AppendLine($"{spaces}{nameof(RepositoryReadmeSource)} = {RepositoryReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(RepositoryBadgesSource)} = {RepositoryBadgesSource}");
            sb.AppendLine($"{spaces}{nameof(InternalCLIDist)} = {InternalCLIDist}");
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(DevArtifacts), DevArtifacts);
            sb.PrintObjListProp(n, nameof(Licenses), Licenses);
            sb.AppendLine($"{spaces}{nameof(Copyright)} = {Copyright}");

            return sb.ToString();
        }
    }
}
