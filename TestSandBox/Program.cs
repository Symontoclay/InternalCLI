using BaseDevPipeline;
using BaseDevPipeline.SourceData;
using CommonUtils;
using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.DevTasks.CoreToAsset;
using Deployment.Helpers;
using Newtonsoft.Json;
using NLog;
using Octokit;
using SiteBuilder;
using SiteBuilder.SiteData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using TestSandBox.XMLDoc;
using XMLDocReader.CSharpDoc;

namespace TestSandBox
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //TstDeployedItemsFactory();
            //TstAddReleaseNote();
            //TstReadAndReSaveReleaseNotes();
            //TstOctokit();
            //TstSecrets();
            //TstGitHubAPICreateRelease();
            //TstGitHubAPIGet();
            //TstTempDirectory();
            //TstCoreToAssetTask();
            //TstReadRepositoryFiles();
            TstSiteSettings();
            //TstFutureReleaseInfo();
            //TstFutureReleaseInfoSource();
            //TstProjectsDataSource();
            //TstGetEnvironmentVariables();
            //TstReleaseItemsHandler();
            //TstLessHandler();
            //TstRoadMap();
            //TstGitTasksHandler();
            //TstDeploymentTaskBasedBuildHandler();
            //TstSimplifyFullNameOfType();
            //TstCreateCSharpApiOptionsFile();
            //TstReadXMLDoc();
        }

        private static void TstDeployedItemsFactory()
        {
            _logger.Info("Begin");

            var deployedItemsList = DeployedItemsFactory.Create("0.3.2", new List<KindOfArtifact>()
                {
                    KindOfArtifact.SourceArch,
                    KindOfArtifact.CLIArch,
                    KindOfArtifact.UnityPackage
                }, "https://github.com/Symontoclay/SymOntoClay/", Directory.GetCurrentDirectory());

            _logger.Info($"deployedItemsList = {deployedItemsList.WriteListToString()}");

            _logger.Info("End");
        }

        private static void TstAddReleaseNote()
        {
            _logger.Info("Begin");

            var releaseNotesFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ReleaseNotes.json");

            _logger.Info($"releaseNotesFilePath = {releaseNotesFilePath}");

            var txt = File.ReadAllText(releaseNotesFilePath);

            _logger.Info($"txt = {txt}");

            var releaseNotesList = JsonConvert.DeserializeObject<List<ReleaseItem>>(txt);

            _logger.Info($"releaseNotesList = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");

            var version = "0.3.2";

            var newReleaseNote = new ReleaseItem()
            {
                Date = DateTime.Now.Date,
                Version = version,
                Description = @"* Example of Description
* Hello World!",
                IsMarkdown = true
            };

            //https://github.com/Symontoclay/SymOntoClay/archive/refs/tags/0.3.1.tar.gz
            //https://github.com/metatypeman/a1/releases/download/3.6.4/MyPackage-3.6.4.unitypackage

            _logger.Info($"newReleaseNote = {newReleaseNote}");

            _logger.Info($"newReleaseNote = {JsonConvert.SerializeObject(newReleaseNote, Formatting.Indented)}");

            releaseNotesList.Add(newReleaseNote);

            releaseNotesList = releaseNotesList.OrderByDescending(p => p.Date).ToList();

            _logger.Info($"releaseNotesList (2) = {JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented)}");

            var newTxt = JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented);

            _logger.Info($"newTxt = {newTxt}");

            File.WriteAllText("ReleaseNotes_2.json", newTxt);

            _logger.Info("End");
        }

        private static void TstReadAndReSaveReleaseNotes()
        {
            _logger.Info("Begin");

            var releaseNotesFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ReleaseNotes.json");

            var txt = File.ReadAllText(releaseNotesFilePath);

            _logger.Info($"txt = {txt}");

            var releaseNotesList = JsonConvert.DeserializeObject<List<ReleaseItem>>(txt);

            var newTxt = JsonConvert.SerializeObject(releaseNotesList, Formatting.Indented);

            _logger.Info($"newTxt = {newTxt}");

            File.WriteAllText("ReleaseNotes_1.json", newTxt);

            _logger.Info("End");
        }        

        private static void TstOctokit()
        {
            _logger.Info("Begin");

            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = SecretFile.ReadSecrets(secretsFileName);

            _logger.Info($"secrets = {JsonConvert.SerializeObject(secrets, Formatting.Indented)}");

            var token = secrets["GitHub"];
            var tokenAuth = new Credentials(token);
            client.Credentials = tokenAuth;

            var owner = "metatypeman";
            var repo = "a1";

            var version = "3.6.4";

            var newRelease = new NewRelease(version);
            newRelease.Name = version;
            newRelease.Body = "**This** is some *Markdown*";
            newRelease.Draft = false;
            newRelease.Prerelease = false;

            var resultTask = client.Repository.Release.Create(owner, repo, newRelease);

            resultTask.Wait();

            var result = resultTask.Result;
            _logger.Info($"Created release id {result.Id}");

            var packageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "cleanedNPCPackage.unitypackage");

            _logger.Info($"packageFilePath = {packageFilePath}");

            using (var archiveContents = File.OpenRead(packageFilePath))
            {
                var assetUpload = new ReleaseAssetUpload()
                {
                    FileName = $"MyPackage-{version}.unitypackage",
                    ContentType = "application/zip",
                    RawData = archiveContents
                };

                var release = client.Repository.Release.Get(owner, repo, result.Id);

                var assetTask = client.Repository.Release.UploadAsset(release.Result, assetUpload);

                assetTask.Wait();

                var asset = assetTask.Result;
            }

            _logger.Info("End");
        }

        private static void TstSecrets()
        {
            _logger.Info("Begin");

            //var release_id = TstGitHubAPICreateRelease();

            //var exampleFile = Path.Combine(Directory.GetCurrentDirectory(), "example_secret.json");

            //TstSecretFile.WriteExample(exampleFile);

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = SecretFile.ReadSecrets(secretsFileName);

            _logger.Info($"secrets = {JsonConvert.SerializeObject(secrets, Formatting.Indented)}");

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://uploads.github.com");
            var token = secrets["GitHub"];

            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/zip"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
            client.DefaultRequestHeaders.Host = "uploads.github.com";
            client.Timeout = new TimeSpan(1, 0, 0);


            //var release_id = 46355165;
            //var release_id = 46355447;
            var release_id = 46355625;

            _logger.Info($"release_id = {release_id}");

            var packageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "cleanedNPCPackage.unitypackage");

            _logger.Info($"packageFilePath = {packageFilePath}");

            var content = new StreamContent(File.OpenRead(packageFilePath));

            var owner = "metatypeman";
            var repo = "a1";

            var path = $"/repos/{owner}/{repo}/releases/{release_id}/assets?name=test.zip&label=some-binary.zip";

            _logger.Info($"path = {path}");

            var responseTask = client.PostAsync(path, content);

            responseTask.Wait();

            var responce = responseTask.Result;

            _logger.Info($"responce = {JsonConvert.SerializeObject(responce, Formatting.Indented)}");

            var responceContent = responce.Content;

            var readAsStringTask = responceContent.ReadAsStringAsync();

            ////readAsStringTask.Wait();

            var resultStr = readAsStringTask.Result;

            _logger.Info($"resultStr = {resultStr}");

            //var resultObj = JsonConvert.DeserializeObject<TstReleaseResponce>(resultStr);

            //_logger.Info($"resultObj.id = {resultObj.id}");

            _logger.Info("End");
        }

        private class TstReleaseResponce
        {
            public int id { get; set; }
        }

        private static int TstGitHubAPICreateRelease()
        {
            _logger.Info("Begin");

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = SecretFile.ReadSecrets(secretsFileName);

            _logger.Info($"secrets = {JsonConvert.SerializeObject(secrets, Formatting.Indented)}");

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = secrets["GitHub"];

            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var objToSerialize = new
            {
                tag_name = "3.6.0",
                body = "Hello, World! 3.6.0"
            };

            var content = JsonContent.Create(objToSerialize);

            _logger.Info($"content = {JsonConvert.SerializeObject(content, Formatting.Indented)}");

            var owner = "metatypeman";
            var repo = "a1";

            var path = $"/repos/{owner}/{repo}/releases";

            _logger.Info($"path = {path}");

            var responseTask = client.PostAsync(path, content);

            //responseTask.Wait();

            var responce = responseTask.Result;

            _logger.Info($"responce = {JsonConvert.SerializeObject(responce, Formatting.Indented)}");

            var responceContent = responce.Content;

            var readAsStringTask = responceContent.ReadAsStringAsync();

            //readAsStringTask.Wait();

            var resultStr = readAsStringTask.Result;

            _logger.Info($"resultStr = {resultStr}");

            var resultObj = JsonConvert.DeserializeObject<TstReleaseResponce>(resultStr);

            _logger.Info($"resultObj.id = {resultObj.id}");

            _logger.Info("End");

            return resultObj.id;
        }

        private static void TstGitHubAPIGet()
        {
            _logger.Info("Begin");

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = SecretFile.ReadSecrets(secretsFileName);

            _logger.Info($"secrets = {JsonConvert.SerializeObject(secrets, Formatting.Indented)}");

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = secrets["GitHub"];

            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var responseTask = client.GetAsync("/user");

            responseTask.Wait();

            var responce = responseTask.Result;

            _logger.Info($"responce = {JsonConvert.SerializeObject(responce, Formatting.Indented)}");

            _logger.Info("End");
        }

        private static void TstTempDirectory()
        {
            _logger.Info("Begin");

            using (var tempDir = new TempDirectory())
            {
                _logger.Info($"tempDir.FullName = {tempDir.FullName}");
            }

            _logger.Info("End");
        }

        private static void TstCoreToAssetTask()
        {
            _logger.Info("Begin");

            var coreToAssetTask = new CoreToAssetDevTask();
            coreToAssetTask.Run();

            _logger.Info("End");
        }

        private static void TstReadRepositoryFiles()
        {
            _logger.Info("Begin");

            var repositoryPath = EVPath.Normalize("%USERPROFILE%/source/repos/InternalCLI");

            _logger.Info($"repositoryPath = {repositoryPath}");

            var result = GitRepositoryHelper.GetRepositoryFileInfoList(repositoryPath);

            _logger.Info($"result = {result.WriteListToString()}");

            _logger.Info("End");
        }

        private static void TstSiteSettings()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            _logger.Info($"siteSolution = {siteSolution}");

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions() { 
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            _logger.Info("End");
        }

        private static void TstFutureReleaseInfo()
        {
            _logger.Info("Begin");

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            _logger.Info($"futureReleaseInfo = {futureReleaseInfo}");

            _logger.Info("End");
        }

        private static void TstFutureReleaseInfoSource()
        {
            _logger.Info("Begin");

            FutureReleaseInfoSource.SaveExampleFile("future_release_1.json");

            _logger.Info("End");
        }

        private static void TstProjectsDataSource()
        {
            _logger.Info("Begin");

            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            _logger.Info($"settings = {settings}");

            //ProjectsDataSource.SaveExampleFile("ProjectsDataSource_1.json");

            _logger.Info("End");
        }

        private static void TstGetEnvironmentVariables()
        {
            _logger.Info("Begin");

            var appName = AppDomain.CurrentDomain.FriendlyName;

            _logger.Info($"appName = {appName}");

            foreach (DictionaryEntry varName in Environment.GetEnvironmentVariables())
            {
                _logger.Info($"varName.Key = '{varName.Key}'; varName.Value = '{varName.Value}'");
            }

            _logger.Info("End");
        }

        private static void TstReleaseItemsHandler()
        {
            _logger.Info("Begin");

            var handler = new ReleaseItemsHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstLessHandler()
        {
            _logger.Info("Begin");

            var handler = new LessHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstRoadMap()
        {
            _logger.Info("Begin");

            var handler = new RoadMapHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstGitTasksHandler()
        {
            _logger.Info("Begin");

            var handler = new GitTasksHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstDeploymentTaskBasedBuildHandler()
        {
            _logger.Info("Begin");

            var handler = new DeploymentTaskBasedBuildHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstSimplifyFullNameOfType()
        {
            _logger.Info("Begin");

            var stringType = typeof(string);

            _logger.Info($"stringType.FullName = '{stringType.FullName}'");

            var name = NamesHelper.SimplifyFullNameOfType(stringType.FullName);

            _logger.Info($"name (1) = '{name}'");

            var typesDict = new Dictionary<string, List<Type[]>>();

            _logger.Info($"typesDict.GetType().FullName = '{typesDict.GetType().FullName}'");

            name = NamesHelper.SimplifyFullNameOfType(typesDict.GetType().FullName);

            _logger.Info($"name (2) = {name}");

            _logger.Info("End");
        }

        private static void TstCreateCSharpApiOptionsFile()
        {
            _logger.Info("Begin");

            var options = new CSharpApiOptions()
            {
                //SolutionDir = "%USERPROFILE%/Documents/GitHub/SymOntoClay",
                //AlternativeSolutionDir = "%USERPROFILE%/source/repos/SymOntoClay",
                XmlDocFiles = new List<string>()
                {
                    "%SITE_SOURCE_PATH%/CSharpApiFiles/SymOntoClay.CoreHelper.xml",
                    "%SITE_SOURCE_PATH%/CSharpApiFiles/SymOntoClay.Core.xml",
                    "%SITE_SOURCE_PATH%/CSharpApiFiles/SymOntoClay.UnityAsset.Core.xml"
                },
                UnityAssetCoreRootTypes = new List<string>()
                {
                    "T:SymOntoClay.UnityAsset.Core.WorldFactory"
                },
                PublicMembersOnly = true,
                IgnoreErrors = true
            };

            _logger.Info($"options = {options}");

            var jsonStr = JsonConvert.SerializeObject(options, Formatting.Indented);

            _logger.Info($"jsonStr = {jsonStr}");

            File.WriteAllText("CSharpApiOptions.json", jsonStr);

            _logger.Info("End");
        }

        private static void TstReadXMLDoc()
        {
            _logger.Info("Begin");

            var handler = new ReadXMLDocHandler();
            handler.Run();
            //handler.ParseGenericType();

            _logger.Info("End");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
