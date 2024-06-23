using BaseDevPipeline;
using BaseDevPipeline.SourceData;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks.GitTasks.Pull;
using System;
using System.IO;

namespace Deployment.Helpers
{
    public static class FutureReleaseInfoReader
    {
        public static string GetFutureVersion()
        {
            return Read().Version;
        }

        public static string GetRepositoryName()
        {
            return ProjectsDataSourceFactory.GetSolution(KindOfProject.ReleaseMngrSolution).RepositoryName;
        }

        public static string GetBaseRepositoryPath()
        {
            return ProjectsDataSourceFactory.GetSolution(KindOfProject.ReleaseMngrSolution).Path;
        }

        public static FutureReleaseInfo Read()
        {
            return Read(GetBaseRepositoryPath());
        }

        public static FutureReleaseInfo Read(string baseRepositoryPath)
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PullTask(new PullTaskOptions()
            {
                RepositoryPath = baseRepositoryPath
            }));

            deploymentPipeline.Run();

            var relaseNotes = File.ReadAllText(GetFutureReleaseNotesFullFileName(baseRepositoryPath));

            var futureReleaseInfoSource = FutureReleaseInfoSource.ReadFile(Path.Combine(baseRepositoryPath, "FutureReleaseInfo.json"));

            var result = new FutureReleaseInfo();
            result.Version = futureReleaseInfoSource.Version;
            result.Description = relaseNotes;
            result.Status = Enum.Parse<FutureReleaseStatus>(futureReleaseInfoSource.Status);
            result.StartDate = futureReleaseInfoSource.StartDate;
            result.FinishDate = futureReleaseInfoSource.FinishDate;

            return result;
        }

        public static string GetFutureReleaseNotesFullFileName()
        {
            return GetFutureReleaseNotesFullFileName(GetBaseRepositoryPath());
        }

        public static string GetFutureReleaseNotesFullFileName(string baseRepositoryPath)
        {
            return Path.Combine(baseRepositoryPath, "FutureReleaseNotes.md");
        }

        public static FutureReleaseInfoSource ReadSource()
        {
            return ReadSource(GetBaseRepositoryPath());
        }

        public static FutureReleaseInfoSource ReadSource(string baseRepositoryPath)
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PullTask(new PullTaskOptions()
            {
                RepositoryPath = baseRepositoryPath
            }));

            deploymentPipeline.Run();

            return FutureReleaseInfoSource.ReadFile(Path.Combine(baseRepositoryPath, "FutureReleaseInfo.json"));
        }
    }
}
