using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.InThePageContentGen
{
    public class ReaderResultOfHtmlContentGenerator
    {
        public HtmlNode ContentPlaceNode { get; set; }
        public List<ContentItem> Items { get; set; } = new List<ContentItem>();
    }
}
