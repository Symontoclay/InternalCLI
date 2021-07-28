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


        public ConstructorCSharpUserApiXMLDocPageProcessor(ConstructorCard constructorCard, SiteElementInfo parent, GeneralSiteBuilderSettings generalSiteBuilderSettings)
            : base(ConvertMethodCardToSiteElementInfo(constructorCard, parent), generalSiteBuilderSettings)
        {
            _constructorCard = constructorCard;
        }

        private ConstructorCard _constructorCard;

        protected override string GetInitialContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<h1>{_constructorCard.Parent.Name.DisplayedName}.{_constructorCard.Name.DisplayedName} Method</h1>");

            PrintMetadata(sb, _constructorCard.Name, _constructorCard.Parent.Package.AssemblyName);

            PrintSummary(sb, _constructorCard.Summary);

            PrintParameters(sb, _constructorCard.ParamsList);

            PrintRemarks(sb, _constructorCard.Remarks);

            return sb.ToString();
        }
    }
}
