using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public class SolutionSource : IObjectToString
    {
        public string Kind { get; set; }
        public List<string> Paths { get; set; }
        public string License { get; set; }
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
        public List<ProjectSource> Projects { get; set; }

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
            sb.PrintPODList(n, nameof(Paths), Paths);
            sb.AppendLine($"{spaces}{nameof(License)} = {License}");
            sb.AppendLine($"{spaces}{nameof(SourcePath)} = {SourcePath}");
            sb.AppendLine($"{spaces}{nameof(DestPath)} = {DestPath}");
            sb.PrintObjListProp(n, nameof(Projects), Projects);

            return sb.ToString();
        }
    }
}
