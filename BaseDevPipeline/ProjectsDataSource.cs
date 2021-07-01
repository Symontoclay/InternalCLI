using BaseDevPipeline.Data;
using BaseDevPipeline.Data.Implementation;
using BaseDevPipeline.SourceData;
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
                },
                UnityPaths = new List<string>()
                {
                    "%ProgramFiles%/Unity/Editor/Unity.exe",
                    "%ProgramFiles%/Unity/Hub/Editor/2020.2.3f1/Editor/Unity.exe"
                }
            };

            settingsSource.Solutions = new List<SolutionSource>();
            settingsSource.Artifacts = new List<ArtifactDest>();
            settingsSource.Licenses = new List<LicenseSource>();

            var symOntoClaySolution = new SolutionSource()
            {
                Kind = KindOfProjectSource.GeneralSolution.ToString(),
                Path = "%BASE_PATH%/SymOntoClay",
                License = "MIT"
            };

            settingsSource.Solutions.Add(symOntoClaySolution);

            symOntoClaySolution.Projects = new List<ProjectSource>();

            var symOntoClayCoreProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.CoreLib.ToString(),
                Path = "SymOntoClayCore"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCoreProject);

            var symOntoClayUnityAssetCoreProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.CoreLib.ToString(),
                Path = "SymOntoClayUnityAssetCore"
            };

            symOntoClaySolution.Projects.Add(symOntoClayUnityAssetCoreProject);

            var symOntoClayCoreHelperProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.Library.ToString(),
                Path = "SymOntoClayCoreHelper"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCoreHelperProject);

            var symOntoClayProjectFilesProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.Library.ToString(),
                Path = "SymOntoClayProjectFiles"
            };

            symOntoClaySolution.Projects.Add(symOntoClayProjectFilesProject);

            var symOntoClayDefaultCLIEnvironmentProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.Library.ToString(),
                Path = "SymOntoClayDefaultCLIEnvironment"
            };

            symOntoClaySolution.Projects.Add(symOntoClayDefaultCLIEnvironmentProject);

            var symOntoClayCLIProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.CLI.ToString(),
                Path = "SymOntoClayCLI"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCLIProject);

            var symOntoClayUnity3DAssetTestProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.IntegrationTest.ToString(),
                Path = "SymOntoClayUnity3DAssetTest"
            };

            symOntoClaySolution.Projects.Add(symOntoClayUnity3DAssetTestProject);

            var testSandboxProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.AdditionalApp.ToString(),
                Path = "TestSandbox"
            };

            symOntoClaySolution.Projects.Add(testSandboxProject);

            var linguisticVariableViewerProject = new ProjectSource()
            {
                Kind = KindOfProjectSource.AdditionalApp.ToString(),
                Path = "LinguisticVariableViewer"
            };

            symOntoClaySolution.Projects.Add(linguisticVariableViewerProject);

            _logger.Info($"symOntoClaySolution = {symOntoClaySolution}");

            var siteSolution = new SolutionSource()
            {
                Kind = KindOfProjectSource.ProjectSite.ToString(),
                Path = "%BASE_PATH%/symontoclay.github.io",
                SourcePath = "%SLN_ROOT_PATH%/siteSource/"
            };

            settingsSource.Solutions.Add(siteSolution);

            _logger.Info($"siteSolution = {siteSolution}");

            var unitySolution = new SolutionSource()
            {
                Kind = KindOfProjectSource.Unity.ToString(),
                Path = "%BASE_PATH%/SymOntoClayAsset",
                SourcePath = "%SLN_ROOT_PATH%/Assets/SymOntoClay",
                License = "MIT"
            };

            _logger.Info($"unitySolution = {unitySolution}");

            settingsSource.Solutions.Add(unitySolution);

            var siteArtifact = new ArtifactDest()
            {
                Kind = KindOfArtifact.ProjectSite.ToString(),
                Path = "%BASE_PATH%/symontoclay.github.io"
            };

            _logger.Info($"siteArtifact = {siteArtifact}");

            settingsSource.Artifacts.Add(siteArtifact);

            var unityArtifact = new ArtifactDest()
            {
                Kind = KindOfArtifact.UnityPackage.ToString(),
                Path = "%USERPROFILE%/Documents/MyFirstSymOntoClayAsset_2.unitypackage"
            };

            _logger.Info($"unityArtifact = {unityArtifact}");

            settingsSource.Artifacts.Add(unityArtifact);

            var mitLicense = new LicenseSource()
            {
                Name = "MIT",
                FileName = "%APPDIR%/smallLicence.txt"
            };

            _logger.Info($"mitLicense = {mitLicense}");

            settingsSource.Licenses.Add(mitLicense);

            _logger.Info($"settingsSource = {settingsSource}");

            var settings = SymOntoClayProjectsSettingsConverter.Convert(settingsSource);

            _logger.Info($"settings = {settings}");
        }

        public static int A { get; set; }
    }
}
