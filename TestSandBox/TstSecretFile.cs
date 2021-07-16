using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public static class TstSecretFile
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static Dictionary<string, string> ReadSecrets(string fileName)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(fileName));
        }

        public static void WriteExample(string fileName)
        {
            var dict = new Dictionary<string, string>() { { "Key1", "ExampleSecret1" }, { "Key2", "ExampleSecret2" } };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(dict, Formatting.Indented));
        }
    }
}
