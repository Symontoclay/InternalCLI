using BaseDevPipeline;
using BaseDevPipeline.Data.Implementation;
using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.BuildExamples;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.ExamplesCreator
{
    public class BuildExamplesTask : BaseDeploymentTask
    {
        public BuildExamplesTask(BuildExamplesTaskOptions options)
            : this(options, null)
        {
        }

        public BuildExamplesTask(BuildExamplesTaskOptions options, IDeploymentTask parentTask)
            : base("740B1C0A-D30A-4081-9986-0EF9BB9C9F4A", false, options, parentTask)
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
            var cacheDir = _options.CacheDir;

#if DEBUG
            //_logger.Info($"cacheDir = {cacheDir}");
#endif

            var hasCache = !string.IsNullOrWhiteSpace(cacheDir);

#if DEBUG
            //_logger.Info($"hasCache = {hasCache}");
#endif

            if (!hasCache)
            {
                ClearDestDir();
            }

            var tempSettings = ProjectsDataSourceFactory.GetTempSettings();

            using var tempDir = new TempDirectory(tempSettings.Dir, tempSettings.ClearOnDispose);

#if DEBUG
            //_logger.Info($"_options.DestDir = {_options.DestDir}");
#endif

            var targetFiles = new List<string>();

            foreach (var lngExamplesPage in _options.LngExamplesPages)
            {
#if DEBUG
                //_logger.Info($"lngExamplesPage = {lngExamplesPage}");
#endif

                var longestBasePath = PathsHelper.GetLongestCommonPath(lngExamplesPage, _options.DestDir);

#if DEBUG
                //_logger.Info($"longestBasePath = '{longestBasePath}'");
#endif

                var preparedFileName = lngExamplesPage.Replace(longestBasePath, string.Empty);

#if DEBUG
                //_logger.Info($"preparedFileName = '{preparedFileName}'");
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

                    if(hasCache)
                    {
                        var needToBuild = ExampleCacheHelper.IsNeedToBuild(example, preparedFileName, cacheDir);

#if DEBUG
                        //_logger.Info($"needToBuild = {needToBuild}");
#endif

                        var fullZipFileName = Path.Combine(_options.DestDir, ExampleCreator.GetZipFileName(example));
                        var fullConsoleFileName = Path.Combine(_options.DestDir, ExampleCreator.GetConsoleFileName(example));

                        targetFiles.Add(fullZipFileName);
                        targetFiles.Add(fullConsoleFileName);

                        if(!needToBuild)
                        {
                            if(!File.Exists(fullZipFileName))
                            {
                                needToBuild = true;
                            }

                            if (!File.Exists(fullConsoleFileName))
                            {
                                needToBuild = true;
                            }
                        }

                        if (needToBuild)
                        {
                            ClearOldExample(example);

                            var createdFiles = CreateExample(example, tempDir);

                            targetFiles.AddRange(createdFiles);

                            ExampleCacheHelper.SaveToCache(example, preparedFileName, cacheDir);
                        }
                    }
                    else
                    {
                        CreateExample(example, tempDir);
                    }
                }
            }

            if(targetFiles.Any())
            {
                ClearDestDir(targetFiles);
            }
        }

        private List<string> CreateExample(CodeExample example, TempDirectory tempDir)
        {
            var createdFiles = new List<string>();

            var result = ExampleCreator.CreateExample(example, tempDir.FullName, _options.SocExePath);

#if DEBUG
            //_logger.Info($"result = {result}");
#endif

            var newArchFileName = Path.Combine(_options.DestDir, Path.GetFileName(result.ArchFileName));

#if DEBUG
            //_logger.Info($"newArchFileName = {newArchFileName}");
#endif

            createdFiles.Add(newArchFileName);

            File.Copy(result.ArchFileName, newArchFileName);

            var newConsoleFileName = Path.Combine(_options.DestDir, Path.GetFileName(result.ConsoleFileName));

#if DEBUG
            //_logger.Info($"newConsoleFileName = {newConsoleFileName}");
#endif

            createdFiles.Add(newConsoleFileName);

            File.Copy(result.ConsoleFileName, newConsoleFileName);

            return createdFiles;
        }

        private void ClearOldExample(CodeExample example)
        {
            var archFileName = Path.Combine(_options.DestDir, ExampleCreator.GetZipFileName(example));

#if DEBUG
            //_logger.Info($"archFileName = {archFileName}");
#endif

            File.Delete(archFileName);

            var consoleFileName = Path.Combine(_options.DestDir, ExampleCreator.GetConsoleFileName(example));

#if DEBUG
            //_logger.Info($"consoleFileName = {consoleFileName}");
#endif

            File.Delete(consoleFileName);
        }

        private void ClearDestDir()
        {
            ClearDestDir(null);
        }

        private void ClearDestDir(List<string> except)
        {
            var cleanedFiles = Directory.GetFiles(_options.DestDir).Where(p => p.EndsWith(".zip") || p.EndsWith(".console")).ToList();

#if DEBUG
            //_logger.Info($"cleanedFiles.Count = {cleanedFiles.Count}");
#endif

            if (!except.IsNullOrEmpty())
            {
                cleanedFiles = cleanedFiles.Except(except).ToList();
            }

#if DEBUG
            //_logger.Info($"cleanedFiles.Count (after) = {cleanedFiles.Count}");
#endif

            foreach (var cleanedFile in cleanedFiles)
            {
#if DEBUG
                //_logger.Info($"cleanedFile = {cleanedFile}");
#endif

                File.Delete(cleanedFile);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var nextN = n + DisplayHelper.IndentationStep;
            var nextNSpaces = DisplayHelper.Spaces(nextN);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Builds examples to '{_options.DestDir}'.");
            sb.AppendLine($"{spaces}Source pages:");

            foreach(var lngExamplesPage in _options.LngExamplesPages)
            {
                sb.AppendLine($"{nextNSpaces}{lngExamplesPage}");
            }

            sb.AppendLine($"{spaces}Using CLI on '{_options.SocExePath}'.");
            sb.AppendLine($"{spaces}CacheDir = '{_options.CacheDir}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
