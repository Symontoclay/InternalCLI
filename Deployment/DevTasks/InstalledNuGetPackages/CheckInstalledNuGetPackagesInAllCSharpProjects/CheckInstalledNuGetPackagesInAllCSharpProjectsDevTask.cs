using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deployment.DevTasks.InstalledNuGetPackages.CheckInstalledNuGetPackagesInAllCSharpProjects
{
    public class CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask : BaseDeploymentTask
    {
        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask()
            : this(null, null)
        {
        }

        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask(CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions options)
            : this(options, null)
        {
        }

        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask(IDeploymentTask parentTask)
            : this(new CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions
            {
                ShowOnlyOutdatedPackages = false
            }, parentTask)
        {
        }

        public CheckInstalledNuGetPackagesInAllCSharpProjectsDevTask(CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions options, IDeploymentTask parentTask)
            : base("B9B0E5D6-9CDA-4748-82D8-E10BE36F6651", false, options, parentTask)
        {
            _options = options;
        }

        private readonly CheckInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            _logger.Info($"_options = {_options}");
#endif

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

                var itemSb = new StringBuilder();

                var packageId = packageIdKvpItem.Key; // replace with the required package

                itemSb.AppendLine($"{spaces}{packageId}:");

                var includePrerelease = false;

                var providers = Repository.Provider.GetCoreV3();
                var source = new PackageSource("https://api.nuget.org/v3/index.json");
                var repository = new SourceRepository(source, providers);

                var metadataResource = repository.GetResource<PackageMetadataResource>();
                var packages = metadataResource.GetMetadataAsync(packageId, includePrerelease, false, new SourceCacheContext(), NullLogger.Instance, CancellationToken.None).Result;

                var latest = packages?.MaxBy(p => p.Identity.Version);

                var latestVersion = latest?.Identity.Version;

                //_logger.Info($"The latest version of the package {packageId}: {latestVersion}");

                var nextN = n + DisplayHelper.IndentationStep;
                var nextSpaces = DisplayHelper.Spaces(nextN);

                var packageIdsItemsDict = packageIdKvpItem.Value.GroupBy(p => p.Version).OrderByDescending(p => p.Key).ToDictionary(p => p.Key, p => p.ToList());

                foreach (var itemsKvp in packageIdsItemsDict)
                {
                    var currentVersion = itemsKvp.Key;

#if DEBUG
                    //_logger.Info($"currentVersion = {currentVersion}");
#endif

                    var isOutdated = false;

                    itemSb.Append($"{nextSpaces}{currentVersion}");

                    if(latestVersion == null)
                    {
                        itemSb.Append(" [*] (Internal)");
                    }
                    else
                    {
                        var currentNugetVersion = new NuGetVersion(currentVersion);

                        if (currentNugetVersion == latestVersion)
                        {
                            itemSb.Append(" [+] Up to date");
                        }
                        else
                        {
                            isOutdated = true;

                            itemSb.Append($" [-] (Outdated, latest: {latestVersion})");
                        }
                    }

                    itemSb.AppendLine(":");

                    var nextNextN = nextN + DisplayHelper.IndentationStep;
                    var nextNextSpaces = DisplayHelper.Spaces(nextNextN);

                    var solutionsDict = itemsKvp.Value.GroupBy(p => p.KindOfProject).ToDictionary(p => p.Key, p => p.ToList());

                    foreach (var solutionKvpItem in solutionsDict)
                    {
#if DEBUG
                        //_logger.Info($"solutionKvpItem.Key = {solutionKvpItem.Key}");
#endif

                        itemSb.AppendLine($"{nextNextSpaces}{solutionKvpItem.Key}:");

                        var nextNextNextN = nextNextN + DisplayHelper.IndentationStep;
                        var nextNextNextSpaces = DisplayHelper.Spaces(nextNextNextN);

                        foreach (var projectItem in solutionKvpItem.Value.Select(p => p.CsProjPath))
                        {
#if DEBUG
                            //_logger.Info($"projectItem = {projectItem}");
#endif

                            itemSb.AppendLine($"{nextNextNextSpaces}{projectItem}");
                        }
                    }

                    if (_options.ShowOnlyOutdatedPackages)
                    {
                        if (isOutdated)
                        {
                            sb.Append(itemSb);
                        }
                    }
                    else
                    {
                        sb.Append(itemSb);
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
