using BaseDevPipeline.Data;
using BaseDevPipeline.Data.Implementation;
using BaseDevPipeline.SourceData;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class ProjectsDataSource
    {
        private static ISymOntoClayProjectsSettings _settings;
        private static object _lockObj = new object();

        public static ISymOntoClayProjectsSettings GetSymOntoClayProjectsSettings()
        {
            lock(_lockObj)
            {
                if(_settings == null)
                {
                    _settings = GetSymOntoClayProjectsSettings(Path.Combine(Directory.GetCurrentDirectory(), "ProjectsDataSource.json"));
                }

                return _settings;
            }
        }

        public static ISymOntoClayProjectsSettings GetSymOntoClayProjectsSettings(string fileName)
        {
            return SymOntoClayProjectsSettingsConverter.Convert(JsonConvert.DeserializeObject<SymOntoClaySettingsSource>(File.ReadAllText(fileName)));
        }

        public static void SaveExampleFile(string fileName)
        {
            var settingsSource = CreateExampleSettings();

            var jsonStr = JsonConvert.SerializeObject(settingsSource, Formatting.Indented);

            File.WriteAllText(fileName, jsonStr);
        }

        private static SymOntoClaySettingsSource CreateExampleSettings()
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
                Kind = KindOfProject.GeneralSolution.ToString(),
                Path = "%BASE_PATH%/SymOntoClay",
                License = "MIT"
            };

            settingsSource.Solutions.Add(symOntoClaySolution);

            symOntoClaySolution.Projects = new List<ProjectSource>();

            var symOntoClayCoreProject = new ProjectSource()
            {
                Kind = KindOfProject.CoreLib.ToString(),
                Path = "SymOntoClayCore"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCoreProject);

            var symOntoClayUnityAssetCoreProject = new ProjectSource()
            {
                Kind = KindOfProject.CoreAssetLib.ToString(),
                Path = "SymOntoClayUnityAssetCore"
            };

            symOntoClaySolution.Projects.Add(symOntoClayUnityAssetCoreProject);

            var symOntoClayCoreHelperProject = new ProjectSource()
            {
                Kind = KindOfProject.Library.ToString(),
                Path = "SymOntoClayCoreHelper"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCoreHelperProject);

            var symOntoClayProjectFilesProject = new ProjectSource()
            {
                Kind = KindOfProject.Library.ToString(),
                Path = "SymOntoClayProjectFiles"
            };

            symOntoClaySolution.Projects.Add(symOntoClayProjectFilesProject);

            var symOntoClayDefaultCLIEnvironmentProject = new ProjectSource()
            {
                Kind = KindOfProject.Library.ToString(),
                Path = "SymOntoClayDefaultCLIEnvironment"
            };

            symOntoClaySolution.Projects.Add(symOntoClayDefaultCLIEnvironmentProject);

            var symOntoClayCLIProject = new ProjectSource()
            {
                Kind = KindOfProject.CLI.ToString(),
                Path = "SymOntoClayCLI"
            };

            symOntoClaySolution.Projects.Add(symOntoClayCLIProject);

            var symOntoClayUnity3DAssetTestProject = new ProjectSource()
            {
                Kind = KindOfProject.IntegrationTest.ToString(),
                Path = "SymOntoClayUnity3DAssetTest"
            };

            symOntoClaySolution.Projects.Add(symOntoClayUnity3DAssetTestProject);

            var testSandboxProject = new ProjectSource()
            {
                Kind = KindOfProject.AdditionalApp.ToString(),
                Path = "TestSandbox"
            };

            symOntoClaySolution.Projects.Add(testSandboxProject);

            var linguisticVariableViewerProject = new ProjectSource()
            {
                Kind = KindOfProject.AdditionalApp.ToString(),
                Path = "LinguisticVariableViewer"
            };

            symOntoClaySolution.Projects.Add(linguisticVariableViewerProject);

            var siteSolution = new SolutionSource()
            {
                Kind = KindOfProject.ProjectSite.ToString(),
                Path = "%BASE_PATH%/symontoclay.github.io",
                SourcePath = "%SLN_ROOT_PATH%/siteSource/"
            };

            settingsSource.Solutions.Add(siteSolution);

            var unitySolution = new SolutionSource()
            {
                Kind = KindOfProject.Unity.ToString(),
                Path = "%BASE_PATH%/SymOntoClayAsset",
                SourcePath = "%SLN_ROOT_PATH%/Assets/SymOntoClay",
                License = "MIT"
            };

            settingsSource.Solutions.Add(unitySolution);

            var siteArtifact = new ArtifactDest()
            {
                Kind = KindOfArtifact.ProjectSite.ToString(),
                Path = "%BASE_PATH%/symontoclay.github.io"
            };

            settingsSource.Artifacts.Add(siteArtifact);

            var unityArtifact = new ArtifactDest()
            {
                Kind = KindOfArtifact.UnityPackage.ToString(),
                Path = "%USERPROFILE%/Documents/MyFirstSymOntoClayAsset_2.unitypackage"
            };

            settingsSource.Artifacts.Add(unityArtifact);

            var mitLicense = new LicenseSource()
            {
                Name = "MIT",
                FileName = "%APPDIR%/smallLicence.txt"
            };

            settingsSource.Licenses.Add(mitLicense);

            return settingsSource;
        }
    }
}
