using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.InThePageContentGen
{
    public class ContentItem
    {
        public string TagName { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public List<ContentItem> Items { get; set; } = new List<ContentItem>();
    }
}
