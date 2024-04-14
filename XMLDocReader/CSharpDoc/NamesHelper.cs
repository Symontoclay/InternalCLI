using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class NamesHelper
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string SimplifyFullNameOfType(string inputName)
        {
            //_logger.Info($"inputName = {inputName}");

            var gravisPos = inputName.IndexOf("`");

            //_logger.Info($"gravisPos = {gravisPos}");

            if(gravisPos == -1)
            {
                return inputName.Trim();
            }

            return ParseGenericType(inputName);
        }

        private static string ParseGenericType(string inputName)
        {
            //_logger.Info($"inputName = {inputName}");

            var gravisPos = inputName.IndexOf("`");

            //_logger.Info($"gravisPos = {gravisPos}");

            var sb = new StringBuilder();

            var name = inputName.Substring(0, gravisPos);

            //_logger.Info($"name = {name}");

            sb.Append(name);
            sb.Append("{");

            var squareBracketPos = inputName.IndexOf("[");

            //_logger.Info($"squareBracketPos = {squareBracketPos}");

            var genericParamsStr = inputName.Substring(squareBracketPos);

            genericParamsStr = genericParamsStr.Substring(1, genericParamsStr.Length - 2);

            //_logger.Info($"genericParamsStr = {genericParamsStr}");

            var paramsList = DivideGenericParams(genericParamsStr);

            if(paramsList.Any())
            {
                sb.Append(string.Join(',', paramsList.Select(SimplifyFullNameOfType)));
            }

            sb.Append("}");

            //_logger.Info($"sb = '{sb}'");

            return sb.ToString();
        }

        private static List<string> DivideGenericParams(string inputName)
        {
            var result = new List<string>();

            var count = 0;
            var isNeedFlush = false;

            var sb = new StringBuilder();

            foreach (var ch in inputName)
            {
                //_logger.Info($"ch = '{ch}'");

                if(ch == '[')
                {
                    count++;
                }
                else
                {
                    if(ch == ']')
                    {
                        count--;
                    }
                }

                //_logger.Info($"count = {count}");
                //_logger.Info($"isNeedFlush = {isNeedFlush}");

                if (count == 0)
                {
                    if(isNeedFlush)
                    {
                        var str = sb.ToString().Substring(1);

                        //_logger.Info($"str = '{str}'");

                        str = RemoveTail(str);

                        //_logger.Info($"str (2) = '{str}'");

                        sb = new StringBuilder();

                        result.Add(str);

                        isNeedFlush = false;
                    }
                }
                else
                {
                    isNeedFlush = true;

                    sb.Append(ch);
                }
            }

            return result;
        }

        private static string RemoveTail(string inputName)
        {
            var charsList = inputName.ToList();

            charsList.Reverse();

            var count = 0;

            var newCharsList = new List<char>();

            foreach (var ch in charsList)
            {
                //_logger.Info($"ch = '{ch}'");

                if(ch == ',')
                {
                    count++;
                }

                //_logger.Info($"count = {count}");

                if(count > 3)
                {
                    newCharsList.Add(ch);
                }
            }

            newCharsList.Reverse();

            var str = new string(newCharsList.ToArray()).Trim();

            return str.Substring(0, str.Length - 1);
        }
    }
}
