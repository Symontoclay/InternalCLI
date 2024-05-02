using SymOntoClay.Common.CollectionsHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Deployment.Helpers
{
    public class FileNamesGetter
    {
        public FileNamesGetter(FileNamesGetterOptions options)
        {
            _options = options;
        }

        private readonly FileNamesGetterOptions _options;
        private string _sourceDir;
        private List<string> _onlySubDirs;
        private List<string> _exceptSubDirs;
        private List<string> _onlyFileExts;
        private List<string> _exceptFileExts;
        private List<string> _fileNameShouldContain;
        private List<string> _fileNameShouldNotContain;

        public List<string> GetFileNames()
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

            return sourceFullFileNamesList;
        }

        private void EnumerateFileNames(string directoryName, List<string> sourceFullFileNamesList)
        {
            foreach (var subDirectoryName in Directory.GetDirectories(directoryName))
            {
                if (!_onlySubDirs.IsNullOrEmpty() || !_exceptSubDirs.IsNullOrEmpty())
                {
                    var pureDir = subDirectoryName.Replace("\\", "/").Replace(_sourceDir, string.Empty).ToLower();

                    if (!_onlySubDirs.IsNullOrEmpty())
                    {
                        if (!_onlySubDirs.Any(p => pureDir.Contains(p)))
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

            foreach (var fileName in Directory.GetFiles(directoryName))
            {
                if (!_onlyFileExts.IsNullOrEmpty() || !_exceptFileExts.IsNullOrEmpty() || !_fileNameShouldContain.IsNullOrEmpty() || !_fileNameShouldNotContain.IsNullOrEmpty())
                {
                    var fileInfo = new FileInfo(fileName);

                    var fileInfoName = fileInfo.Name;

                    var pureFileName = Path.Combine(fileInfo.DirectoryName, fileInfoName.Substring(0, fileInfoName.Length - fileInfo.Extension.Length)).Replace("\\", "/").Replace(_sourceDir, string.Empty).ToLower();

                    var ext = string.IsNullOrWhiteSpace(fileInfo.Extension) ? string.Empty : fileInfo.Extension.Substring(1);

                    if (!_onlyFileExts.IsNullOrEmpty())
                    {
                        if(string.IsNullOrWhiteSpace(ext))
                        {
                            continue;
                        }

                        if (!_onlyFileExts.Any(p => p == ext))
                        {
                            continue;
                        }
                    }
                    if (!_exceptFileExts.IsNullOrEmpty())
                    {
                        if (!string.IsNullOrWhiteSpace(ext) && _exceptFileExts.Any(p => p == ext))
                        {
                            continue;
                        }
                    }

                    if (!_fileNameShouldContain.IsNullOrEmpty())
                    {
                        if (!_fileNameShouldContain.Any(p => pureFileName.Contains(p)))
                        {
                            continue;
                        }
                    }

                    if (!_fileNameShouldNotContain.IsNullOrEmpty())
                    {
                        if (_fileNameShouldNotContain.Any(p => pureFileName.Contains(p)))
                        {
                            continue;
                        }
                    }
                }

                sourceFullFileNamesList.Add(fileName.Replace("\\", "/"));
            }
        }
    }
}
