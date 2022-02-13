using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionCopyright;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionVersion;
using Deployment.Tasks.ProjectsTasks.UpdateUnityPackageVersion;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateProjectsVersion
{
    public class UpdateProjectsVersionDevTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateProjectsVersionDevTask()
            : this(0u)
        {
        }

        public UpdateProjectsVersionDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var releaseMngrRepositoryPath = settings.GetSolution(KindOfProject.ReleaseMngrSolution).Path;

            var futureReleaseInfo = FutureReleaseInfoReader.Read(releaseMngrRepositoryPath);

            var version = futureReleaseInfo.Version;

            var copyright = settings.Copyright;

            var targetSolutions = settings.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            foreach (var targetSolution in targetSolutions)
            {
#if DEBUG
                _logger.Info($"targetSolution = {targetSolution}");
#endif

                var kind = targetSolution.Kind;

                switch (kind)
                {
                    case KindOfProject.CoreSolution:
                        Exec(new UpdateSolutionVersionTask(new UpdateSolutionVersionTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Version = version
                        }, NextDeep));

                        Exec(new UpdateSolutionCopyrightTask(new UpdateSolutionCopyrightTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Copyright = copyright
                        }, NextDeep));
                        break;

                    case KindOfProject.Unity:
                        Exec(new UpdateUnityPackageVersionTask(new UpdateUnityPackageVersionTaskOptions()
                        {
                            PackageSourcePath = targetSolution.SourcePath,
                            Version = version,
                            UnityVersion = targetSolution.UnityVersion,
                            UnityRelease = targetSolution.UnityRelease
                        }, NextDeep));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates Version and Copyright for all projects with maintained versions.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
