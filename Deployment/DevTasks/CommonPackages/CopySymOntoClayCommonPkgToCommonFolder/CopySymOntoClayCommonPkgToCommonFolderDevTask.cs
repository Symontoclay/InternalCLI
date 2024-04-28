using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.CommonPackages.CopySymOntoClayCommonPkgToCommonFolder
{
    public class CopySymOntoClayCommonPkgToCommonFolderDevTask : BaseDeploymentTask
    {
        public CopySymOntoClayCommonPkgToCommonFolderDevTask()
            : this(null)
        {
        }

        public CopySymOntoClayCommonPkgToCommonFolderDevTask(IDeploymentTask parentTask)
            : base("8639689E-6B17-404C-B291-8D8998475F8A", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var commonPackageSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.CommonPackagesSolution);

#if DEBUG
            //_logger.Info($"commonPackageSolution = {commonPackageSolution}");
#endif

            var destPackagesPath = Path.Combine(commonPackageSolution.Path, commonPackageSolution.BuiltNuGetPackages);

#if DEBUG
            //_logger.Info($"destPackagesPath = {destPackagesPath}");
#endif

            Directory.CreateDirectory(destPackagesPath);

            foreach (var project in commonPackageSolution.Projects)
            {
#if DEBUG
                //_logger.Info($"project = {project}");
#endif

                var outputPath = Path.Combine(project.Path, "bin", "Debug");

#if DEBUG
                //_logger.Info($"outputPath = {outputPath}");
#endif

                var version = CSharpProjectHelper.GetVersion(project.CsProjPath);

                var packageName = $"{project.FolderName}.{version}.nupkg";

#if DEBUG
                //_logger.Info($"packageName = {packageName}");
#endif

                var sourcePackageFullName = Path.Combine(outputPath, packageName);

#if DEBUG
                //_logger.Info($"sourcePackageFullName = {sourcePackageFullName}");
#endif

                var destPackageFullName = Path.Combine(destPackagesPath, packageName);

#if DEBUG
                //_logger.Info($"destPackageFullName = {destPackageFullName}");
#endif

                File.Copy(sourcePackageFullName, destPackageFullName, true);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copies built SymOntoClay.Common NuGetPackages to common folder.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
