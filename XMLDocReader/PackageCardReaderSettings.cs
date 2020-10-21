using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader
{
    public class PackageCardReaderSettings : IObjectToString
    {
        public string XMLDocFileName { get; set; }
        public string AssemblyFileName { get; set; }

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



            return sb.ToString();
        }
    }
}
