using CommonUtils;
using Deployment.Building;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSandBox
{
    public class BuildHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            var solutionDir = "%USERPROFILE%/source/repos/SymOntoClay";

            _logger.Info($"solutionDir = {solutionDir}");

            solutionDir = EVPath.Normalize(solutionDir);

            _logger.Info($"solutionDir (2) = {solutionDir}");

            var options = new BuildOptions();

            var solutionOption = new BuildSourceSolutionOptions();
            solutionOption.SolutionDir = solutionDir;

            options.SolutionOptions.Add(solutionOption);

            var projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCoreHelper";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCore";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayUnityAssetCore";
            projectOption.Kind = KindOfSourceProject.Library;

            projectOption = new BuildSourceProjectOptions();
            solutionOption.ProjectsOptions.Add(projectOption);
            projectOption.ProjectDir = "SymOntoClayCLI";
            projectOption.Kind = KindOfSourceProject.CLI;

            _logger.Info($"options = {options}");

            //_logger.Info($" = {}");

            _logger.Info("End");
        }
    }
}
