using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects
{
    public class CheckTargetFrameworksInAllCSharpProjectsDevTask : BaseDeploymentTask
    {
        public CheckTargetFrameworksInAllCSharpProjectsDevTask()
            : this(null)
        {
        }

        public CheckTargetFrameworksInAllCSharpProjectsDevTask(IDeploymentTask parentTask)
            : base("F4148046-893B-4136-AE00-43FDE3335ED5", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var projectInformationList = GetInformationAboutProjects();

            var currentFrameworksDict = projectInformationList.GroupBy(p => p.KindOfTargetCSharpFramework).ToDictionary(p => p.Key, p => p.ToList());

            var sb = new StringBuilder();

            var n = 0u;

            var spaces = DisplayHelper.Spaces(n);

            foreach (var currentFrameworksKvpItem in currentFrameworksDict)
            {
#if DEBUG
                //_logger.Info($"currentFrameworksKvpItem.Key = {currentFrameworksKvpItem.Key}");
#endif

                sb.AppendLine($"{spaces}{currentFrameworksKvpItem.Key}:");

                var nextN = n + DisplayHelper.IndentationStep;
                var nextSpaces = DisplayHelper.Spaces(nextN);

                var currentFrameworksItemsDict = currentFrameworksKvpItem.Value.GroupBy(p => p.Version).OrderByDescending(p => p.Key).ToDictionary(p => p.Key, p => p.ToList());

                foreach (var itemsKvp in currentFrameworksItemsDict)
                {
#if DEBUG
                    //_logger.Info($"itemsKvp.Key = {itemsKvp.Key}");
#endif

                    sb.AppendLine($"{nextSpaces}{itemsKvp.Key}:");

                    var nextNextN = nextN + DisplayHelper.IndentationStep;
                    var nextNextSpaces = DisplayHelper.Spaces(nextNextN);

                    var solutionsDict = itemsKvp.Value.GroupBy(p => p.KindOfProject).ToDictionary(p => p.Key, p => p.ToList());

                    foreach (var solutionKvpItem in solutionsDict)
                    {
#if DEBUG
                        //_logger.Info($"solutionKvpItem.Key = {solutionKvpItem.Key}");
#endif

                        sb.AppendLine($"{nextNextSpaces}{solutionKvpItem.Key}:");

                        var nextNextNextN = nextNextN + DisplayHelper.IndentationStep;
                        var nextNextNextSpaces = DisplayHelper.Spaces(nextNextNextN);

                        foreach (var projectItem in solutionKvpItem.Value.Select(p => p.CsProjPath))
                        {
#if DEBUG
                            //_logger.Info($"projectItem = {projectItem}");
#endif

                            sb.AppendLine($"{nextNextNextSpaces}{projectItem}");
                        }
                    }
                }
            }

            _logger.Info(sb);
        }

        private List<(string CsProjPath, KindOfProject KindOfProject, KindOfTargetCSharpFramework KindOfTargetCSharpFramework, Version Version)> GetInformationAboutProjects()
        {
            var projectInformationList = new List<(string CsProjPath, KindOfProject KindOfProject, KindOfTargetCSharpFramework KindOfTargetCSharpFramework, Version Version)>();

            var cSharpSolutions = ProjectsDataSourceFactory.GetCSharpSolutions();

            foreach (var solution in cSharpSolutions)
            {
#if DEBUG
                _logger.Info($"solution.Name = {solution.Name}");
#endif

                foreach (var project in solution.Projects)
                {
#if DEBUG
                    _logger.Info($"project.FolderName = {project.FolderName}");
                    _logger.Info($"project.CsProjPath = {project.CsProjPath}");
#endif

                    var currentFramework = CSharpProjectHelper.GetTargetFrameworkVersion(project.CsProjPath);

#if DEBUG
                    _logger.Info($"currentFramework = {currentFramework}");
#endif

                    projectInformationList.Add((project.CsProjPath, solution.Kind, currentFramework.Kind, currentFramework.Version));
                }
            }

            return projectInformationList;
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Checks all C# projects in organization and prints their target frameworks version to log file.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
