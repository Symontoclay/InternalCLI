using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public class SymOntoClaySettingsSource : IObjectToString
    {
        public List<string> BasePaths { get; set; }
        public List<SolutionSource> Solutions { get; set; }
        public List<ArtifactDest> Artifacts { get; set; }

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
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(Artifacts), Artifacts);

            return sb.ToString();
        }
    }
}
