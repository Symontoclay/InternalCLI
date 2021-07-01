using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            sb.AppendLine($"{spaces}{nameof(HeaderContent)} = {HeaderContent}");
            sb.AppendLine($"{spaces}{nameof(FileName)} = {FileName}");
            sb.AppendLine($"{spaces}{nameof(Content)} = {Content}");

            return sb.ToString();
        }
    }
}
