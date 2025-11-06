using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.DirectoriesTasks.CopyTargetFiles
{
    public class CopyTargetFilesTask : BaseDeploymentTask
    {
        public CopyTargetFilesTask(CopyTargetFilesTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.DestDir), false, options, parentTask)
        {
            _options = options;
            _targetFiles = options?.TargetFiles.Select(p => p.Replace("\\", "/").Trim()).ToList();
        }

        private readonly CopyTargetFilesTaskOptions _options;
        private List<string> _targetFiles;
        private ExistingFileStrategy _existingFileStrategy;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
            ValidateList(nameof(_options.TargetFiles), _options.TargetFiles);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _existingFileStrategy = _options.ExistingFileStrategy;

            if (_options.SaveSubDirs)
            {
                CopyWithSaveSubDirs();
            }
            else
            {
                CopyWithoutSaveSubDirs();
            }
        }

        private void CopyWithSaveSubDirs()
        {
            var baseSourceDir = _options.BaseSourceDir;

            if (string.IsNullOrWhiteSpace(baseSourceDir))
            {
                baseSourceDir = CalculateBaseSourceDir();
            }

            foreach (var fileName in _targetFiles)
            {
                var targetFileName = fileName.Replace(baseSourceDir, _options.DestDir);

#if DEBUG
                //_logger.Info($"fileName = {fileName}; stargetFileName = {targetFileName}");
#endif

                var fileInfo = new FileInfo(targetFileName);

                if(fileInfo.Exists)
                {
                    switch(_existingFileStrategy)
                    {
                        case ExistingFileStrategy.Unknown:
                        case ExistingFileStrategy.Exception:
                            throw new Exception($"The file {targetFileName} already exists.");

                        case ExistingFileStrategy.Skip:
                            break;

                        case ExistingFileStrategy.Overwrite:
                            File.Copy(fileName, targetFileName, true);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(_existingFileStrategy), _existingFileStrategy, null);
                    }
                }
                else
                {
                    fileInfo.Directory.Create();

                    File.Copy(fileName, targetFileName);
                }
            }
        }

        private void CopyWithoutSaveSubDirs()
        {
            foreach (var fileName in _targetFiles)
            {
                var fileInfo = new FileInfo(fileName);
                var targetFileName = Path.Combine(_options.DestDir, fileInfo.Name);

#if DEBUG
                //_logger.Info($"fileName = {fileName}; stargetFileName = {targetFileName}");
#endif

                if (File.Exists(targetFileName))
                {
                    switch (_existingFileStrategy)
                    {
                        case ExistingFileStrategy.Unknown:
                        case ExistingFileStrategy.Exception:
                            throw new Exception($"The file {targetFileName} already exists.");

                        case ExistingFileStrategy.Skip:
                            break;

                        case ExistingFileStrategy.Overwrite:
                            File.Copy(fileName, targetFileName, true);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(_existingFileStrategy), _existingFileStrategy, null);
                    }
                }
                else
                {
                    File.Copy(fileName, targetFileName);
                }                
            }
        }

        private string CalculateBaseSourceDir()
        {
            var firstItem = _targetFiles.First();

            var directoryInfo = new FileInfo(firstItem).Directory;

            var rootName = directoryInfo.Root.FullName.Replace("\\", "/");

            if (!_targetFiles.All(p => p.StartsWith(rootName)))
            {
                return string.Empty;
            }

            var result = string.Empty;

            var targetFilesWithoutRoot = _targetFiles.Select(p => p.Replace(rootName, string.Empty).Trim()).ToList();

            firstItem = targetFilesWithoutRoot.First();

            var pos = 0;

            while (true)
            {
                pos = firstItem.IndexOf("/", pos + 1);

                if(pos == -1)
                {
                    return Path.Combine(rootName, result);
                }

                var fragment = firstItem.Substring(0, pos).Trim();

                if(!targetFilesWithoutRoot.All(p => p.StartsWith(fragment)))
                {
                    return Path.Combine(rootName, result);
                }

                result = fragment;
            }

            throw new NotImplementedException("3EAD9684-DA90-4E16-B717-1BD9B4B760E3");
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + DisplayHelper.IndentationStep;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copies target files to directory {_options.DestDir}.");
            if (_options.SaveSubDirs)
            {
                sb.AppendLine($"{spaces}Saves subdirectories' structure.");
            }
            else
            {
                sb.AppendLine($"{spaces}All fles will be put to dest directory without saving subdirectories' structure.");
            }

            sb.AppendLine($"{spaces}ExistingFileStrategy: {_options.ExistingFileStrategy}");

            if (!_options.TargetFiles.IsNullOrEmpty())
            {
                sb.AppendLine($"{spaces}The target copied files:");
                foreach (var targetFile in _options.TargetFiles)
                {
                    sb.AppendLine($"{nextSpaces}{targetFile}");
                }
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
