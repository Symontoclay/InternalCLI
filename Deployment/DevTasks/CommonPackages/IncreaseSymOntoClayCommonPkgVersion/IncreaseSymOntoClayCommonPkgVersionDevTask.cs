using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using CSharpUtils;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.DevTasks.CommonPackages.IncreaseSymOntoClayCommonPkgVersion
{
    public class IncreaseSymOntoClayCommonPkgVersionDevTask : BaseDeploymentTask
    {
        public IncreaseSymOntoClayCommonPkgVersionDevTask()
            : this(null)
        {
        }

        public IncreaseSymOntoClayCommonPkgVersionDevTask(IDeploymentTask parentTask)
            : base("D80F9215-4CFA-4C63-9C62-C622B33ACE4D", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var commonPackagesSolution = ProjectsDataSourceFactory.GetSolution(KindOfProject.CommonPackagesSolution);

            var versionStr = CSharpProjectHelper.GetMaxVersionOfSolution(commonPackagesSolution.Path);

#if DEBUG
            //_logger.Info($"versionStr = {versionStr}");
#endif

            var version = new Version(versionStr);

#if DEBUG
            //_logger.Info($"version = {version}");
#endif

            var newVersion = new Version(version.Major, version.Minor, version.Build, version.Revision == -1 ? 1 : version.Revision + 1);

#if DEBUG
            //_logger.Info($"newVersion = {newVersion}");
#endif

            CSharpProjectHelper.SetVersionToSolution(commonPackagesSolution.Path, newVersion.ToString());
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Increases version of SymOntoClay.Common.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
