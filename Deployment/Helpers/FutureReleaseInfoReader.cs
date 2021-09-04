using BaseDevPipeline;
using BaseDevPipeline.SourceData;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.Pull;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class FutureReleaseInfoReader
    {
        public static FutureReleaseInfo Read()
        {
            return Read(ProjectsDataSource.GetSolution(KindOfProject.ReleaseMngrSolution).Path);
        }

        public static FutureReleaseInfo Read(string baseRepositoryPath)
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PullTask(new PullTaskOptions()
            {
                RepositoryPath = baseRepositoryPath
            }));

            deploymentPipeline.Run();

            var relaseNotes = File.ReadAllText(Path.Combine(baseRepositoryPath, "FutureReleaseNotes.md"));

            var futureReleaseInfoSource = FutureReleaseInfoSource.ReadFile(Path.Combine(baseRepositoryPath, "FutureReleaseInfo.json"));

            var result = new FutureReleaseInfo();
            result.Version = futureReleaseInfoSource.Version;
            result.Description = relaseNotes;
            result.Status = Enum.Parse<FutureReleaseStatus>(futureReleaseInfoSource.Status);
            result.StartDate = futureReleaseInfoSource.StartDate;
            result.FinishDate = futureReleaseInfoSource.FinishDate;

            return result;
        }

        public static FutureReleaseInfoSource ReadSource()
        {
            return ReadSource(ProjectsDataSource.GetSolution(KindOfProject.ReleaseMngrSolution).Path);
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
