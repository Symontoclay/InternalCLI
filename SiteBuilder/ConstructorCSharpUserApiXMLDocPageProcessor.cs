using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class ConstructorCSharpUserApiXMLDocPageProcessor : BaseCSharpUserApiXMLDocPageProcessor
    {
        private static SiteElementInfo ConvertMethodCardToSiteElementInfo(ConstructorCard constructorCard, SiteElementInfo parent)
        {
#if DEBUG
            //_logger.Info($"constructorCard = {constructorCard}");
#endif

            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = constructorCard.Href;
            result.TargetFullFileName = constructorCard.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;

            var title = $"{constructorCard.Parent.Name.DisplayedName}.{constructorCard.Name.Name} Method ({constructorCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

            result.BreadcrumbTitle = constructorCard.Name.Name;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = constructorCard.Name.Name;

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            microdata.Title = title;
            microdata.Description = constructorCard.Summary;

            result.Parent = parent;
            parent.SubItemsList.Add(result);

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            return result;
        }

        public ConstructorCSharpUserApiXMLDocPageProcessor(ConstructorCard constructorCard, SiteElementInfo parent, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertMethodCardToSiteElementInfo(constructorCard, parent), generalSiteBuilderSettings)
        {
            _constructorCard = constructorCard;
        }

        private ConstructorCard _constructorCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_constructorCard.Parent.Name.DisplayedName}.{_constructorCard.Name.DisplayedName} Constructor</h1>");

            PrintMetadata(sb, _constructorCard.Name, _constructorCard.Parent.Package.AssemblyName);

            PrintSummary(sb, _constructorCard.Summary);

            PrintParameters(sb, _constructorCard.ParamsList);

            PrintRemarks(sb, _constructorCard.Remarks);

            return sb.ToString();
        }
    }
}
