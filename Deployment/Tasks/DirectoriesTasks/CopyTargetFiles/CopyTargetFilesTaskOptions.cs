using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Tasks.DirectoriesTasks.CopyTargetFiles
{
    public class CopyTargetFilesTaskOptions : IObjectToString
    {
        public string DestDir { get; set; }
        public bool SaveSubDirs { get; set; } = true;
        public List<string> TargetFiles { get; set; }
        public string BaseSourceDir { get; set; }
        public ExistingFileStrategy ExistingFileStrategy { get; set; } = ExistingFileStrategy.Overwrite;

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

            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");
            sb.AppendLine($"{spaces}{nameof(SaveSubDirs)} = {SaveSubDirs}");
            sb.AppendLine($"{spaces}{nameof(BaseSourceDir)} = {BaseSourceDir}");
            sb.AppendLine($"{spaces}{nameof(ExistingFileStrategy)} = {ExistingFileStrategy}");
            sb.PrintPODList(n, nameof(TargetFiles), TargetFiles);

            return sb.ToString();
        }
    }
}
