using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.DevTasks.DetectUnmanagedProjects
{
    public class DetectUnmanagedProjectsDevTaskOptions : IObjectToString
    {
        public List<string> ManagedSolutions { get; set; }

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
            var sb = new StringBuilder();

            sb.PrintPODList(n, nameof(ManagedSolutions), ManagedSolutions);

            return sb.ToString();
        }
    }
}
