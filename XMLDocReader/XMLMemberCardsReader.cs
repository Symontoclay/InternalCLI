using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XMLDocReader
{
    public static class XMLMemberCardsReader
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static List<XMLMemberCard> Read(string fileName)
        {
            var result = new List<XMLMemberCard>();

            using var fs = File.OpenRead(fileName);
            var doc = XElement.Load(fs);

            var membersXElem = doc.Element("members");

            foreach (var childElem in membersXElem.Elements())
            {
                var memberCard = new XMLMemberCard();
                result.Add(memberCard);

                var initialName = childElem.Attribute("name").Value;

                memberCard.Name = MemberNameParser.Parse(initialName);

                var inheritedDocNode = childElem.Element("inheritdoc");

                if (inheritedDocNode != null)
                {
                    memberCard.IsInheritdoc = true;

                    var crefAttr = inheritedDocNode.Attribute("cref");

                    if (crefAttr != null)
                    {
                        memberCard.InheritdocCref = crefAttr.Value.Trim();
                    }

                    continue;
                }

                var includeNode = childElem.Element("include");

                if (includeNode != null)
                {
                    memberCard.IsInclude = true;

                    memberCard.IncludeFile = includeNode.Attribute("file").Value.Trim();

                    memberCard.IncludePath = includeNode.Attribute("path").Value.Trim();

                    continue;
                }

                var summaryNode = childElem.Element("summary");

                if (summaryNode != null)
                {
                    memberCard.Summary = summaryNode.Value.Trim();

                    //summaryNode.Remove();
                }

                var remarksNode = childElem.Element("remarks");

                if (remarksNode != null)
                {
                    memberCard.Remarks = remarksNode.Value.Trim();

                    //remarksNode.Remove();
                }

                var typeParamsList = childElem.Elements("typeparam").ToList();

                foreach (var typeParamNode in typeParamsList)
                {
                    var typeParamItem = new XMLParamCard();

                    typeParamItem.Name = typeParamNode.Attribute("name").Value.Trim();
                    typeParamItem.Value = typeParamNode.Value.Trim();

                    memberCard.TypeParamsList.Add(typeParamItem);

                    //typeParamNode.Remove();
                }

                var paramsList = childElem.Elements("param").ToList();

                foreach (var paramNode in paramsList)
                {
                    var paramItem = new XMLParamCard();

                    paramItem.Name = paramNode.Attribute("name").Value.Trim();
                    paramItem.Value = paramNode.Value.Trim();

                    memberCard.ParamsList.Add(paramItem);

                    //paramNode.Remove();
                }

                var returnsNode = childElem.Element("returns");

                if (returnsNode != null)
                {
                    memberCard.Returns = returnsNode.Value.Trim();

                    //returnsNode.Remove();
                }

                var valueNode = childElem.Element("value");

                if (valueNode != null)
                {
                    memberCard.Value = valueNode.Value.Trim();

                    //valueNode.Remove();
                }

                var examplesList = childElem.Elements("example").ToList();

                foreach (var exampleNode in examplesList)
                {
                    memberCard.ExamplesList.Add(exampleNode.Value.Trim());

                    //exampleNode.Remove();
                }

                var exceptionsList = childElem.Elements("exception").ToList();

                foreach (var exceptionNode in exceptionsList)
                {
                    var exceptionItem = new XMLExceptionCard();
                    exceptionItem.Cref = exceptionNode.Attribute("cref").Value.Trim();
                    exceptionItem.Value = exceptionNode.Value.Trim();

                    memberCard.ExceptionsList.Add(exceptionItem);

                    //exceptionNode.Remove();
                }
            }

            return result;
        }
    }
}
