using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks.DirectoriesTasks.CopyTargetFiles;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopyAllFromDirectory
{
    public class CopyAllFromDirectoryTask : OldBaseDeploymentTask
    {
        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options)
            : this(options, 0u)
        {
        }

        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options, uint deep)
            : base(options, deep)
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
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
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
