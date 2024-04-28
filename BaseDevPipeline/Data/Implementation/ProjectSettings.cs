using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace BaseDevPipeline.Data.Implementation
{
    public class ProjectSettings: IProjectSettings
    {
        public SolutionSettings Solution { get; set; }

        /// <inheritdoc/>
        ISolutionSettings IProjectSettings.Solution => Solution;

        public KindOfProject Kind { get; set; }

        public string FolderName { get; set; }
        public string Path { get; set; }
        public string CsProjPath { get; set; }

        public string LicenseName { get; set; }

        public LicenseSettings License { get; set; }

        /// <inheritdoc/>
        ILicenseSettings IProjectSettings.License => License;

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

            sb.PrintExisting(n, nameof(Solution), Solution);
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(FolderName)} = {FolderName}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(CsProjPath)} = {CsProjPath}");
            sb.AppendLine($"{spaces}{nameof(LicenseName)} = {LicenseName}");
            sb.PrintObjProp(n, nameof(License), License);

            return sb.ToString();
        }
    }
}
