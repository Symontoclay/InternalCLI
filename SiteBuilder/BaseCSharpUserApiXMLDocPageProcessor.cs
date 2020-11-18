using CommonMark;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder
{
    public abstract class BaseCSharpUserApiXMLDocPageProcessor: PageProcessor
    {
        protected BaseCSharpUserApiXMLDocPageProcessor(SiteElementInfo siteElement)
            : base(siteElement)
        {
        }

        protected void PrintSummary(StringBuilder sb, string summary)
        {
            sb.AppendLine(CommonMarkConverter.Convert(summary));
        }
    }
}

//
//ClassCSharpUserApiXMLDocPageProcessor
//EnumCSharpUserApiXMLDocPageProcessor