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

            var title = $"{methodCard.Parent.Name.DisplayedName}.{methodCard.Name.DisplayedName} Method ({methodCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

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

        public MethodCSharpUserApiXMLDocPageProcessor(MethodCard methodCard, SiteElementInfo parent, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertMethodCardToSiteElementInfo(methodCard, parent), generalSiteBuilderSettings)
        {
            _methodCard = methodCard;
        }

        private MethodCard _methodCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_methodCard.Parent.Name.DisplayedName}.{_methodCard.Name.DisplayedName.Replace("<", "&lt;").Replace(">", "&gt;").Trim()} Method</h1>");

            PrintMetadata(sb, _methodCard.Name, _methodCard.Parent.Package.AssemblyName);

            PrintSummary(sb, _methodCard.Summary);

            PrintTypeParameters(sb, _methodCard.TypeParamsList);

            PrintParameters(sb, _methodCard.ParamsList);

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
