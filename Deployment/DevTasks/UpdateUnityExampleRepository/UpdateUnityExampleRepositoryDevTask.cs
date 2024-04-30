using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.UpdateUnityExampleRepository
{
    public class UpdateUnityExampleRepositoryDevTask : BaseDeploymentTask
    {
        public UpdateUnityExampleRepositoryDevTask(UpdateUnityExampleRepositoryDevTaskOptions options)
             : this(options, null)
        {
        }

        public UpdateUnityExampleRepositoryDevTask(UpdateUnityExampleRepositoryDevTaskOptions options, IDeploymentTask parentTask)
             : base(MD5Helper.GetHash(options.SourceRepository), false, options, parentTask)
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
#if DEBUG
            _logger.Info($"_options.SourceRepository = {_options.SourceRepository}");
            _logger.Info($"_options.DestinationRepository = {_options.DestinationRepository}");
#endif

            UpdatePpubishedAssetDirectory();
            UpdateSamplesDirectory();
            UpdateSymOntoClayScriptsDirectory();
        }

        private void UpdatePpubishedAssetDirectory()
        {
            var pubishedAssetDirectory = PathsHelper.Normalize(Path.Combine(_options.SourceRepository, "Assets", "SymOntoClay"));

            var destinationPublishedAssetDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClay"));

#if DEBUG
            _logger.Info($"pubishedAssetDirectory = {pubishedAssetDirectory}");
            _logger.Info($"destinationPublishedAssetDirectory = {destinationPublishedAssetDirectory}");
#endif

            if (Directory.Exists(destinationPublishedAssetDirectory))
            {
                Directory.Delete(destinationPublishedAssetDirectory, true);
            }

            EnumerateFilesForCopying(pubishedAssetDirectory, pubishedAssetDirectory, destinationPublishedAssetDirectory);
        }

        private void UpdateSamplesDirectory()
        {
            var oldSsamplesDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "ExamplesOfSymOntoClay"));

#if DEBUG
            _logger.Info($"oldSsamplesDirectory = {oldSsamplesDirectory}");
#endif

            if (Directory.Exists(oldSsamplesDirectory))
            {
                Directory.Delete(oldSsamplesDirectory, true);
            }

            var samplesDirectory = PathsHelper.Normalize(Path.Combine(_options.SourceRepository, "Assets", "SymOntoClaySamles"));

            var destinationSamplesDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClaySamles"));

            if(Directory.Exists(destinationSamplesDirectory))
            {
                Directory.Delete(destinationSamplesDirectory, true);
            }

#if DEBUG
            _logger.Info($"samplesDirectory = {samplesDirectory}");
            _logger.Info($"destinationSamplesDirectory = {destinationSamplesDirectory}");
#endif

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

        private void UpdateSymOntoClayScriptsDirectory()
        {
            var oldSymOntoClayScriptsDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClayDSLCode"));

#if DEBUG
            _logger.Info($"oldSymOntoClayScriptsDirectory = {oldSymOntoClayScriptsDirectory}");
#endif

            if (!Directory.Exists(oldSymOntoClayScriptsDirectory))
            {
#if DEBUG
                _logger.Info("!Directory.Exists(oldSymOntoClayScriptsDirectory) return;");
#endif

                return;
            }

            var destinationSymOntoClayScriptsDirectory = PathsHelper.Normalize(Path.Combine(_options.DestinationRepository, "Assets", "SymOntoClayScripts"));

#if DEBUG
            _logger.Info($"destinationSymOntoClayScriptsDirectory = {destinationSymOntoClayScriptsDirectory}");
#endif

            if (Directory.Exists(destinationSymOntoClayScriptsDirectory))
            {
                Directory.Delete(destinationSymOntoClayScriptsDirectory, true);
            }

            Directory.CreateDirectory(destinationSymOntoClayScriptsDirectory);

            EnumerateFilesForCopying(oldSymOntoClayScriptsDirectory, oldSymOntoClayScriptsDirectory, destinationSymOntoClayScriptsDirectory);

            Directory.Delete(oldSymOntoClayScriptsDirectory, true);

            var oldSymOntoClayScriptsDirectoryMetaFileName = $"{oldSymOntoClayScriptsDirectory}.meta";

#if DEBUG
            _logger.Info($"oldSymOntoClayScriptsDirectoryMetaFileName = {oldSymOntoClayScriptsDirectoryMetaFileName}");
#endif

            if (File.Exists(oldSymOntoClayScriptsDirectoryMetaFileName))
            {
                File.Delete(oldSymOntoClayScriptsDirectoryMetaFileName);
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
