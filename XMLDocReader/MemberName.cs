using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader
{
    public class MemberName: IObjectToString
    {
        public string InitialName { get; set; } = string.Empty;
        public KindOfMember Kind { get; set; } = KindOfMember.Unknown;
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string ImplInterfaceName { get; set; } = string.Empty;
        public List<string> ParametersList { get; set; } = new List<string>();

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

            sb.AppendLine($"{spaces}{nameof(InitialName)} = {InitialName}");
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(FullName)} = {FullName}");
            sb.AppendLine($"{spaces}{nameof(Path)} = {Path}");
            sb.AppendLine($"{spaces}{nameof(ImplInterfaceName)} = {ImplInterfaceName}");
            sb.PrintPODList(n, nameof(ParametersList), ParametersList);

            return sb.ToString();
        }
    }
}
