using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Text;

namespace BaseDevPipeline
{
    public class FutureReleaseInfo : IObjectToString
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public FutureReleaseStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.PrintPODProp(n, nameof(Description), Description);
            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(StartDate)} = {StartDate}");
            sb.AppendLine($"{spaces}{nameof(FinishDate)} = {FinishDate}");

            return sb.ToString();
        }
    }
}
