//using NLog;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace CommonUtils
//{
//    [Obsolete("Use SymOntoClay.Common.EVPath")]
//    public static class EVPath
//    {
//        private static Regex _normalizeMatch = new Regex("(%(\\w|\\(|\\))+%)");
//        private static Regex _normalizeMatch2 = new Regex("(\\w|\\(|\\))+");

//        static EVPath()
//        {
//            RegVar("APPDIR", Directory.GetCurrentDirectory());
//        }

//        public static string Normalize(string sourcePath)
//        {
//            if(string.IsNullOrWhiteSpace(sourcePath))
//            {
//                return string.Empty;
//            }

//            var match = _normalizeMatch.Match(sourcePath);

//            if (match.Success)
//            {
//                var targetValue = match.Value;

//                var match2 = _normalizeMatch2.Match(targetValue);

//                if (match2.Success)
//                {
//                    var variableName = match2.Value;

//                    var variableValue = string.Empty;

//                    if(_additionalVariablesDict.ContainsKey(variableName))
//                    {
//                        variableValue = _additionalVariablesDict[variableName];
//                    }
//                    else
//                    {
//                        variableValue = Environment.GetEnvironmentVariable(variableName);
//                    }

//                    if (!string.IsNullOrWhiteSpace(variableValue))
//                    {
//                        sourcePath = sourcePath.Replace(targetValue, variableValue);
//                    }
//                }
//            }

//            var fullPath = Path.GetFullPath(sourcePath);

//            var colonPos = fullPath.IndexOf(":", 5);

//            if(colonPos == -1)
//            {
//                return fullPath;
//            }

//            var backSlashPos = DetectBackSlachPos(fullPath, colonPos);

//            return fullPath.Substring(backSlashPos + 1);
//        }

//        private static int DetectBackSlachPos(string value, int colonPos)
//        {
//            for(var i = colonPos; i >= 0; i--)
//            {
//                var ch = value[i];

//                if(ch == '/' || ch == '\\')
//                {
//                    return i;
//                }
//            }

//            return 0;
//        }

//        public static void RegVar(string varName, string varValue)
//        {
//            _additionalVariablesDict[varName] = varValue;
//        }

//        private static readonly Dictionary<string, string> _additionalVariablesDict = new Dictionary<string, string>();
//    }
//}
