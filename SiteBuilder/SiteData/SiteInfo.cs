using CommonUtils;
using Newtonsoft.Json;
using SymOntoClay.Common;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class SiteInfo : IObjectToString
    {
        public MenuInfo Menu { get; set; }
        public string MainTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitlesDelimiter { get; set; } = string.Empty;
        public bool EnableFavicon { get; set; }
        public string Logo { get; set; } = string.Empty;
        public string RoadMapJsonPath { get; set; } = string.Empty;
        public string ReleaseNotesJsonPath { get; set; } = string.Empty;
        public string BaseReleaseNotesPath { get; set; } = string.Empty;
        public string CSharpUserApiJsonPath { get; set; } = string.Empty;
        public string DestCSharpUserApiPath { get; set; } = string.Empty;
        public string DestKeyFeaturesPath { get; set; } = string.Empty;
        public string LngExamplesPath { get; set; } = string.Empty;
        public string DestLngExamplesPath { get; set; } = string.Empty;
        public string LngExamplesCachePath { get; set; } = string.Empty;
        public List<string> LngExamplesPages { get; set; }
        public MicroDataInfo Microdata { get; set; }
        public List<string> IgnoredDirs { get; set; }

        private void Init()
        {
            if (!string.IsNullOrWhiteSpace(RoadMapJsonPath))
            {
                RoadMapJsonPath = EVPath.Normalize(RoadMapJsonPath);
            }

            if(!string.IsNullOrWhiteSpace(ReleaseNotesJsonPath))
            {
                ReleaseNotesJsonPath = EVPath.Normalize(ReleaseNotesJsonPath);
            }

            if(!string.IsNullOrWhiteSpace(BaseReleaseNotesPath))
            {
                BaseReleaseNotesPath = EVPath.Normalize(BaseReleaseNotesPath);
            }

            if (!string.IsNullOrWhiteSpace(CSharpUserApiJsonPath))
            {
                CSharpUserApiJsonPath = EVPath.Normalize(CSharpUserApiJsonPath);
            }

            if (!string.IsNullOrWhiteSpace(LngExamplesPath))
            {
                LngExamplesPath = EVPath.Normalize(LngExamplesPath);
            }

            if (!string.IsNullOrWhiteSpace(LngExamplesCachePath))
            {
                LngExamplesCachePath = EVPath.Normalize(LngExamplesCachePath);
            }

            if (!LngExamplesPages.IsNullOrEmpty())
            {
                LngExamplesPages = LngExamplesPages.Select(p => EVPath.Normalize(p)).ToList();
            }

            if (!IgnoredDirs.IsNullOrEmpty())
            {
                IgnoredDirs = IgnoredDirs.Select(p => EVPath.Normalize(p)).ToList();
            }

            //if(!string.IsNullOrWhiteSpace(DestCSharpUserApiPath))
            //{
            //    DestCSharpUserApiPath = DestCSharpUserApiPath;
            //}

            if (Microdata == null)
            {
                Microdata = new MicroDataInfo();
            }
        }

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

            sb.PrintObjProp(n, nameof(Menu), Menu);
            sb.AppendLine($"{spaces}{nameof(MainTitle)} = {MainTitle}");
            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(TitlesDelimiter)} = {TitlesDelimiter}");
            sb.AppendLine($"{spaces}{nameof(EnableFavicon)} = {EnableFavicon}");
            sb.AppendLine($"{spaces}{nameof(Logo)} = {Logo}");
            sb.AppendLine($"{spaces}{nameof(RoadMapJsonPath)} = {RoadMapJsonPath}");
            sb.AppendLine($"{spaces}{nameof(ReleaseNotesJsonPath)} = {ReleaseNotesJsonPath}");
            sb.AppendLine($"{spaces}{nameof(BaseReleaseNotesPath)} = {BaseReleaseNotesPath}");
            sb.AppendLine($"{spaces}{nameof(CSharpUserApiJsonPath)} = {CSharpUserApiJsonPath}");
            sb.AppendLine($"{spaces}{nameof(DestCSharpUserApiPath)} = {DestCSharpUserApiPath}");
            sb.AppendLine($"{spaces}{nameof(DestKeyFeaturesPath)} = {DestKeyFeaturesPath}");
            sb.AppendLine($"{spaces}{nameof(LngExamplesPath)} = {LngExamplesPath}");
            sb.AppendLine($"{spaces}{nameof(DestLngExamplesPath)} = {DestLngExamplesPath}");
            sb.PrintPODList(n, nameof(IgnoredDirs), IgnoredDirs);

            sb.PrintObjProp(n, nameof(Microdata), Microdata);

            return sb.ToString();
        }

        public static SiteInfo LoadFromFile(string path)
        {
            var result = JsonConvert.DeserializeObject<SiteInfo>(File.ReadAllText(path));
            result.Init();
            return result;
        }
    }
}
