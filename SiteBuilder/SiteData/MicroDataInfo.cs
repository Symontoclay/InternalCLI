using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class MicroDataInfo : IObjectToString
    {
        /// <summary>
        /// og:description, description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// og:image:secure_url, image
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// og:image:alt
        /// </summary>
        public string ImageAlt { get; set; } = string.Empty;

        /// <summary>
        /// og:title. If this title is empty I will take title of the page.
        /// </summary>
        public string Title { get; set; } = string.Empty;

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

            sb.AppendLine($"{spaces}{nameof(Description)} = {Description}");
            sb.AppendLine($"{spaces}{nameof(ImageUrl)} = {ImageUrl}");
            sb.AppendLine($"{spaces}{nameof(ImageAlt)} = {ImageAlt}");
            sb.AppendLine($"{spaces}{nameof(Title)} = {Title}");

            return sb.ToString();
        }
    }
}
