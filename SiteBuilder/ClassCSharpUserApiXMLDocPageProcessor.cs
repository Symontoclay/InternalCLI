using System;
using System.Collections.Generic;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class ClassCSharpUserApiXMLDocPageProcessor : BaseClassCardCSharpUserApiXMLDocPageProcessor
    {
        public ClassCSharpUserApiXMLDocPageProcessor(ClassCard classInfo)
            : base(classInfo)
        {
        }

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            PrintHeader(sb);
            PrintMetadata(sb);
            PrintSummary(sb);
            
            PrintProperties(sb);
            PrintMethods(sb);

            PrintRemarks(sb);

            return sb.ToString();
        }
    }
}
