using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public class SolutionSettings: ISolutionSettings
    {
        public KindOfProject Kind { get; set; }
        public string Href { get; set; }
        public string GitFileHref { get; set; }
        public string RepositoryName { get; set; }
        public string OwnerName { get; set; }
        public string Path { get; set; }
        public string SlnPath { get; set; }
        public string SourcePath { get; set; }

        public List<ProjectSettings> Projects { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IProjectSettings> ISolutionSettings.Projects => Projects;

        public string LicenseName { get; set; }

        public LicenseSettings License { get; set; }

        /// <inheritdoc/>
        ILicenseSettings ISolutionSettings.License => License;

        public List<KindOfArtifact> ArtifactsForDeployment { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<KindOfArtifact> ISolutionSettings.ArtifactsForDeployment => ArtifactsForDeployment;

        public bool EnableGenerateReadme { get; set; }
        public string ReadmeSource { get; set; }
        public string BadgesSource { get; set; }
        public bool IsCommonReadmeSource { get; set; }
        public string CommonReadmeSource { get; set; }
        public string CommonBadgesSource { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");
            sb.AppendLine($"{spaces}{nameof(GitFileHref)} = {GitFileHref}");
            sb.AppendLine($"{spaces}{nameof(RepositoryName)} = {RepositoryName}");
            sb.AppendLine($"{spaces}{nameof(OwnerName)} = {OwnerName}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(SlnPath)} = {SlnPath}");
            sb.AppendLine($"{spaces}{nameof(SourcePath)} = {SourcePath}");
            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.AppendLine($"{spaces}{nameof(LicenseName)} = {LicenseName}");
            sb.PrintObjProp(n, nameof(License), License);
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.AppendLine($"{spaces}{nameof(EnableGenerateReadme)} = {EnableGenerateReadme}");
            sb.AppendLine($"{spaces}{nameof(ReadmeSource)} = {ReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(BadgesSource)} = {BadgesSource}");

            return sb.ToString();
        }
    }
}
