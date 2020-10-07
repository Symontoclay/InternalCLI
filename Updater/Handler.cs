using CommonUtils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        }

        private readonly string _solutionPath;
        private readonly string _targetVersion;

        public void Run()
        {
#if DEBUG
            _logger.Info("Begin");
            _logger.Info($"_solutionPath = {_solutionPath}");
            _logger.Info($"_targetVersion = {_targetVersion}");
#endif

            ChangeProjects();

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

                _logger.Info($"projectName = {projectName}; updated = {updated}");
            }

#if DEBUG
            _logger.Info("End");
#endif
        }
    }
}
