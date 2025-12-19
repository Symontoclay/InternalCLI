using NLog;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiteBuilder
{
    public static class PDFCreator
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static void CreateFile(string fileName, string title, string content, string executablePath, string fontPath)
        {
            CreateFileAsync(
                fileName: fileName,
                title: title,
                content: content,
                executablePath: executablePath,
                fontPath: fontPath
            ).GetAwaiter().GetResult();
        }

        public static async Task CreateFileAsync(string fileName, string title, string content, string executablePath, string fontPath)
        {
#if DEBUG
            _logger.Info($"fileName = {fileName}");
            _logger.Info($"title = {title}");
            _logger.Info($"content = {content}");
            _logger.Info($"executablePath = {executablePath}");
            _logger.Info($"fontPath = {fontPath}");
#endif

            var css = @"
body {
    font-family: 'Liberation Serif', serif;
    font-size: 10px;       
    line-height: 1.0;      
    margin-top: 0;
    margin-left: 25mm;
    margin-right: 25mm;
    margin-bottom: 25mm;
    padding: 0;
}

p {
    text-align: justify;
}

h1, h2, h3 {
    font-weight: bold;
    margin-top: 12px;
    margin-bottom: 6px;
}

h1 { font-size: 14px; }
h2 { font-size: 12px; }
h3 { font-size: 11px; }

table {
    border-collapse: collapse;
    margin: 10px 0;
    font-size: 10px;
}

th, td {
    border: 1px solid #ccc;
    padding: 4px 8px;
}

pre {
  margin: 0;
  padding: 0.6em;
  line-height: 1.4;
}
code {
  font-family: 'Liberation Mono', monospace;
  font-size: 10px;
}
";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
/* Main text */
@font-face {{
  font-family: 'Liberation Serif';
  src: url('file:///{fontPath}/LiberationSerif-Regular.ttf') format('truetype');
  font-weight: normal;
  font-style: normal;
}}
@font-face {{
  font-family: 'Liberation Serif';
  src: url('file:///{fontPath}/LiberationSerif-Bold.ttf') format('truetype');
  font-weight: bold;
  font-style: normal;
}}
@font-face {{
  font-family: 'Liberation Serif';
  src: url('file:///{fontPath}/LiberationSerif-Italic.ttf') format('truetype');
  font-weight: normal;
  font-style: italic;
}}

/* Code blocks */
@font-face {{
  font-family: 'Liberation Mono';
  src: url('file:///{fontPath}/LiberationMono-Regular.ttf') format('truetype');
  font-weight: normal;
  font-style: normal;
}}
@font-face {{
  font-family: 'Liberation Mono';
  src: url('file:///{fontPath}/LiberationMono-Bold.ttf') format('truetype');
  font-weight: bold;
  font-style: normal;
}}
@font-face {{
  font-family: 'Liberation Mono';
  src: url('file:///{fontPath}/LiberationMono-Italic.ttf') format('truetype');
  font-weight: normal;
  font-style: italic;
}}

        {css}
    </style>
    <title>{title}</title>
</head>
<body>
{content}
</body>
</html>";

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = executablePath
            });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            if(File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            await page.PdfAsync(fileName, new PdfOptions
            {
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions { Top = "60px", Bottom = "60px", Left = "40px", Right = "40px" },
                DisplayHeaderFooter = true,
                HeaderTemplate = "<div></div>",
                FooterTemplate = "<div style='font-size:10px; text-align:center;width:100%;'>Page <span class=\"pageNumber\"></span> of <span class=\"totalPages\"></span></div>"
            });
        }
    }
}
