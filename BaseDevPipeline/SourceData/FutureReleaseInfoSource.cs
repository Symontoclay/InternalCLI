using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using SymOntoClay.Common;
using System;
using System.IO;
using System.Text;

namespace BaseDevPipeline.SourceData
{
    public class FutureReleaseInfoSource : IObjectToString
    {
        public string Version { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(StartDate)} = {StartDate}");
            sb.AppendLine($"{spaces}{nameof(FinishDate)} = {FinishDate}");

            return sb.ToString();
        }

        public static FutureReleaseInfoSource ReadFile(string fileName)
        {
            return JsonConvert.DeserializeObject<FutureReleaseInfoSource>(File.ReadAllText(fileName));
        }

        public static void SaveExampleFile(string fileName)
        {
            var futureReleaseInfo = new FutureReleaseInfoSource()
            {
                Version = "0.0.0",
                Status = "Started",
                StartDate = DateTime.Now
            };

            SaveFile(fileName, futureReleaseInfo);
        }

        public static void SaveFile(string fileName, FutureReleaseInfoSource futureReleaseInfo)
        {
            var jsonStr = JsonConvert.SerializeObject(futureReleaseInfo, Formatting.Indented);

            File.WriteAllText(fileName, jsonStr);
        }
    }
}
