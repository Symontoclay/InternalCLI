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

            return sb.ToString();
        }
    }
}
