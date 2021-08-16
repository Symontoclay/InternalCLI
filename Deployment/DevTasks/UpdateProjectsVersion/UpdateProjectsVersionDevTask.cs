using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.VersionTasks.UpdateSolutionCopyright;
using Deployment.Tasks.VersionTasks.UpdateSolutionVersion;
using Deployment.Tasks.VersionTasks.UpdateUnityPackageVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateProjectsVersion
{
    public class UpdateProjectsVersionDevTask : BaseDeploymentTask
    {
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
                var kind = targetSolution.Kind;

                switch (kind)
                {
                    case KindOfProject.CoreSolution:
                        Exec(new UpdateSolutionVersionTask(new UpdateSolutionVersionTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Version = version
                        }));

                        Exec(new UpdateSolutionCopyrightTask(new UpdateSolutionCopyrightTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Copyright = copyright
                        }));
                        break;

                    case KindOfProject.Unity:
                        Exec(new UpdateUnityPackageVersionTask(new UpdateUnityPackageVersionTaskOptions()
                        {
                            PackageSourcePath = targetSolution.SourcePath,
                            Version = version
                        }));
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
