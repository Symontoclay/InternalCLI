using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory
{
    public class CopyAllFromDirectoryTaskOptions : IObjectToString
    {
        public string SourceDir { get; set; }
        public string DestDir { get; set; }
        public bool SaveSubDirs { get; set; } = true;
        public List<string> OnlySubDirs { get; set; }
        public List<string> ExceptSubDirs { get; set; }
        public List<string> OnlyFileExts { get; set; }
        public List<string> ExceptFileExts { get; set; }
        public List<string> FileNameShouldContain { get; set; }
        public List<string> FileNameShouldNotContain { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(SourceDir)} = {SourceDir}");
            sb.AppendLine($"{spaces}{nameof(DestDir)} = {DestDir}");
            sb.AppendLine($"{spaces}{nameof(SaveSubDirs)} = {SaveSubDirs}");
            sb.PrintPODList(n, nameof(OnlySubDirs), OnlySubDirs);
            sb.PrintPODList(n, nameof(ExceptSubDirs), ExceptSubDirs);
            sb.PrintPODList(n, nameof(OnlyFileExts), OnlyFileExts);
            sb.PrintPODList(n, nameof(ExceptFileExts), ExceptFileExts);
            sb.PrintPODList(n, nameof(FileNameShouldContain), FileNameShouldContain);
            sb.PrintPODList(n, nameof(FileNameShouldNotContain), FileNameShouldNotContain);

            return sb.ToString();
        }
    }
}
