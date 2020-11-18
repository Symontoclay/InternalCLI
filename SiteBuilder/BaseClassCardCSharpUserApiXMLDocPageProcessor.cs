using CommonMark;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLDocReader;
using XMLDocReader.CSharpDoc;

namespace SiteBuilder
{
    public abstract class BaseClassCardCSharpUserApiXMLDocPageProcessor : BaseCSharpUserApiXMLDocPageProcessor
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        private static SiteElementInfo ConvertClassCardToSiteElementInfo(ClassCard classCard)
        {
#if DEBUG
            _logger.Info($"classCard = {classCard}");
#endif

            var result = new SiteElementInfo();

            result.Kind = KindOfSiteElement.Page;
            result.Href = classCard.Href;
            result.TargetFullFileName = classCard.TargetFullFileName;

            var sitePageInfo = new SitePageInfo();

            result.SitePageInfo = sitePageInfo;
            sitePageInfo.IsReady = true;
    
            var title = $"{classCard.Name.Name} {(classCard.KindOfType == KindOfType.Class ? "Class" : "Interface")} ({classCard.Name.Path}) | C# user API reference | SymOntoClay Docs";

            result.BreadcrumbTitle = classCard.Name.Name;
            sitePageInfo.Title = title;
            sitePageInfo.BreadcrumbTitle = classCard.Name.Name;

            var microdata = new MicroDataInfo();
            sitePageInfo.Microdata = microdata;

            microdata.Title = title;
            microdata.Description = classCard.Summary;

            result.Parent = GeneralSettings.RootCSharpUserApiXMLDocSiteElement;
            GeneralSettings.RootCSharpUserApiXMLDocSiteElement.SubItemsList.Add(result);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            return result;
        }

        protected BaseClassCardCSharpUserApiXMLDocPageProcessor(ClassCard classCard)
            : base(ConvertClassCardToSiteElementInfo(classCard))
        {
            _classCard = classCard;

#if DEBUG
            if (!string.IsNullOrWhiteSpace(_classCard.Remarks))
            {
                throw new NotImplementedException();
            }

            if(_classCard.ExamplesList.Any())
            {
                throw new NotImplementedException();
            }

            if(_classCard.PropertiesList.Any())
            {
                //throw new NotImplementedException();
            }

            if(_classCard.MethodsList.Any())
            {
                //throw new NotImplementedException();
            }
#endif
        }

        private readonly ClassCard _classCard;

        protected void PrintHeader(StringBuilder sb)
        {
            sb.AppendLine($"<h1>{_classCard.Name.Name} {(_classCard.KindOfType == KindOfType.Class ? "Class" : "Interface")}</h1>");
        }

        protected void PrintSummary(StringBuilder sb)
        {
            PrintSummary(sb, _classCard.Summary);
        }
    }
}
