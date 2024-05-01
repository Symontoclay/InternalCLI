using BaseDevPipeline;
using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.SiteTasks.UpdateReleaseNotes
{
    public class UpdateReleaseNotesTaskOptions : IObjectToString
    {
        public FutureReleaseInfo FutureReleaseInfo { get; set; }
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

            sb.PrintObjProp(n, nameof(FutureReleaseInfo), FutureReleaseInfo);
            sb.PrintPODList(n, nameof(ArtifactsForDeployment), ArtifactsForDeployment);
            sb.AppendLine($"{spaces}{nameof(ReleaseNotesFilePath)} = {ReleaseNotesFilePath}");
            sb.AppendLine($"{spaces}{nameof(BaseHref)} = {BaseHref}");

            return sb.ToString();
        }
    }
}
