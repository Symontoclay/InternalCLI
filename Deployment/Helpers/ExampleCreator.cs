using CommonUtils;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class ExampleCreator
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static ExampleCreatorResult CreateExample(CodeExample example, string baseDir, string socExePath)
        {
#if DEBUG
            //_logger.Info($"example = {example}");
            //_logger.Info($"baseDir = {baseDir}");
            _logger.Info($"socExePath = {socExePath}");
#endif

            var newProcessSyncWrapper = new ProcessSyncWrapper(socExePath, "new Example", baseDir);

            var exitCode = newProcessSyncWrapper.Run();

#if DEBUG
            //_logger.Info($"exitCode = {exitCode}");
#endif

            if(exitCode != 0)
            {
                throw new Exception($"Exit code is {exitCode}!");
            }

            var mainSocFileName = Path.Combine(baseDir, "Example", "Npcs", "Example", "Example.soc");

#if DEBUG
            //_logger.Info($"mainSocFileName = {mainSocFileName}");
#endif

#if DEBUG
            _logger.Info($"example.Code.Trim() = {example.Code.Trim()}");
#endif

            File.WriteAllText(mainSocFileName, example.Code.Trim());

            var exampleDir = Path.Combine(baseDir, "Example");

#if DEBUG
            //_logger.Info($"exampleDir = {exampleDir}");
#endif

            var runProcessSyncWrapper = new ProcessSyncWrapper(socExePath, "run -nologo -timeout 5000", exampleDir);

            exitCode = runProcessSyncWrapper.Run();

#if DEBUG
            _logger.Info($"exitCode = {exitCode}");
            _logger.Info($"runProcessSyncWrapper.Output = {JsonConvert.SerializeObject(runProcessSyncWrapper.Output, Formatting.Indented)}");
#endif

            if (exitCode != 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Unable to run example '{example.Name}':");
                sb.AppendLine(example.Code);
                sb.AppendLine($"Exit code is {exitCode}!");
                throw new Exception(sb.ToString());
            }

            var someLogFile = Directory.GetFiles(exampleDir).SingleOrDefault(p => p.EndsWith(".log"));

#if DEBUG
            //_logger.Info($"someLogFile = {someLogFile}");
#endif

            if(!string.IsNullOrWhiteSpace(someLogFile))
            {
                File.Delete(someLogFile);
            }            

            var targetZipFileName = Path.Combine(baseDir, $"{example.Name}.zip");

#if DEBUG
            //_logger.Info($"targetZipFileName = {targetZipFileName}");
#endif

            ZipFile.CreateFromDirectory(exampleDir, targetZipFileName);

            var consoleFileName = Path.Combine(baseDir, $"{example.Name}.console");

#if DEBUG
            //_logger.Info($"consoleFileName = {consoleFileName}");
#endif

            var consoleSb = new StringBuilder();

            foreach(var line in runProcessSyncWrapper.Output)
            {
#if DEBUG
                _logger.Info($"line = {line}");
#endif

                if (DetectException(line))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Unable to run example '{example.Name}':");
                    sb.AppendLine(example.Code);
                    sb.AppendLine("The suspicious otput:");
                    sb.AppendLine(line);
                    throw new Exception(sb.ToString());
                }

                consoleSb.AppendLine(NormalizeTextForConsole(line));
            }

#if DEBUG
            //_logger.Info($"consoleSb = {consoleSb}");
#endif

            File.WriteAllText(consoleFileName, consoleSb.ToString());

            Directory.Delete(exampleDir, true);

            return new ExampleCreatorResult() 
            { 
                ArchFileName = targetZipFileName,
                ConsoleFileName = consoleFileName
            };
        }

        private static bool DetectException(string source)
        {
            return (source.Contains("System.") || source.Contains("at SymOntoClay.")) && (source.Contains(@"in C:\") || source.Contains(@"in D:\")) && source.Contains(".cs:line ");
        }

        private static string NormalizeTextForConsole(string source)
        {
            return source.Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
