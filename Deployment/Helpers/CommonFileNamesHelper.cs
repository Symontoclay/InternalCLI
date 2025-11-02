using BaseDevPipeline;
using System.IO;

namespace Deployment.Helpers
{
    public static class CommonFileNamesHelper
    {
        public static string BuildReleaseNotesPath()
        {
            return BuildReleaseNotesPath(ProjectsDataSourceFactory.GetSolution(KindOfProject.ProjectSite).SourcePath);
        }

        public static string BuildReleaseNotesPath(string basePath)
        {
            return Path.Combine(basePath, "ReleaseNotes.json");
        }
    }
}
