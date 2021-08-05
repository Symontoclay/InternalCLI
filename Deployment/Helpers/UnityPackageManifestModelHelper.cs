using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class UnityPackageManifestModelHelper
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettingsForCompactFile = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public static UnityPackageManifestModel Read(string fileName)
        {
            return JsonConvert.DeserializeObject<UnityPackageManifestModel>(File.ReadAllText(fileName));
        }

        public static void SaveCompactFile(string fileName, UnityPackageManifestModel manifest)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(manifest, Formatting.Indented, _jsonSerializerSettingsForCompactFile));
        }

        public static void SaveExampleFile(string fileName)
        {
            var manifest = new UnityPackageManifestModel
            {
                name = "com.unity.example",
                version = "1.2.3",
                displayName = "Package Example",
                description = "This is an example package",
                unity = "2019.1",
                documentationUrl = "https://example.com/",
                changelogUrl = "https://example.com/changelog.html",
                licensesUrl = "https://example.com/licensing.html",
                dependencies = new Dictionary<string, string>() { { "com.unity.some-package", "1.0.0" }, { "com.unity.other-package", "2.0.0" } },
                keywords = new List<string>() { "keyword1", "keyword2", "keyword3" },
                author = new AuthorOfUnityPackageManifestModel()
                {
                    name = "Unity",
                    email = "unity@example.com",
                    url = "https://www.unity3d.com"
                }
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }

        public static void SaveExampleFileOnlyWithRequiredAndMandatoryProperties(string fileName)
        {
            var manifest = new UnityPackageManifestModel
            {
                name = "com.unity.example",
                version = "1.2.3",
                displayName = "Package Example",
                description = "This is an example package",
                unity = "2019.1"
            };

            SaveCompactFile(fileName, manifest);
        }
    }
}
