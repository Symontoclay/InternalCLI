using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionCopyright;
using Deployment.Tasks.ProjectsTasks.UpdateSolutionVersion;
using Deployment.Tasks.ProjectsTasks.UpdateUnityPackageVersion;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.DevTasks.UpdateProjectsVersion
{
    public class UpdateProjectsVersionDevTask : BaseDeploymentTask
    {
        public UpdateProjectsVersionDevTask()
            : this(null)
        {
        }

        public UpdateProjectsVersionDevTask(IDeploymentTask parentTask)
            : base("3351A477-F764-4A89-8BCC-82CC91861ED7", false, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var releaseMngrRepositoryPath = settings.GetSolution(KindOfProject.ReleaseMngrSolution).Path;

            var futureReleaseInfo = FutureReleaseInfoReader.Read(releaseMngrRepositoryPath);

            var version = futureReleaseInfo.Version;

            var copyright = settings.Copyright;

            var targetSolutions = settings.GetSolutionsWithMaintainedVersionsInCSharpProjects();

            foreach (var targetSolution in targetSolutions)
            {
#if DEBUG
                //_logger.Info($"targetSolution = {targetSolution}");
#endif

                var kind = targetSolution.Kind;

                switch (kind)
                {
                    case KindOfProject.CoreSolution:
                    case KindOfProject.CommonPackagesSolution:
                        Exec(new UpdateSolutionVersionTask(new UpdateSolutionVersionTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Version = version
                        }, this));

                        Exec(new UpdateSolutionCopyrightTask(new UpdateSolutionCopyrightTaskOptions()
                        {
                            SolutionFilePath = targetSolution.SlnPath,
                            Copyright = copyright
                        }, this));
                        break;

                    case KindOfProject.Unity:
                        Exec(new UpdateUnityPackageVersionTask(new UpdateUnityPackageVersionTaskOptions()
                        {
                            PackageSourcePath = targetSolution.SourcePath,
                            Version = version,
                            UnityVersion = targetSolution.UnityVersion,
                            UnityRelease = targetSolution.UnityRelease
                        }, this));
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
