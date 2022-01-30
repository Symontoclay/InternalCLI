using CommonUtils;
using CommonUtils.DebugHelpers;
using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateUnityExampleRepository
{
    public class UpdateUnityExampleRepositoryDevTask : BaseDeploymentTask
    {
        public UpdateUnityExampleRepositoryDevTask(UpdateUnityExampleRepositoryDevTaskOptions options)
             : this(options, 0u)
        {
        }

        public UpdateUnityExampleRepositoryDevTask(UpdateUnityExampleRepositoryDevTaskOptions options, uint deep)
             : base(options, deep)
        {
            _options = options;
        }

        private readonly UpdateUnityExampleRepositoryDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceRepository), _options.SourceRepository);
            ValidateDirectory(nameof(_options.DestinationRepository), _options.DestinationRepository);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            UpdatePpubishedAssetDirectory();
            UpdateSamplesDirectory();
        }

        private void UpdatePpubishedAssetDirectory()
        {
            var pubishedAssetDirectory = PathsHelper.Normalize(Path.Combine(_options.SourceRepository, "Assets", "SymOntoClay"));

            var destinationPublishedAssetDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClay"));

            if(Directory.Exists(destinationPublishedAssetDirectory))
            {
                Directory.Delete(destinationPublishedAssetDirectory, true);
            }

            EnumerateFilesForCopying(pubishedAssetDirectory, pubishedAssetDirectory, destinationPublishedAssetDirectory);
        }

        private void UpdateSamplesDirectory()
        {
            var oldSsamplesDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "ExamplesOfSymOntoClay"));

            if (Directory.Exists(oldSsamplesDirectory))
            {
                Directory.Delete(oldSsamplesDirectory, true);
            }

            var samplesDirectory = PathsHelper.Normalize(Path.Combine(_options.SourceRepository, "Assets", "SymOntoClaySamles"));

            var destinationSamplesDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClaySamles"));

            EnumerateFilesForCopying(samplesDirectory, samplesDirectory, destinationSamplesDirectory);
        }

        private void EnumerateFilesForCopying(string dir, string baseSourceDir, string baseDestDir)
        {
            var newDir = PathsHelper.Normalize(dir).Replace(baseSourceDir, baseDestDir);

            if(!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }

            var subDirs = Directory.GetDirectories(dir);

            foreach (var subDir in subDirs)
            {
                EnumerateFilesForCopying(subDir, baseSourceDir, baseDestDir);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                if(file.EndsWith(".meta"))
                {
                    continue;
                }

                var newFileName = PathsHelper.Normalize(file).Replace(baseSourceDir, baseDestDir);

                File.Copy(file, newFileName, true);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates unity example repository '{_options.DestinationRepository}' by '{_options.SourceRepository}'");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
