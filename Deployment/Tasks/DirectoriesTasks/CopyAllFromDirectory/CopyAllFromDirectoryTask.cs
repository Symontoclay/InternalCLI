using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory
{
    public class CopyAllFromDirectoryTask : BaseDeploymentTask
    {
        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options)
            : this(options, null)
        {
        }

        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.SourceDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly CopyAllFromDirectoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var fileNamesGetterOptions = new FileNamesGetterOptions();

            fileNamesGetterOptions.SourceDir = _options.SourceDir;
            fileNamesGetterOptions.OnlySubDirs = _options.OnlySubDirs;
            fileNamesGetterOptions.ExceptSubDirs = _options.ExceptSubDirs;
            fileNamesGetterOptions.OnlyFileExts = _options.OnlyFileExts;
            fileNamesGetterOptions.ExceptFileExts = _options.ExceptFileExts;
            fileNamesGetterOptions.FileNameShouldContain = _options.FileNameShouldContain;
            fileNamesGetterOptions.FileNameShouldNotContain = _options.FileNameShouldNotContain;

            var fileNamesGetter = new FileNamesGetter(fileNamesGetterOptions);

            var sourceFullFileNamesList = fileNamesGetter.GetFileNames();

            if (!sourceFullFileNamesList.Any())
            {
                return;
            }

            Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
            {
                DestDir = _options.DestDir,
                SaveSubDirs = _options.SaveSubDirs,
                TargetFiles = sourceFullFileNamesList
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + DisplayHelper.IndentationStep;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copies all files form directory '{_options.SourceDir}' to {_options.DestDir}.");
            if (_options.SaveSubDirs)
            {
                sb.AppendLine($"{spaces}Saves subdirectories' structure.");
            }
            else
            {
                sb.AppendLine($"{spaces}All fles will be put to dest directory without saving subdirectories' structure.");
            }
            if(!_options.OnlySubDirs.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Copy when directory contains:");
                foreach (var subdir in _options.OnlySubDirs)
                {
                    sb.AppendLine($"{nextSpaces}{subdir}");
                }
            }
            if(!_options.ExceptSubDirs.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Don't copy when directory contains:");
                foreach (var subdir in _options.ExceptSubDirs)
                {
                    sb.AppendLine($"{nextSpaces}{subdir}");
                }
            }
            if(!_options.OnlyFileExts.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Copy when file name has extension:");
                foreach (var item in _options.OnlyFileExts)
                {
                    sb.AppendLine($"{nextSpaces}{item}");
                }
            }
            if(!_options.ExceptFileExts.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Don't copy when file name has extension:");
                foreach (var item in _options.ExceptFileExts)
                {
                    sb.AppendLine($"{nextSpaces}{item}");
                }
            }
            if(!_options.FileNameShouldContain.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Copy when file name contains:");
                foreach (var item in _options.FileNameShouldContain)
                {
                    sb.AppendLine($"{nextSpaces}{item}");
                }
            }
            if(!_options.FileNameShouldNotContain.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}Don't copy when file name contains:");
                foreach (var item in _options.FileNameShouldNotContain)
                {
                    sb.AppendLine($"{nextSpaces}{item}");
                }
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
