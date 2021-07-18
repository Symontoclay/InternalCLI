using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public class EnumCSharpUserApiXMLDocPageProcessor : BaseCSharpUserApiXMLDocPageProcessor
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static SiteElementInfo ConvertEnumCardToSiteElementInfo(EnumCard enumCard, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //_logger.Info($"enumCard = {enumCard}");
#endif

            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = enumCard.Href;
            result.TargetFullFileName = enumCard.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;

            var title = $"{enumCard.Name.Name} Enum ({enumCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

            result.BreadcrumbTitle = enumCard.Name.Name;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = enumCard.Name.Name;

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            microdata.Title = title;
            microdata.Description = enumCard.Summary;

            result.Parent = generalSiteBuilderSettings.RootCSharpUserApiXMLDocSiteElement;
            generalSiteBuilderSettings.RootCSharpUserApiXMLDocSiteElement.SubItemsList.Add(result);

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            return result;
        }

        public EnumCSharpUserApiXMLDocPageProcessor(EnumCard enumCard, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertEnumCardToSiteElementInfo(enumCard, generalSiteBuilderSettings), generalSiteBuilderSettings)
        {
            _enumCard = enumCard;
        }

        private readonly EnumCard _enumCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_enumCard.Name.DisplayedName} Enum</h1>");

            PrintMetadata(sb, _enumCard.Name, _enumCard.Package.AssemblyName);

            PrintSummary(sb, _enumCard.Summary);

            sb.AppendLine("<h2>Fields</h2>");

            sb.AppendLine("<div>");
            sb.AppendLine("<dl>");

            foreach (var field in _enumCard.FieldsList)
            {
#if DEBUG
                //_logger.Info($"field = {field}");
#endif

                sb.AppendLine($"<dt>{field.Name.DisplayedName}</dt>");
                sb.AppendLine($"<dd>{GetContent(field.Summary)}</dd>");
            }

            sb.AppendLine("</dl>");
            sb.AppendLine("</div>");

            PrintRemarks(sb, _enumCard.Remarks);

            return sb.ToString();
        }
    }
}
