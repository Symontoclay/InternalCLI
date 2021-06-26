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
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public CopyAllFromDirectoryTask(CopyAllFromDirectoryTaskOptions options)
        {
            _options = options;
        }

        private readonly CopyAllFromDirectoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory("SourceDir", _options.SourceDir);
            ValidateDirectory("DestDir", _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Begin");

            var sourceFullFileNamesList = new List<string>();

            EnumerateFileNames(_options.SourceDir, sourceFullFileNamesList);

            _logger.Info($"sourceFullFileNamesList = {JsonConvert.SerializeObject(sourceFullFileNamesList, Formatting.Indented)}");
            
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

            _logger.Info("End");
        }

        private void EnumerateFileNames(string directoryName, List<string> sourceFullFileNamesList)
        {
            _logger.Info($"directoryName = {directoryName}");

            foreach (var subDirectoryName in Directory.GetDirectories(directoryName))
            {
                if (!_options.OnlySubDirs.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }
                if (!_options.ExceptSubDirs.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }

                EnumerateFileNames(subDirectoryName, sourceFullFileNamesList);
            }

            foreach(var fileName in Directory.GetFiles(directoryName))
            {
                _logger.Info($"fileName = {fileName}");

                if (!_options.OnlyFileExts.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }
                if (!_options.ExceptFileExts.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }
                if (!_options.FileNameShouldContain.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }
                if (!_options.FileNameShouldNotContain.IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }

                sourceFullFileNamesList.Add(fileName);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.Append($"{spaces}Copies all files form directory '{_options.SourceDir}' to {_options.DestDir}.");
            if (_options.SaveSubDirs)
            {
                sb.Append(" Saves subdirectories' structure.");
            }
            else
            {
                sb.Append(" All fles will be put to dest directory without saving subdirectories' structure.");
            }
            if(!_options.OnlySubDirs.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            if(!_options.ExceptSubDirs.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            if(!_options.OnlyFileExts.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            if(!_options.ExceptFileExts.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            if(!_options.FileNameShouldContain.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            if(!_options.FileNameShouldNotContain.IsNullOrEmpty())
            {
                throw new NotImplementedException();
            }
            sb.AppendLine();
            sb.Append(PrintValidation(n));
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
