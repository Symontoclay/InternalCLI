using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public class ProjectSettings: IProjectSettings
    {
        public SolutionSettings Solution { get; set; }

        /// <inheritdoc/>
        ISolutionSettings IProjectSettings.Solution => Solution;

        public KindOfProjectSource Kind { get; set; }
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
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(CsProjPath)} = {CsProjPath}");
            sb.AppendLine($"{spaces}{nameof(LicenseName)} = {LicenseName}");
            sb.PrintObjProp(n, nameof(License), License);

            return sb.ToString();
        }
    }
}
