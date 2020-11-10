using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder.HtmlPreprocessors
{
    public static class HtmlDocumentHelpers
    {
        public static string ToHtmlString(this HtmlDocument doc)
        {
            if(doc == null)
            {
                return string.Empty;
            }

            var strWriter = new StringWriter();
            doc.Save(strWriter);

            return strWriter.ToString();
        }
    }
}
