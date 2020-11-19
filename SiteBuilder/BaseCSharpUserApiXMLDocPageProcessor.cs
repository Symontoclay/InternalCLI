using CommonMark;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public abstract class BaseCSharpUserApiXMLDocPageProcessor: PageProcessor
    {
        protected BaseCSharpUserApiXMLDocPageProcessor(SiteElementInfo siteElement)
            : base(siteElement)
        {
        }

        protected void PrintMetadata(StringBuilder sb, MemberName name, AssemblyName assemblyName)
        {
            sb.AppendLine("<div style='margin-top: 20px; margin-bottom: 20px; font-size: 14px;'>");
            sb.AppendLine($"<div><span style='font-weight: bold;'>Namespace:</span>&nbsp;{name.Path}</div>");
            sb.AppendLine($"<div><span style='font-weight: bold;'>Assembly:</span>&nbsp;{assemblyName.Name}</div>");
            sb.AppendLine("</div>");
        }

        protected void PrintSummary(StringBuilder sb, string summary)
        {
            sb.AppendLine($"<div>{GetContent(summary)}</div>");
        }

        protected void PrintRemarks(StringBuilder sb, string remarks)
        {
            if(string.IsNullOrWhiteSpace(remarks))
            {
                return;
            }

            sb.AppendLine("<h2>Remarks</h2>");
            sb.AppendLine("<div>");
            sb.AppendLine(GetContent(remarks));
            sb.AppendLine("</div>");
        }

        protected string GetContent(string content)
        {
            return CommonMarkConverter.Convert(content);
        }
    }
}

//
//
//