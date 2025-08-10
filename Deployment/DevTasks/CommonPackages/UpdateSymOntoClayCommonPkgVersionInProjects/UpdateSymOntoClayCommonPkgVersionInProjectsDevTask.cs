using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.CommonPackages.UpdateSymOntoClayCommonPkgVersionInProjects
{
    public class UpdateSymOntoClayCommonPkgVersionInProjectsDevTask : BaseDeploymentTask
    {
        public UpdateSymOntoClayCommonPkgVersionInProjectsDevTask()
            : this(null)
        {
        }

        public UpdateSymOntoClayCommonPkgVersionInProjectsDevTask(IDeploymentTask parentTask)
            : base("93CB1EDF-5973-4B11-9323-DE4B06DFCD5C", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var commonPackagesSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.CommonPackagesSolution);

            var pkgVersion = CSharpProjectHelper.GetMaxVersionOfSolution(commonPackagesSolution.Path);

#if DEBUG
            //_logger.Info($"pkgVersion = {pkgVersion}");
#endif

            var targetVersion = new Version(pkgVersion);

#if DEBUG
            //_logger.Info($"targetVersion = {targetVersion}");
#endif

            var packagesList = commonPackagesSolution.Projects.Where(p => p.Kind == KindOfProject.Library).Select(p => p.FolderName).ToList();

#if DEBUG
            //_logger.Info($"packagesList = {JsonConvert.SerializeObject(packagesList, Formatting.Indented)}");
#endif

            if (packagesList.Count == 0)
            {
                return;
            }

            var solutionsList = ProjectsDataSourceFactory.GetSolutionsWhichUseCommonPakage();

            foreach (var solution in solutionsList)
            {
#if DEBUG
                //_logger.Info($"solution.Name = {solution.Name}");
#endif

                foreach (var project in solution.Projects)
                {
#if DEBUG
                    //_logger.Info($"project.Path = {project.Path}");
#endif

                    foreach (var package in packagesList)
                    {
#if DEBUG
                        //_logger.Info($"package = {package}");
#endif

                        var versionStr = CSharpProjectHelper.GetInstalledPackageVersion(project.CsProjPath, package);

#if DEBUG
                        //_logger.Info($"versionStr = {versionStr}");
#endif

                        if (string.IsNullOrWhiteSpace(versionStr))
                        {
                            continue;
                        }

                        var existingVersion = new Version(versionStr);

#if DEBUG
                        //_logger.Info($"existingVersion = {existingVersion}");
#endif

                        if (targetVersion > existingVersion)
                        {
                            CSharpProjectHelper.UpdateInstalledPackageVersion(project.CsProjPath, package, targetVersion.ToString());
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates all project which depends on SymOntoClay.Common NuGetPackages.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
