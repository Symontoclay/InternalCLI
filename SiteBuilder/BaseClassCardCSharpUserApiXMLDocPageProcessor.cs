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

        private static SiteElementInfo ConvertClassCardToSiteElementInfo(ClassCard classCard, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
#if DEBUG
            //_logger.Info($"classCard = {classCard}");
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

            result.Parent = generalSiteBuilderSettings.RootCSharpUserApiXMLDocSiteElement;
            generalSiteBuilderSettings.RootCSharpUserApiXMLDocSiteElement.SubItemsList.Add(result);

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            return result;
        }

        protected BaseClassCardCSharpUserApiXMLDocPageProcessor(ClassCard classCard, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertClassCardToSiteElementInfo(classCard, generalSiteBuilderSettings), generalSiteBuilderSettings)
        {
            _classCard = classCard;

#if DEBUG
            if(_classCard.ExamplesList.Any())
            {
                throw new NotImplementedException();
            }
#endif
        }

        private readonly ClassCard _classCard;

        protected void PrintHeader(StringBuilder sb)
        {
            sb.AppendLine($"<h1>{_classCard.Name.Name} {(_classCard.KindOfType == KindOfType.Class ? "Class" : "Interface")}</h1>");
        }

        protected void PrintMetadata(StringBuilder sb)
        {
            PrintMetadata(sb, _classCard.Name, _classCard.Package.AssemblyName);
        }

        protected void PrintSummary(StringBuilder sb)
        {
            PrintSummary(sb, _classCard.Summary);
        }

        protected void PrintRemarks(StringBuilder sb)
        {
            PrintRemarks(sb, _classCard.Remarks);
        }

        protected void PrintConstructors(StringBuilder sb)
        {
            if(!_classCard.ConstructorsList.Any())
            {
                return;
            }

            sb.AppendLine("<h2>Constructors</h2>");

            PrintMembersList(sb, _classCard.ConstructorsList);
        }

        protected void PrintProperties(StringBuilder sb)
        {
            if (!_classCard.PropertiesList.Any())
            {
                return;
            }

            sb.AppendLine("<h2>Properties</h2>");

            PrintMembersList(sb, _classCard.PropertiesList);
        }

        protected void PrintMethods(StringBuilder sb)
        {
            if (!_classCard.MethodsList.Any())
            {
                return;
            }

            sb.AppendLine("<h2>Methods</h2>");

            PrintMembersList(sb, _classCard.MethodsList);
        }

        protected void PrintMembersList(StringBuilder sb, IEnumerable<MemberCard> membersList)
        {
            sb.AppendLine("<table style='font-size: 14px; margin-bottom: 20px; width=100%;' class='table'>");
            foreach (var item in membersList)
            {
#if DEBUG
                //_logger.Info($"item.Name.InitialName = {item.Name.InitialName}");
                //_logger.Info($"item.Name.Name = {item.Name.Name}");
                //_logger.Info($"item.Name.FullName = {item.Name.FullName}");
                //_logger.Info($"item.Href = '{item.Href}'");
                //_logger.Info($"item.Name.DisplayedName = {item.Name.DisplayedName}");
#endif

                sb.AppendLine("<tr style='border-bottom: solid 1px #e2e2e2;'>");
                sb.AppendLine("<td style='width: 300px;'>");
                sb.AppendLine($"<a href='{item.Href}'>{item.Name.DisplayedName.Replace("<", "&lt;").Replace(">", "&gt;").Trim()}</a>");
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(GetContent(item.Summary));
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
        }
    }
}
