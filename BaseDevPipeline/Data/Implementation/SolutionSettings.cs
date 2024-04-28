using CommonUtils.DebugHelpers;
using CSharpUtils;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.Text;

namespace BaseDevPipeline.Data.Implementation
{
    public class SolutionSettings: ISolutionSettings
    {
        public string Name { get; set; }
        public KindOfProject Kind { get; set; }
        public string Href { get; set; }
        public string GitFileHref { get; set; }
        public string RepositoryName { get; set; }
        public string OwnerName { get; set; }
        public string Path { get; set; }
        public string SlnPath { get; set; }
        public string SourcePath { get; set; }
        public string BuiltNuGetPackages { get; set; }

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
        public string CodeOfConductSource { get; set; }
        public string ContributingSource { get; set; }
        public string FullUnityVersion { get; set; } = string.Empty;
        public string UnityVersion { get; set; } = string.Empty;
        public string UnityRelease { get; set; } = string.Empty;

        /// <inheritdoc/>
        public void RereadUnityVersion()
        {
            if (Kind == KindOfProject.Unity || Kind == KindOfProject.UnityExample)
            {
                var targetUnityVersion = UnityHelper.GetTargetUnityVersion(Path);

                if (!string.IsNullOrWhiteSpace(targetUnityVersion))
                {
                    FullUnityVersion = targetUnityVersion;

                    var lastPointPos = targetUnityVersion.IndexOf(".", targetUnityVersion.IndexOf(".") + 1);

                    UnityVersion = targetUnityVersion.Substring(0, lastPointPos);
                    UnityRelease = targetUnityVersion.Substring(lastPointPos + 1);
                }
            }
        }

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Href)} = {Href}");
            sb.AppendLine($"{spaces}{nameof(GitFileHref)} = {GitFileHref}");
            sb.AppendLine($"{spaces}{nameof(RepositoryName)} = {RepositoryName}");
            sb.AppendLine($"{spaces}{nameof(OwnerName)} = {OwnerName}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(SlnPath)} = {SlnPath}");
            sb.AppendLine($"{spaces}{nameof(SourcePath)} = {SourcePath}");
            sb.AppendLine($"{spaces}{nameof(BuiltNuGetPackages)} = {BuiltNuGetPackages}");
            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.AppendLine($"{spaces}{nameof(LicenseName)} = {LicenseName}");
            sb.PrintObjProp(n, nameof(License), License);
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.AppendLine($"{spaces}{nameof(EnableGenerateReadme)} = {EnableGenerateReadme}");
            sb.AppendLine($"{spaces}{nameof(ReadmeSource)} = {ReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(BadgesSource)} = {BadgesSource}");
            sb.AppendLine($"{spaces}{nameof(CodeOfConductSource)} = {CodeOfConductSource}");
            sb.AppendLine($"{spaces}{nameof(ContributingSource)} = {ContributingSource}");
            sb.AppendLine($"{spaces}{nameof(FullUnityVersion)} = {FullUnityVersion}");
            sb.AppendLine($"{spaces}{nameof(UnityVersion)} = {UnityVersion}");
            sb.AppendLine($"{spaces}{nameof(UnityRelease)} = {UnityRelease}");

            return sb.ToString();
        }
    }
}
