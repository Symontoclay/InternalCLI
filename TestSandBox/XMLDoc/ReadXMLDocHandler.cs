using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

            var membersXElem = doc.Elements("members");

            foreach (var childElem in membersXElem.Elements())
            {
                //_logger.Info($"childElem.Name = {childElem.Name}");
                var initialName = childElem.Attribute("name").Value;
                //_logger.Info($"initialName = {initialName}");

                var type = initialName[0];

                _logger.Info($"type = {type}");

                var initialNameWithoutType = initialName.Substring(2);

                _logger.Info($"initialNameWithoutType = {initialNameWithoutType}");

                var lastDotPos = LastDotPos(initialNameWithoutType);

                _logger.Info($"lastDotPos = {lastDotPos}");

                var path = initialNameWithoutType.Substring(0, lastDotPos).Trim();

                _logger.Info($"path = {path}");

                var rawName = initialNameWithoutType.Substring(lastDotPos + 1).Trim();

                _logger.Info($"rawName = {rawName}");

                if(rawName.Contains("#"))
                {
                    var lastSharpPos = LastSharpPos(rawName);

                    _logger.Info($"lastSharpPos = {lastSharpPos}");

                    var rawImplInterfaceName = rawName.Substring(0, lastSharpPos);

                    _logger.Info($"rawImplInterfaceName = {rawImplInterfaceName}");

                    var implInterfaceName = rawImplInterfaceName.Replace("#", ".").Trim();

                    _logger.Info($"implInterfaceName = {implInterfaceName}");

                    rawName = rawName.Substring(lastSharpPos + 1).Trim();

                    _logger.Info($"rawName (2) = {rawName}");
                }

                if(rawName.Contains("("))
                {
                    var openRoundBracketPos = rawName.IndexOf("(");

                    _logger.Info($"openRoundBracketPos = {openRoundBracketPos}");

                    var strWithParameters = rawName.Substring(openRoundBracketPos + 1, rawName.Length - openRoundBracketPos - 2);

                    _logger.Info($"strWithParameters = {strWithParameters}");

                    rawName = rawName.Substring(0, openRoundBracketPos);

                    _logger.Info($"rawName (3) = {rawName}");

                    var parametersList = GetParametersList(strWithParameters);

                    _logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList, Formatting.Indented)}");
                }

                if(rawName.Contains("{"))
                {
                    throw new NotImplementedException();
                }

                if (rawName.Contains("["))
                {
                    throw new NotImplementedException();
                }
            }

            _logger.Info("End");
        }

        private List<string> GetParametersList(string inputStr)
        {
            _logger.Info($"inputStr = {inputStr}");

            var commaPos = inputStr.IndexOf(",");

            _logger.Info($"commaPos = {commaPos}");

            if(commaPos == -1)
            {
                return new List<string>() { inputStr.Trim() };
            }

            var figureBracketPos = inputStr.IndexOf("{");

            _logger.Info($"figureBracketPos = {figureBracketPos}");

            if(figureBracketPos == -1)
            {
                return inputStr.Split(",").Select(p => p.Trim()).ToList();
            }

            var result = new List<string>();

            do
            {
                throw new NotImplementedException();
            } while (true);

            throw new NotImplementedException();
        }

        private int LastDotPos(string name)
        {
            var openRoundBracketPos = name.IndexOf("(");

            var newDotPos = -1;
            var dotPos = -1;

            while((newDotPos = name.IndexOf(".", newDotPos + 1)) != -1)
            {
                if(openRoundBracketPos == -1)
                {
                    dotPos = newDotPos;
                }
                else
                {
                    if(newDotPos < openRoundBracketPos)
                    {
                        dotPos = newDotPos;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return dotPos;    
        }

        private int LastSharpPos(string name)
        {
            var openRoundBracketPos = name.IndexOf("(");

            var newSharpPos = -1;
            var sharpPos = -1;

            while ((newSharpPos = name.IndexOf("#", newSharpPos + 1)) != -1)
            {
                if (openRoundBracketPos == -1)
                {
                    sharpPos = newSharpPos;
                }
                else
                {
                    if (newSharpPos < openRoundBracketPos)
                    {
                        sharpPos = newSharpPos;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return sharpPos;
        }
    }
}
