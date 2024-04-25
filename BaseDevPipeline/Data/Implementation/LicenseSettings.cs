using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Text;

namespace BaseDevPipeline.Data.Implementation
{
    public class LicenseSettings: ILicenseSettings
    {
        public string Name { get; set; }
        public string HeaderFileName { get; set; }
        public string HeaderContent { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(HeaderFileName)} = {HeaderFileName}");
            sb.PrintPODProp(n, nameof(HeaderContent), HeaderContent);
            sb.AppendLine($"{spaces}{nameof(FileName)} = {FileName}");
            sb.PrintPODProp(n, nameof(Content), Content);

            return sb.ToString();
        }
    }
}
