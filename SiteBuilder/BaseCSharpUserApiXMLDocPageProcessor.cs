using CommonMark;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public abstract class BaseCSharpUserApiXMLDocPageProcessor: PageProcessor
    {
        protected BaseCSharpUserApiXMLDocPageProcessor(SiteElementInfo siteElement, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(siteElement, generalSiteBuilderSettings)
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

        protected void PrintTypeParameters(StringBuilder sb, List<TypeParamCard> typeParamsList)
        {
            if (typeParamsList.Any())
            {
                sb.AppendLine("<h2>Type Parameters</h2>");

                sb.AppendLine("<div>");
                sb.AppendLine("<dl>");

                foreach (var param in typeParamsList)
                {
#if DEBUG
                    //_logger.Info($"param = {param}");
#endif

                    sb.AppendLine($"<dt>{param.Name}</dt>");
                    sb.AppendLine($"<dd>{GetContent(param.Value)}</dd>");
                }

                sb.AppendLine("</dl>");
                sb.AppendLine("</div>");
            }
        }

        protected void PrintParameters(StringBuilder sb, List<MethodParamCard> paramsList)
        {
            if (paramsList.Any())
            {
                sb.AppendLine("<h2>Parameters</h2>");

                sb.AppendLine("<div>");
                sb.AppendLine("<dl>");

                foreach (var param in paramsList)
                {
#if DEBUG
                    //_logger.Info($"param = {param}");
#endif

                    sb.AppendLine($"<dt>{param.Name}</dt>");
                    sb.AppendLine($"<dd>{GetContent(param.Summary)}</dd>");
                }

                sb.AppendLine("</dl>");
                sb.AppendLine("</div>");
            }
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
