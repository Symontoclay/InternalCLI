using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    public static class UnityHelper
    {
        public static string GetTargetUnityVersion(string rootPath)
        {
            var targetProjectPath = Path.Combine(rootPath, "ProjectSettings", "ProjectVersion.txt");

            if (!File.Exists(targetProjectPath))
            {
                return string.Empty;
            }

            var linesList = File.ReadAllLines(targetProjectPath);

            var line = linesList.SingleOrDefault(p => p.Contains("m_EditorVersion:"));

            return line.Replace("m_EditorVersion:", string.Empty).Trim();
        }
    }
}
