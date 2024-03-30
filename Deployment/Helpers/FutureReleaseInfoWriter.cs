using BaseDevPipeline;
using BaseDevPipeline.SourceData;
using CommonUtils.DeploymentTasks;
using Deployment.Tasks;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class FutureReleaseInfoWriter
    {
        public static void WriteSource(FutureReleaseInfoSource futureReleaseInfoSource)
        {
            WriteSource(futureReleaseInfoSource, ProjectsDataSourceFactory.GetSolution(KindOfProject.ReleaseMngrSolution).Path);
        }

        public static void WriteSource(FutureReleaseInfoSource futureReleaseInfoSource, string baseRepositoryPath)
        {
            FutureReleaseInfoSource.SaveFile(Path.Combine(baseRepositoryPath, "FutureReleaseInfo.json"), futureReleaseInfoSource);

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
            {
                Message = "Set release as completed",
                RepositoryPaths = new List<string>() { ProjectsDataSourceFactory.GetSolution(KindOfProject.ReleaseMngrSolution).Path }
            }));

            deploymentPipeline.Run();
        }
    }
}
