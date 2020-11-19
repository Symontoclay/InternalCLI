﻿using System;
using System.Collections.Generic;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class InterfaceCSharpUserApiXMLDocPageProcessor: BaseClassCardCSharpUserApiXMLDocPageProcessor
    {

        public InterfaceCSharpUserApiXMLDocPageProcessor(ClassCard interfaceInfo)
            : base(interfaceInfo)
        {
        }

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            PrintHeader(sb);
            PrintMetadata(sb);
            PrintSummary(sb);
            PrintRemarks(sb);
            PrintProperties(sb);
            PrintMethods(sb);

            return sb.ToString();
        }
    }
}
