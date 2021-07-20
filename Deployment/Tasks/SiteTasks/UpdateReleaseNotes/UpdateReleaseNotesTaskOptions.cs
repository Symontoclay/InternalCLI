using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.SiteTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesTaskOptions : IObjectToString
    {
        public string FutureReleaseFilePath { get; set; }
        public List<KindOfArtifact> ArtifactsForDeployment { get; set; }
        public string ReleaseNotesFilePath { get; set; }
        public string BaseHref { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(FutureReleaseFilePath)} = {FutureReleaseFilePath}");
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.AppendLine($"{spaces}{nameof(ReleaseNotesFilePath)} = {ReleaseNotesFilePath}");
            sb.AppendLine($"{spaces}{nameof(BaseHref)} = {BaseHref}");

            return sb.ToString();
        }
    }
}
