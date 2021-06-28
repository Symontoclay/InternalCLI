using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
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
    public class CopyAllFromDirectoryTask : BaseDeploymentTask
    {
        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options)
        {
            _options = options;
        }

        private readonly CopyAllFromDirectoryTaskOptions _options;
        private string _sourceDir;
        private List<string> _onlySubDirs;
        private List<string> _exceptSubDirs;
        private List<string> _onlyFileExts;
        private List<string> _exceptFileExts;
        private List<string> _fileNameShouldContain;
        private List<string> _fileNameShouldNotContain;

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
            _sourceDir = _options.SourceDir.Replace("\\", "/");

            _onlySubDirs = _options.OnlySubDirs?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();
            _exceptSubDirs = _options.ExceptSubDirs?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();
            _onlyFileExts = _options.OnlyFileExts?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();
            _exceptFileExts = _options.ExceptFileExts?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();
            _fileNameShouldContain = _options.FileNameShouldContain?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();
            _fileNameShouldNotContain = _options.FileNameShouldNotContain?.Select(p => p.Replace("\\", "/").ToLower().Trim()).ToList();

            var sourceFullFileNamesList = new List<string>();

            EnumerateFileNames(_sourceDir, sourceFullFileNamesList);
            
            if(!sourceFullFileNamesList.Any())
            {
                return;
            }

            Exec(new CopyTargetFilesTask(new CopyTargetFilesTaskOptions()
            {
                DestDir = _options.DestDir,
                SaveSubDirs = _options.SaveSubDirs,
                TargetFiles = sourceFullFileNamesList
            }));
        }

        private void EnumerateFileNames(string directoryName, List<string> sourceFullFileNamesList)
        {
            foreach (var subDirectoryName in Directory.GetDirectories(directoryName))
            {
                if(!_onlySubDirs.IsNullOrEmpty() || !_exceptSubDirs.IsNullOrEmpty())
                {
                    var pureDir = subDirectoryName.Replace("\\", "/").Replace(_sourceDir, string.Empty).ToLower();

                    if (!_onlySubDirs.IsNullOrEmpty())
                    {
                        if(!_onlySubDirs.Any(p => pureDir.Contains(p)))
                        {
                            continue;
                        }
                    }
                    if (!_exceptSubDirs.IsNullOrEmpty())
                    {
                        if (_exceptSubDirs.Any(p => pureDir.Contains(p)))
                        {
                            continue;
                        }
                    }
                }

                EnumerateFileNames(subDirectoryName, sourceFullFileNamesList);
            }

            foreach(var fileName in Directory.GetFiles(directoryName))
            {
                if(!_onlyFileExts.IsNullOrEmpty() || !_exceptFileExts.IsNullOrEmpty() || !_fileNameShouldContain.IsNullOrEmpty() || !_fileNameShouldNotContain.IsNullOrEmpty())
                {
                    var fileInfo = new FileInfo(fileName);

                    var fileInfoName = fileInfo.Name;

                    var pureFileName = Path.Combine(fileInfo.DirectoryName, fileInfoName.Substring(0, fileInfoName.Length - fileInfo.Extension.Length)).Replace("\\", "/").Replace(_sourceDir, string.Empty).ToLower();

                    var ext = fileInfo.Extension.Substring(1);

                    if (!_onlyFileExts.IsNullOrEmpty())
                    {
                        if (!_onlyFileExts.Any(p => p == ext))
                        {
                            continue;
                        }
                    }
                    if (!_exceptFileExts.IsNullOrEmpty())
                    {
                        if (_exceptFileExts.Any(p => p == ext))
                        {
                            continue;
                        }
                    }

                    if (!_fileNameShouldContain.IsNullOrEmpty())
                    {
                        if(!_fileNameShouldContain.Any(p => pureFileName.Contains(p)))
                        {
                            continue;
                        }
                    }

                    if (!_fileNameShouldNotContain.IsNullOrEmpty())
                    {
                        if(_fileNameShouldNotContain.Any(p => pureFileName.Contains(p)))
                        {
                            continue;
                        }
                    }
                }

                sourceFullFileNamesList.Add(fileName.Replace("\\", "/"));
            }
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
