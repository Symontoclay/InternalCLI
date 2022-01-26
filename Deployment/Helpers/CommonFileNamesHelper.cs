using BaseDevPipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class CommonFileNamesHelper
    {
        public static string BuildReleaseNotesPath()
        {
            return BuildReleaseNotesPath(ProjectsDataSource.GetSolution(KindOfProject.ProjectSite).SourcePath);
        }

        public static string BuildReleaseNotesPath(string basePath)
        {
            return Path.Combine(basePath, "ReleaseNotes.json");
        }
    }
}
