using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XMLDocReader;

namespace TestSandBox.XMLDoc
{
    public class ReadXMLDocHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var targetXMLDocFaleName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml");

            _logger.Info($"targetXMLDocFaleName = {targetXMLDocFaleName}");

            using var fs = File.OpenRead(targetXMLDocFaleName);
            var doc = XElement.Load(fs);

            var membersXElem = doc.Element("members");

            foreach (var childElem in membersXElem.Elements())
            {
                _logger.Info($"childElem = {childElem}");

                var memberCard = new XMLMemberCard();

                var initialName = childElem.Attribute("name").Value;

                memberCard.Name = MemberNameParser.Parse(initialName);

                var inheritedDocNode = childElem.Element("inheritdoc");

                if(inheritedDocNode != null)
                {
                    memberCard.IsInheritdoc = true;

                    var crefAttr = inheritedDocNode.Attribute("cref");

                    if(crefAttr != null)
                    {
                        memberCard.InheritdocCref = crefAttr.Value.Trim();
                    }                    

                    _logger.Info($"memberCard = {memberCard}");

                    continue;
                }

                var includeNode = childElem.Element("include");

                if(includeNode != null)
                {
                    memberCard.IsInclude = true;

                    memberCard.IncludeFile = includeNode.Attribute("file").Value.Trim();

                    memberCard.IncludePath = includeNode.Attribute("path").Value.Trim();

                    _logger.Info($"memberCard = {memberCard}");

                    continue;
                }

                var summaryNode = childElem.Element("summary");

                if(summaryNode != null)
                {
                    memberCard.Summary = summaryNode.Value.Trim();

                    summaryNode.Remove();
                }

                var remarksNode = childElem.Element("remarks");

                if(remarksNode != null)
                {
                    memberCard.Remarks = remarksNode.Value.Trim();

                    remarksNode.Remove();
                }

                var typeParamsList = childElem.Elements("typeparam").ToList();

                foreach(var typeParamNode in typeParamsList)
                {
                    var typeParamItem = new XMLParamCard();

                    typeParamItem.Name = typeParamNode.Attribute("name").Value.Trim();
                    typeParamItem.Value = typeParamNode.Value.Trim();

                    memberCard.TypeParamsList.Add(typeParamItem);

                    typeParamNode.Remove();
                }

                var paramsList = childElem.Elements("param").ToList();

                foreach(var paramNode in paramsList)
                {
                    var paramItem = new XMLParamCard();

                    paramItem.Name = paramNode.Attribute("name").Value.Trim();
                    paramItem.Value = paramNode.Value.Trim();

                    memberCard.ParamsList.Add(paramItem);

                    paramNode.Remove();
                }

                var returnsNode = childElem.Element("returns");

                if(returnsNode != null)
                {
                    memberCard.Returns = returnsNode.Value.Trim();

                    returnsNode.Remove();
                }

                var valueNode = childElem.Element("value");

                if(valueNode != null)
                {
                    memberCard.Value = valueNode.Value.Trim();

                    valueNode.Remove();
                }

                var examplesList = childElem.Elements("example").ToList();

                foreach(var exampleNode in examplesList)
                {
                    memberCard.ExamplesList.Add(exampleNode.Value.Trim());

                    exampleNode.Remove();
                }

                var exceptionsList = childElem.Elements("exception").ToList();

                foreach(var exceptionNode in exceptionsList)
                {
                    var exceptionItem = new XMLExceptionCard();
                    exceptionItem.Cref = exceptionNode.Attribute("cref").Value.Trim();
                    exceptionItem.Value = exceptionNode.Value.Trim();

                    memberCard.ExceptionsList.Add(exceptionItem);

                    exceptionNode.Remove();
                }

                var seeAlsoList = childElem.Elements("seealso").ToList();

                foreach (var seeAlsoNode in seeAlsoList)
                {
                    memberCard.SeeAlsoList.Add(seeAlsoNode.Attribute("cref").Value.Trim());

                    seeAlsoNode.Remove();
                }

                _logger.Info($"childElem (2) = {childElem}");

                _logger.Info($"memberCard = {memberCard}");
            }

            _logger.Info("End");
        }

        //private string GetSee()
        //{

        //}

        public void ParseGenericType()
        {
            _logger.Info("Begin");

            var fullTypeName = "System.Collections.Generic.IList{SymOntoClay.UnityAsset.Core.DeviceOfBiped}";

            var figureBracketPos = fullTypeName.IndexOf("{");

            _logger.Info($"figureBracketPos = {figureBracketPos}");

            var name = fullTypeName.Substring(0, figureBracketPos);

            _logger.Info($"name = {name}");

            var strWithParameters = fullTypeName.Substring(figureBracketPos + 1, fullTypeName.Length - figureBracketPos - 2);

            _logger.Info($"strWithParameters = {strWithParameters}");

            var parametersList = MemberNameParser.GetParametersList(strWithParameters);

            _logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList, Formatting.Indented)}");

            _logger.Info("End");
        }
    }
}
