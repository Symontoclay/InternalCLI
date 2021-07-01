using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public class SymOntoClayProjectsSettings: ISymOntoClayProjectsSettings
    {
        /// <inheritdoc/>
        public string BasePath { get; set; }

        public List<UtityExeInstance> UtityExeInstances { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IUtityExeInstance> ISymOntoClayProjectsSettings.UtityExeInstances => UtityExeInstances;

        public List<SolutionSettings> Solutions { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ISolutionSettings> ISymOntoClayProjectsSettings.Solutions => Solutions;

        public List<ProjectSettings> Projects { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IProjectSettings> ISymOntoClayProjectsSettings.Projects => Projects;

        public List<ArtifactSettings> Artifacts { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IArtifactSettings> ISymOntoClayProjectsSettings.Artifacts => Artifacts;

        public List<LicenseSettings> Licenses { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ILicenseSettings> ISymOntoClayProjectsSettings.Licenses => Licenses;

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

            sb.AppendLine($"{spaces}{nameof(BasePath)} = {BasePath}");
            sb.PrintObjListProp(n, nameof(UtityExeInstances), UtityExeInstances);
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.PrintObjListProp(n, nameof(Artifacts), Artifacts);
            sb.PrintObjListProp(n, nameof(Licenses), Licenses);

            return sb.ToString();
        }
    }
}
