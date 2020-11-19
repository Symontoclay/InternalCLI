using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class PropertyCSharpUserApiXMLDocPageProcessor : BaseCSharpUserApiXMLDocPageProcessor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static SiteElementInfo ConvertPropertyCardToSiteElementInfo(PropertyCard propertyCard, SiteElementInfo parent)
        {
#if DEBUG
            _logger.Info($"propertyCard = {propertyCard}");
#endif

            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = propertyCard.Href;
            result.TargetFullFileName = propertyCard.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;

            var title = $"{propertyCard.Parent.Name.DisplayedName}.{propertyCard.Name.Name} Property ({propertyCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

            result.BreadcrumbTitle = propertyCard.Name.Name;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = propertyCard.Name.Name;

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            microdata.Title = title;
            microdata.Description = propertyCard.Summary;

            result.Parent = parent;
            parent.SubItemsList.Add(result);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            return result;
        }

        public PropertyCSharpUserApiXMLDocPageProcessor(PropertyCard propertyCard, SiteElementInfo parent)
            : base(ConvertPropertyCardToSiteElementInfo(propertyCard, parent))
        {
            _propertyCard = propertyCard;
        }

        private PropertyCard _propertyCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_propertyCard.Parent.Name.DisplayedName}.{_propertyCard.Name.DisplayedName} Property</h1>");

            PrintMetadata(sb, _propertyCard.Name, _propertyCard.Parent.Package.AssemblyName);
            PrintSummary(sb, _propertyCard.Summary);
            PrintRemarks(sb, _propertyCard.Remarks);

            if (!string.IsNullOrWhiteSpace(_propertyCard.Value))
            {
                sb.AppendLine("<h2>Remarks</h2>");
                sb.AppendLine("<div>");
                sb.AppendLine(GetContent(_propertyCard.Value));
                sb.AppendLine("</div>");
            }

            return sb.ToString();
        }
    }
}
