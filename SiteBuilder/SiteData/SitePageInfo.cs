using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class SitePageInfo : IObjectToString
    {
        public string Extension { get; set; } = "html";
        public string ContentPath { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BreadcrumbTitle { get; set; } = string.Empty;
        public bool IsBreadcrumbRoot { get; set; }
        public string AdditionalMenu { get; set; }
        public bool EnableMathML { get; set; }
        public bool UseMarkdown { get; set; }
        public bool IsReady { get; set; }
        public MicroDataInfo Microdata { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(Extension)} = {Extension}");
            sb.AppendLine($"{spaces}{nameof(ContentPath)} = {ContentPath}");
            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");
            sb.AppendLine($"{spaces}{nameof(BreadcrumbTitle)} = {BreadcrumbTitle}");
            sb.AppendLine($"{spaces}{nameof(IsBreadcrumbRoot)} = {IsBreadcrumbRoot}");
            sb.AppendLine($"{spaces}{nameof(AdditionalMenu)} = {AdditionalMenu}");
            sb.AppendLine($"{spaces}{nameof(EnableMathML)} = {EnableMathML}");
            sb.AppendLine($"{spaces}{nameof(UseMarkdown)} = {UseMarkdown}");
            sb.AppendLine($"{spaces}{nameof(IsReady)} = {IsReady}");

            sb.PrintObjProp(n, nameof(Microdata), Microdata);

            return sb.ToString();
        }

        public static SitePageInfo LoadFromFile(string path)
        {
            var result = JsonConvert.DeserializeObject<SitePageInfo>(File.ReadAllText(path));

            if(result.Microdata == null)
            {
                result.Microdata = new MicroDataInfo();
            }

            return result;
        }
    }
}
