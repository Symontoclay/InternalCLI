using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.Text;

namespace BaseDevPipeline.SourceData
{
    public class SolutionSource : IObjectToString
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Href { get; set; }
        public string GitFileHref { get; set; }
        public string Path { get; set; }
        /// <summary>
        /// This filed should be used if Sln has non standard name and can not be detected automatically.
        /// </summary>
        public string SlnPath { get; set; }
        public string License { get; set; }
        public string SourcePath { get; set; }
        public string BuiltNuGetPackages { get; set; }
        public bool EnableGenerateReadme { get; set; }
        public string ReadmeSource { get; set; }
        public string BadgesSource { get; set; }
        public bool IsCommonReadmeSource { get; set; }
        public string CommonReadmeSource { get; set; }
        public string CommonBadgesSource { get; set; }
        public string CodeOfConductSource { get; set; }
        public string ContributingSource { get; set; }
        public List<ProjectSource> Projects { get; set; }
        public List<string> ArtifactsForDeployment { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(SlnPath)} = {SlnPath}");
            sb.AppendLine($"{spaces}{nameof(License)} = {License}");
            sb.AppendLine($"{spaces}{nameof(SourcePath)} = {SourcePath}");
            sb.AppendLine($"{spaces}{nameof(BuiltNuGetPackages)} = {BuiltNuGetPackages}");
            sb.AppendLine($"{spaces}{nameof(EnableGenerateReadme)} = {EnableGenerateReadme}");
            sb.AppendLine($"{spaces}{nameof(ReadmeSource)} = {ReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(BadgesSource)} = {BadgesSource}");
            sb.AppendLine($"{spaces}{nameof(IsCommonReadmeSource)} = { IsCommonReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(CommonReadmeSource)} = {CommonReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(CommonBadgesSource)} = {CommonBadgesSource}");
            sb.AppendLine($"{spaces}{nameof(CodeOfConductSource)} = {CodeOfConductSource}");
            sb.AppendLine($"{spaces}{nameof(ContributingSource)} = {ContributingSource}");

            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);

            return sb.ToString();
        }
    }
}
