using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLDocReader.CSharpDoc;

namespace XMLDocReader
{
    public static class JsonSerializationHelper
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            TypeNameHandling = TypeNameHandling.All
        };

        public static string SerializeToString(PackageCard packageCard)
        {
            return JsonConvert.SerializeObject(packageCard, JsonSerializerSettings);
        }

        public static void SerializeToFile(PackageCard packageCard, string fileName)
        {
            var jsonStr = SerializeToString(packageCard);
            File.WriteAllText(fileName, jsonStr);
        }

        public static PackageCard DeserializeObjectFromString(string jsonStr)
        {
            return JsonConvert.DeserializeObject<PackageCard>(jsonStr, JsonSerializerSettings);
        }

        public static PackageCard DeserializeObjectFromFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            return DeserializeObjectFromString(text);
        }
    }
}
