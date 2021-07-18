using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public class FutureReleaseInfo : IObjectToString
    {
        public string Version { get; set; }
        public string Description { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Version)} = {Version}");
            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");

            return sb.ToString();
        }

        public static FutureReleaseInfo ReadFile(string fileName)
        {
            return JsonConvert.DeserializeObject<FutureReleaseInfo>(File.ReadAllText(fileName));
        }

        public static void SaveExampleFile(string fileName)
        {
            var futureReleaseInfo = new FutureReleaseInfo()
            {
                Version = "0.0.0",
                Description = "Example of release description."
            };

            var jsonStr = JsonConvert.SerializeObject(futureReleaseInfo, Formatting.Indented);

            File.WriteAllText(fileName, jsonStr);
        }
    }
}
