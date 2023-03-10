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
        public static SecretInfo ReadKey(string fileName, string key)
        {
            return ReadSecrets(fileName)[key];
        }

        public static Dictionary<string, SecretInfo> ReadSecrets(string fileName)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, SecretInfo>>(File.ReadAllText(fileName));
        }

        public static void WriteExample(string fileName)
        {
            var dict = new Dictionary<string, SecretInfo>() { { "Key1", new SecretInfo { Value = "ExampleSecret1", ExpDate = DateTime.Now } }, { "Key2", new SecretInfo { Value = "ExampleSecret2", ExpDate = DateTime.Now }  } };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(dict, Formatting.Indented));
        }
    }
}
