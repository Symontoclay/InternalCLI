using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CreateExtendedDocFile
{
    public class CreateExtendedDocFileDevTaskOptions : IObjectToString
    {
        public string XmlDocFile { get; set; }
        public string ExtendedDocFile { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(XmlDocFile)} = {XmlDocFile}");
            sb.AppendLine($"{spaces}{nameof(ExtendedDocFile)} = {ExtendedDocFile}");
            return sb.ToString();
        }
    }
}
