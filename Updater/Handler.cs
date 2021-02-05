using CommonUtils;
using CSharpUtils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Updater
{
    public class Handler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Handler()
        {
            _solutionPath = EVPath.Normalize(ConfigurationManager.AppSettings["solutionPath"]);
            _targetVersion = ConfigurationManager.AppSettings["targetVersion"];
            _copyright = ConfigurationManager.AppSettings["copyright"];
            _smallLicenceFile = EVPath.Normalize(ConfigurationManager.AppSettings["smallLicenceFile"]);
        }

        private readonly string _solutionPath;
        private readonly string _targetVersion;
        private readonly string _copyright;
        private readonly string _smallLicenceFile;

        public void Run()
        {
#if DEBUG
            _logger.Info("Begin");
            _logger.Info($"_solutionPath = {_solutionPath}");
            _logger.Info($"_targetVersion = {_targetVersion}");
            _logger.Info($"_copyright = {_copyright}");
            _logger.Info($"_smallLicenceFile = {_smallLicenceFile}");
#endif

            //ChangeProjects();
            AddCopyrightHeaderToCSharpFiles();

#if DEBUG
            _logger.Info("End");
#endif
        }

        private void ChangeProjects()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            var projectNamesList = SolutionHelper.GetProjectsNames(_solutionPath);

            foreach (var projectName in projectNamesList)
            {
                var updated = CSharpProjectHelper.SetVersion(projectName, _targetVersion);

                updated = updated || CSharpProjectHelper.SetCopyright(projectName, _copyright);

                _logger.Info($"projectName = {projectName}; updated = {updated}");
            }

#if DEBUG
            _logger.Info("End");
#endif
        }

        private void AddCopyrightHeaderToCSharpFiles()
        {
#if DEBUG
            _logger.Info("Begin");
#endif

            var licenceText = File.ReadAllText(_smallLicenceFile);

#if DEBUG
            _logger.Info($"licenceText = '{licenceText}'");
#endif

            if(!licenceText.StartsWith("/*"))
            {
                licenceText = $"/*{licenceText}*/";
            }

#if DEBUG
            _logger.Info($"licenceText (after) = '{licenceText}'");
#endif

            var fileNamesList = SolutionHelper.GetSCharpFileNames(_solutionPath);

#if DEBUG
            _logger.Info($"fileNamesList.Count = {fileNamesList.Count}");
#endif

            foreach (var fileName in fileNamesList)
            {
                var updated = CSharpFileHelper.AddCopyrightHeader(fileName, licenceText);

#if DEBUG
                _logger.Info($"fileName = {fileName}; updated = {updated}");
#endif
            }

#if DEBUG
            _logger.Info("End");
#endif
        }
    }
}
