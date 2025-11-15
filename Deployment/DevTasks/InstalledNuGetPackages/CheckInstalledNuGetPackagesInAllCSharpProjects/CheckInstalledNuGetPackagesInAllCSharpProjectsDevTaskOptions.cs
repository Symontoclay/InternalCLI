using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects
{
    public class CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions : IObjectToString
    {
        public bool ShowOnlyOutdatedPackages { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(ShowOnlyOutdatedPackages)} = {ShowOnlyOutdatedPackages}");

            return sb.ToString();
        }
    }
}
