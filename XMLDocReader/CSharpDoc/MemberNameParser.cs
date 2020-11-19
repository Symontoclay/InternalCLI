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
            //_logger.Info($"initialName = {initialName}");

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

            if (initialNameWithoutType.Contains("{") && memberName.Kind == KindOfMember.Type)
            {
                ReadAsGenericMemberName(initialNameWithoutType, memberName);
                FillUpDisplayedName(memberName);
                return memberName;
            }

            if (initialNameWithoutType.Contains("[") && !initialNameWithoutType.Contains("("))
            {
                throw new NotImplementedException();
            }

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

            FillUpDisplayedName(memberName);

            return memberName;
        }

        private static void FillUpDisplayedName(MemberName memberName)
        {
            switch (memberName.Kind)
            {
                case KindOfMember.Type:
                    memberName.DisplayedName = GetDisplayedNameForType(memberName.Name, memberName.TypeParametersList);
                    break;

                case KindOfMember.Property:
                case KindOfMember.Field:
                case KindOfMember.Event:
                    memberName.DisplayedName = memberName.Name;
                    break;

                case KindOfMember.Method:
                    memberName.DisplayedName = GetDisplayedNameForMethod(memberName.Name, memberName.TypeParametersList, memberName.ParametersList);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(memberName.Kind), memberName.Kind, null);
            }

#if DEBUG
            //_logger.Info($"memberName.DisplayedName = '{memberName.DisplayedName}'");
#endif
        }

        private static string GetDisplayedNameForType(string name, List<string> typeParametersList)
        {
#if DEBUG
            //_logger.Info($"name = '{name}'");
            //_logger.Info($"typeParametersList = {JsonConvert.SerializeObject(typeParametersList)}");
#endif

            if(!typeParametersList.Any())
            {
                return name;
            }

            var sb = new StringBuilder(name);

            sb.Append("<");

            var resultParamsList = new List<string>();

            foreach (var parameter in typeParametersList)
            {
#if DEBUG
                //_logger.Info($"parameter = '{parameter}'");
#endif

                var paramName = Parse($"T:{parameter}");

#if DEBUG
                //_logger.Info($"paramName = {paramName}");
#endif

                resultParamsList.Add(paramName.DisplayedName);
            }

            sb.Append(string.Join(',', resultParamsList));
            sb.Append(">");

#if DEBUG
            //_logger.Info($"sb = {sb}");
#endif

            return sb.ToString();
        }

        private static string GetDisplayedNameForMethod(string name, List<string> typeParametersList, List<string> parametersList)
        {
#if DEBUG
            //_logger.Info($"name = '{name}'");
            //_logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList)}");
#endif

            var sb = new StringBuilder(name);

            if(!parametersList.Any())
            {
                sb.Append("()");
                return sb.ToString();
            }

            var resultParamsList = new List<string>();

            if(typeParametersList.Any())
            {
                sb.Append("<");

                foreach (var parameter in typeParametersList)
                {
#if DEBUG
                    //_logger.Info($"parameter = '{parameter}'");
#endif

                    var paramName = Parse($"T:{parameter}");

#if DEBUG
                    //_logger.Info($"paramName = {paramName}");
#endif

                    resultParamsList.Add(paramName.DisplayedName);
                }

                sb.Append(string.Join(',', resultParamsList));
                sb.Append(">");
            }

            sb.Append("(");

            resultParamsList = new List<string>();

            foreach(var parameter in parametersList)
            {
#if DEBUG
                //_logger.Info($"parameter = '{parameter}'");
#endif

                var paramName = Parse($"T:{parameter}");

#if DEBUG
                //_logger.Info($"paramName = {paramName}");
#endif

                resultParamsList.Add(paramName.DisplayedName);
            }

            sb.Append(string.Join(',', resultParamsList));
            sb.Append(")");

#if DEBUG
            //_logger.Info($"sb = {sb}");
#endif

            return sb.ToString();
        }

        private static void ReadAsGenericMemberName(string initialNameWithoutType, MemberName memberName)
        {
            //_logger.Info($"initialNameWithoutType = {initialNameWithoutType}");

            var lastDotPos = LastDotPosWihTypePameters(initialNameWithoutType);

            //_logger.Info($"lastDotPos = {lastDotPos}");

            var path = initialNameWithoutType.Substring(0, lastDotPos).Trim();

            //_logger.Info($"path = {path}");

            memberName.Path = path;

            var rawName = initialNameWithoutType.Substring(lastDotPos + 1).Trim();

            //_logger.Info($"rawName = {rawName}");

            var openFigureBracketPos = rawName.IndexOf("{");

            memberName.Name = rawName.Substring(0, openFigureBracketPos).Trim();

            var strWithParameters = rawName.Substring(openFigureBracketPos + 1, rawName.Length - openFigureBracketPos - 2);

            //_logger.Info($"strWithParameters = {strWithParameters}");

            memberName.TypeParametersList = GetParametersList(strWithParameters);

            //_logger.Info($"memberName (after) = {memberName}");
        }

        private static int LastDotPosWihTypePameters(string inputStr)
        {
            var result = -1;
            var i = -1;

            foreach (var ch in inputStr)
            {
                i++;

                if(ch == '.')
                {
                    result = i;
                }
                else
                {
                    if(ch == '{')
                    {
                        return result;
                    }
                }
            }

            return result;
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
