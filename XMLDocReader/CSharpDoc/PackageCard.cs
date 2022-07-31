using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public class PackageCard : IObjectToString, IObjectToShortString, IObjectToBriefString
    {
        public string AssemblyName { get; set; }
        public List<NamespaceCard> NamespacesList { get; set; } = new List<NamespaceCard>();
        public List<ClassCard> ClassesList { get; set; } = new List<ClassCard>();
        public List<ClassCard> InterfacesList { get; set; } = new List<ClassCard>();
        public List<EnumCard> EnumsList { get; set; } = new List<EnumCard>();
        public List<XMLMemberCard> XMLCardsWithoutTypeList { get; set; } = new List<XMLMemberCard>();

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(AssemblyName)} = {AssemblyName}");
            sb.PrintObjListProp(n, nameof(NamespacesList), NamespacesList);
            sb.PrintObjListProp(n, nameof(ClassesList), ClassesList);
            sb.PrintObjListProp(n, nameof(InterfacesList), InterfacesList);
            sb.PrintObjListProp(n, nameof(EnumsList), EnumsList);
            sb.PrintObjListProp(n, nameof(XMLCardsWithoutTypeList), XMLCardsWithoutTypeList);

            return sb.ToString();
        }

        /// <inheritdoc/>
        public string ToShortString()
        {
            return ToShortString(0u);
        }

        /// <inheritdoc/>
        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToShortString.PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(AssemblyName)} = {AssemblyName}");
            sb.PrintShortObjListProp(n, nameof(NamespacesList), NamespacesList);
            sb.PrintShortObjListProp(n, nameof(ClassesList), ClassesList);
            sb.PrintShortObjListProp(n, nameof(InterfacesList), InterfacesList);
            sb.PrintShortObjListProp(n, nameof(EnumsList), EnumsList);
            sb.PrintExistingList(n, nameof(XMLCardsWithoutTypeList), XMLCardsWithoutTypeList);

            return sb.ToString();
        }

        /// <inheritdoc/>
        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        /// <inheritdoc/>
        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToBriefString.PropertiesToBriefString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(AssemblyName)} = {AssemblyName}");
            sb.PrintExistingList(n, nameof(NamespacesList), NamespacesList);
            sb.PrintExistingList(n, nameof(XMLCardsWithoutTypeList), XMLCardsWithoutTypeList);

            return sb.ToString();
        }
    }
}
