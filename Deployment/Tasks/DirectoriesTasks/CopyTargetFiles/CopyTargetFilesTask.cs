using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopyTargetFiles
{
    public class CopyTargetFilesTask : BaseDeploymentTask
    {
        public CopyTargetFilesTask(CopyTargetFilesTaskOptions options)
        {
            _options = options;
            _targetFiles = options?.TargetFiles.Select(p => p.Replace("\\", "/").Trim()).ToList();
        }

        private readonly CopyTargetFilesTaskOptions _options;
        private List<string> _targetFiles;

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

                var fileInfo = new FileInfo(targetFileName);

                fileInfo.Directory.Create();

                File.Copy(fileName, targetFileName, true);
            }
        }

        private void CopyWithoutSaveSubDirs()
        {
            foreach (var fileName in _targetFiles)
            {
                var fileInfo = new FileInfo(fileName);
                var targetFileName = Path.Combine(_options.DestDir, fileInfo.Name);

                File.Copy(fileName, targetFileName, true);
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

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
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
