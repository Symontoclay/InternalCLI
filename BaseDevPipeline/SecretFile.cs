using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class SecretFile
    {
        public static string ReadKey(string fileName, string key)
        {
            return ReadSecrets(fileName)[key];
        }

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
