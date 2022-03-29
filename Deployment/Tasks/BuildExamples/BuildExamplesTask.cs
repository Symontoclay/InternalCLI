using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using NLog;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ExamplesCreator
{
    public class BuildExamplesTask : BaseDeploymentTask
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public BuildExamplesTask(BuildExamplesTaskOptions options)
            : this(options, 0u)
        {
        }

        public BuildExamplesTask(BuildExamplesTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly BuildExamplesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateList(nameof(_options.LngExamplesPages), _options.LngExamplesPages);
            ValidateFileName(nameof(_options.SocExePath), _options.SocExePath);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            ClearDestDir();

            using var tempDir = new TempDirectory();

            foreach (var lngExamplesPage in _options.LngExamplesPages)
            {
#if DEBUG
                //_logger.Info($"lngExamplesPage = {lngExamplesPage}");
#endif

                var examplesList = CodeExampleReader.Read(lngExamplesPage);

#if DEBUG
                //_logger.Info($"examplesList.Count = {examplesList.Count}");
#endif
                foreach (var example in examplesList)
                {
#if DEBUG
                    //_logger.Info($"example = {example}");
#endif

                    var result = ExampleCreator.CreateExample(example, tempDir.FullName, _options.SocExePath);

#if DEBUG
                    //_logger.Info($"result = {result}");
#endif

                    var newArchFileName = Path.Combine(_options.DestDir, Path.GetFileName(result.ArchFileName));

#if DEBUG
                    //_logger.Info($"newArchFileName = {newArchFileName}");
#endif

                    File.Copy(result.ArchFileName, newArchFileName);

                    var newConsoleFileName = Path.Combine(_options.DestDir, Path.GetFileName(result.ConsoleFileName));

#if DEBUG
                    //_logger.Info($"newConsoleFileName = {newConsoleFileName}");
#endif

                    File.Copy(result.ConsoleFileName, newConsoleFileName);
                }
            }
        }

        private void ClearDestDir()
        {
            var cleanedFiles = Directory.GetFiles(_options.DestDir).Where(p => p.EndsWith(".zip") || p.EndsWith(".console")).ToList();

            foreach(var cleanedFile in cleanedFiles)
            {
#if DEBUG
                _logger.Info($"cleanedFile = {cleanedFile}");
#endif

                File.Delete(cleanedFile);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = DisplayHelper.Spaces(nextN);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds examples to '{_options.DestDir}'.");
            sb.AppendLine($"{spaces}Source pages:");

            foreach(var lngExamplesPage in _options.LngExamplesPages)
            {
                sb.AppendLine($"{nextNSpaces}{lngExamplesPage}");
            }

            sb.AppendLine($"{spaces}Using CLI on '{_options.SocExePath}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
