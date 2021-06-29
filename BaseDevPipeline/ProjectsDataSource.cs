using BaseDevPipeline.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class ProjectsDataSource
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static ProjectsDataSource()
        {
            var settingsSource = new SymOntoClaySettingsSource()
            {
                BasePaths = new List<string>()
                {
                    "%USERPROFILE%/Documents/GitHub",
                    "%USERPROFILE%/source/repos"
                }
            };

            settingsSource.Solutions = new List<SolutionSource>();
            settingsSource.Artifacts = new List<ArtifactDest>();


            var siteSolution = new SolutionSource()
            {
                Kind = KindOfProjectSource.ProjectSite.ToString(),
                Paths = new List<string>()
                {
                    "%USERPROFILE%/Documents/GitHub/symontoclay.github.io",
                    "%USERPROFILE%/source/repos/symontoclay.github.io"
                },
                SourcePath = "%SITE_ROOT_PATH%/siteSource/",
                DestPath = "%SITE_ROOT_PATH%/"
            };

            settingsSource.Solutions.Add(siteSolution);

            _logger.Info($"siteSolution = {siteSolution}");

            var siteArtifact = new ArtifactDest()
            {
                Kind = KindOfArtifact.ProjectSite.ToString(),
            };

            _logger.Info($"siteArtifact = {siteArtifact}");

            settingsSource.Artifacts.Add(siteArtifact);

            _logger.Info($"settingsSource = {settingsSource}");
        }

        public static int A { get; set; }
    }
}
