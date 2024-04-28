using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class XMLMemberCard : IObjectToString
    {
        public MemberName Name { get; set; }
        public bool IsInheritdoc { get; set; }
        public string InheritdocCref { get; set; }
        public bool IsInclude { get; set; }
        public string IncludeFile { get; set; }
        public string IncludePath { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public List<XMLParamCard> TypeParamsList { get; set; } = new List<XMLParamCard>();
        public List<XMLParamCard> ParamsList { get; set; } = new List<XMLParamCard>();
        public string Returns { get; set; }
        public string Value { get; set; }
        public List<string> ExamplesList { get; set; } = new List<string>();  
        public List<XMLExceptionCard> ExceptionsList { get; set; } = new List<XMLExceptionCard>();
        
        public bool IsBuiltTypeOrMemberCard { get; set; }
        public bool IsInheritdocOrIncludeResolved { get; set; }

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

            sb.PrintObjProp(n, nameof(Name), Name);
            sb.AppendLine($"{spaces}{nameof(IsInheritdoc)} = {IsInheritdoc}");
            sb.AppendLine($"{spaces}{nameof(InheritdocCref)} = {InheritdocCref}");
            sb.AppendLine($"{spaces}{nameof(IsInclude)} = {IsInclude}");
            sb.AppendLine($"{spaces}{nameof(IncludeFile)} = {IncludeFile}");
            sb.AppendLine($"{spaces}{nameof(IncludePath)} = {IncludePath}");
            sb.AppendLine($"{spaces}{nameof(Summary)} = {Summary}");
            sb.AppendLine($"{spaces}{nameof(Remarks)} = {Remarks}");
            sb.PrintObjListProp(n, nameof(TypeParamsList), TypeParamsList);
            sb.PrintObjListProp(n, nameof(ParamsList), ParamsList);
            sb.AppendLine($"{spaces}{nameof(Returns)} = {Returns}");
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.PrintPODList(n, nameof(ExamplesList), ExamplesList);
            sb.PrintObjListProp(n, nameof(ExceptionsList), ExceptionsList);
            sb.AppendLine($"{spaces}{nameof(IsBuiltTypeOrMemberCard)} = {IsBuiltTypeOrMemberCard}");
            sb.AppendLine($"{spaces}{nameof(IsInheritdocOrIncludeResolved)} = {IsInheritdocOrIncludeResolved}");

            //sb.AppendLine($"{spaces}{nameof()} = {}");

            return sb.ToString();
        }
    }
}
