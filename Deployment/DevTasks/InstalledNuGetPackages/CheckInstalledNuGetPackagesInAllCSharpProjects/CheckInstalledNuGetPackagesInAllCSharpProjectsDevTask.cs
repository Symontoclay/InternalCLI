using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects
{
    public class CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask : BaseDeploymentTask
    {
        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask()
            : this(null)
        {
        }

        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask(IDeploymentTask parentTask)
            : base("B9B0E5D6-9CDA-4748-82D8-E10BE36F6651", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var packagesInformationList = GetPackagesInformationList();

            var packageIdsDict = packagesInformationList.GroupBy(p => p.PackageId).ToDictionary(p => p.Key, p => p.ToList());

            var sb = new StringBuilder();

            var n = 0u;

            var spaces = DisplayHelper.Spaces(n);

            foreach (var packageIdKvpItem in packageIdsDict)
            {
#if DEBUG
                //_logger.Info($"packageIdKvpItem.Key = {packageIdKvpItem.Key}");
#endif

                sb.AppendLine($"{spaces}{packageIdKvpItem.Key}:");

                var nextN = n + DisplayHelper.IndentationStep;
                var nextSpaces = DisplayHelper.Spaces(nextN);

                var packageIdsItemsDict = packageIdKvpItem.Value.GroupBy(p => p.Version).OrderByDescending(p => p.Key).ToDictionary(p => p.Key, p => p.ToList());

                foreach (var itemsKvp in packageIdsItemsDict)
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

        private List<(string CsProjPath, KindOfProject KindOfProject, string PackageId, Version Version)> GetPackagesInformationList()
        {
            var packagesInformationList = new List<(string CsProjPath, KindOfProject KindOfProject, string PackageId, Version Version)>();

            var cSharpSolutions = ProjectsDataSourceFactory.GetCSharpSolutionsWhichUseNuGetPakages();

            foreach (var solution in cSharpSolutions)
            {
#if DEBUG
                //_logger.Info($"solution.Name = {solution.Name}");
#endif

                foreach (var project in solution.Projects)
                {
#if DEBUG
                    //_logger.Info($"project.FolderName = {project.FolderName}");
                    //_logger.Info($"project.CsProjPath = {project.CsProjPath}");
#endif

                    var installedPackages = CSharpProjectHelper.GetInstalledPackages(project.CsProjPath);

                    foreach (var package in installedPackages)
                    {
#if DEBUG
                        //_logger.Info($"package = {package}");
#endif

                        packagesInformationList.Add((project.CsProjPath, solution.Kind, package.PackageId, package.Version));
                    }
                }
            }

            return packagesInformationList;
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Checks all installed NuGet packages in organization and prints their target frameworks version to log file.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
