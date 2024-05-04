using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class ConfigAppSettingsHelper
    {
        private static readonly string[] _allKeys;
        private static readonly Dictionary<string, string> _valueCache = new Dictionary<string, string>();

        static ConfigAppSettingsHelper()
        {
            _allKeys = ConfigurationManager.AppSettings.AllKeys;
        }

        public static string GetExistingFileName(string key)
        {
            return GetValue(key, (string targetKey, string val) => File.Exists(val), true);
        }

        public static string GetExistingDirectoryName(string key)
        {
            return GetValue(key, (string targetKey, string val) => Directory.Exists(val), true);
        }

        public static string GetValue(string key, Func<string, string, bool> fitFun, bool useEvNormalize)
        {
            if (_valueCache.ContainsKey(key))
            {
                return _valueCache[key];
            }

            var primaryValue = ConfigurationManager.AppSettings[key];

            if (useEvNormalize)
            {
                primaryValue = EVPath.Normalize(primaryValue);
            }

            if (fitFun(key, primaryValue))
            {
                _valueCache[key] = primaryValue;
                return primaryValue;
            }

            var targetKeys = _allKeys.Where(p => p.StartsWith(key)).ToList();
            var resultKeys = new List<KeyValuePair<int, string>>();

            foreach (var targetKey in targetKeys)
            {
                var tail = targetKey.Replace(key, string.Empty);

                if (tail.StartsWith("_"))
                {
                    tail = tail.Substring(1);
                }

                if (string.IsNullOrWhiteSpace(tail))
                {
                    continue;
                }

                if (int.TryParse(tail, out int num))
                {
                    resultKeys.Add(new KeyValuePair<int, string>(num, targetKey));
                }
            }

            if (!resultKeys.Any())
            {
                _valueCache[key] = string.Empty;
                return string.Empty;
            }

            resultKeys = resultKeys.OrderBy(p => p.Key).ToList();

            foreach (var resultKey in resultKeys)
            {
                var secondaryValue = ConfigurationManager.AppSettings[resultKey.Value];

                if (useEvNormalize)
                {
                    secondaryValue = EVPath.Normalize(secondaryValue);
                }

                if (fitFun(resultKey.Value, secondaryValue))
                {
                    _valueCache[key] = secondaryValue;
                    return secondaryValue;
                }
            }

            _valueCache[key] = string.Empty;
            return string.Empty;
        }
    }
}
