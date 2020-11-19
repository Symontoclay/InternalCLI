using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class MethodCSharpUserApiXMLDocPageProcessor : BaseCSharpUserApiXMLDocPageProcessor
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static SiteElementInfo ConvertMethodCardToSiteElementInfo(MethodCard methodCard, SiteElementInfo parent)
        {
#if DEBUG
            //_logger.Info($"methodCard = {methodCard}");
#endif

            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = methodCard.Href;
            result.TargetFullFileName = methodCard.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;

            var title = $"{methodCard.Parent.Name.DisplayedName}.{methodCard.Name.Name} Method ({methodCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

            result.BreadcrumbTitle = methodCard.Name.Name;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = methodCard.Name.Name;

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            microdata.Title = title;
            microdata.Description = methodCard.Summary;

            result.Parent = parent;
            parent.SubItemsList.Add(result);

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            return result;
        }

        public MethodCSharpUserApiXMLDocPageProcessor(MethodCard methodCard, SiteElementInfo parent)
            : base(ConvertMethodCardToSiteElementInfo(methodCard, parent))
        {
            _methodCard = methodCard;
        }

        private MethodCard _methodCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_methodCard.Parent.Name.DisplayedName}.{_methodCard.Name.DisplayedName} Method</h1>");

            PrintMetadata(sb, _methodCard.Name, _methodCard.Parent.Package.AssemblyName);

            PrintSummary(sb, _methodCard.Summary);
            

            if (_methodCard.ParamsList.Any())
            {
                sb.AppendLine("<h2>Parameters</h2>");

                sb.AppendLine("<div>");
                sb.AppendLine("<dl>");
                
                foreach(var param in _methodCard.ParamsList)
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

            if(!string.IsNullOrWhiteSpace(_methodCard.Returns))
            {
                sb.AppendLine("<h2>Returns</h2>");

                sb.AppendLine("<div>");
                sb.AppendLine(GetContent(_methodCard.Returns));
                sb.AppendLine("</div>");

#if DEBUG
                //_logger.Info($"sb = {sb}");
#endif
            }

            PrintRemarks(sb, _methodCard.Remarks);

            return sb.ToString();
        }
    }
}
