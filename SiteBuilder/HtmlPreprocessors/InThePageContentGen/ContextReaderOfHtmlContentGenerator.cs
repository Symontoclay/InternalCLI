using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors.InThePageContentGen
{
    public class ContextReaderOfHtmlContentGenerator
    {
        public bool NeedProcess;
        public List<ContentItem> ContentItemsList = new List<ContentItem>();
        public HtmlNode ContentPlaceNode { get; set; }
    }
}
