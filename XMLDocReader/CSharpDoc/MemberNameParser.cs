using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class MemberNameParser
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static MemberName Parse(string initialName)
        {
            var memberName = new MemberName();

            memberName.InitialName = initialName;

            var type = initialName[0];

            switch (type)
            {
                case 'T':
                    memberName.Kind = KindOfMember.Type;
                    break;

                case 'P':
                    memberName.Kind = KindOfMember.Property;
                    break;

                case 'F':
                    memberName.Kind = KindOfMember.Field;
                    break;

                case 'M':
                    memberName.Kind = KindOfMember.Method;
                    break;

                case 'E':
                    memberName.Kind = KindOfMember.Event;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var initialNameWithoutType = initialName.Substring(2);

            var lastDotPos = LastDotPos(initialNameWithoutType);

            var path = initialNameWithoutType.Substring(0, lastDotPos).Trim();

            //_logger.Info($"path = {path}");

            memberName.Path = path;

            var rawName = initialNameWithoutType.Substring(lastDotPos + 1).Trim();

            //_logger.Info($"rawName = {rawName}");

            if (rawName.Contains("#"))
            {
                var lastSharpPos = LastSharpPos(rawName);

                //_logger.Info($"lastSharpPos = {lastSharpPos}");

                var rawImplInterfaceName = rawName.Substring(0, lastSharpPos);

                //_logger.Info($"rawImplInterfaceName = {rawImplInterfaceName}");

                var implInterfaceName = rawImplInterfaceName.Replace("#", ".").Trim();

                //_logger.Info($"implInterfaceName = {implInterfaceName}");

                memberName.ImplInterfaceName = implInterfaceName;

                rawName = rawName.Substring(lastSharpPos + 1).Trim();

                //_logger.Info($"rawName (2) = {rawName}");
            }

            if (rawName.Contains("("))
            {
                var openRoundBracketPos = rawName.IndexOf("(");

                //_logger.Info($"openRoundBracketPos = {openRoundBracketPos}");

                var strWithParameters = rawName.Substring(openRoundBracketPos + 1, rawName.Length - openRoundBracketPos - 2);

                //_logger.Info($"strWithParameters = {strWithParameters}");

                rawName = rawName.Substring(0, openRoundBracketPos);

                //_logger.Info($"rawName (3) = {rawName}");

                var parametersList = GetParametersList(strWithParameters);

                //_logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList, Formatting.Indented)}");

                memberName.ParametersList = parametersList;
            }

            if (rawName.Contains("{"))
            {
                throw new NotImplementedException();
            }

            if (rawName.Contains("["))
            {
                throw new NotImplementedException();
            }

            memberName.Name = rawName;

            memberName.FullName = $"{memberName.Path}.{memberName.Name}";

            return memberName;
        }

        public static List<string> GetParametersList(string inputStr)
        {
            var commaPos = inputStr.IndexOf(",");

            if (commaPos == -1)
            {
                return new List<string>() { inputStr.Trim() };
            }

            var figureBracketPos = inputStr.IndexOf("{");

            if (figureBracketPos == -1)
            {
                return inputStr.Split(",").Select(p => p.Trim()).ToList();
            }

            var result = new List<string>();

            var count = 0;
            var sb = new StringBuilder();

            foreach(var ch in inputStr)
            {
                if(ch == '{')
                {
                    count++;
                }
                else
                {
                    if(ch == '}')
                    {
                        count--;
                    }
                }

                if(count == 0)
                {
                    if(ch == ',')
                    {
                        result.Add(sb.ToString().Trim());

                        sb = new StringBuilder();
                    }
                    else
                    {
                        sb.Append(ch);
                    }                    
                }
                else
                {
                    sb.Append(ch);
                }
            }

            result.Add(sb.ToString().Trim());

            return result;
        }

        private static int LastDotPos(string name)
        {
            var openRoundBracketPos = name.IndexOf("(");

            var newDotPos = -1;
            var dotPos = -1;

            while ((newDotPos = name.IndexOf(".", newDotPos + 1)) != -1)
            {
                if (openRoundBracketPos == -1)
                {
                    dotPos = newDotPos;
                }
                else
                {
                    if (newDotPos < openRoundBracketPos)
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

        private static int LastSharpPos(string name)
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
