using BaseDevPipeline;
using CommonUtils;
using Deployment;
using Deployment.DevPipelines.CoreToAsset;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;
using TestSandBox.XMLDoc;
using XMLDocReader;
using XMLDocReader.CSharpDoc;

namespace TestSandBox
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TstSecrets();
            //TstGitHubAPICreateRelease();
            //TstGitHubAPIGet();
            //TstTempDirectory();
            //TstCoreToAssetTask();
            //TstProjectsDataSource();
            //TstGetEnvironmentVariables();
            //TstReleaseItemsHandler();
            //TstLessHandler();
            //TstRoadMap();
            //TstDeploymentTaskBasedBuildHandler();
            //TstBuild();
            //TstSimplifyFullNameOfType();
            //TstCreateCSharpApiOptionsFile();
            //TstReadXMLDoc();
        }
        
        private static void TstSecrets()
        {
            _logger.Info("Begin");

            //var release_id = TstGitHubAPICreateRelease();

            //var exampleFile = Path.Combine(Directory.GetCurrentDirectory(), "example_secret.json");

            //TstSecretFile.WriteExample(exampleFile);

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = TstSecretFile.ReadSecrets(secretsFileName);

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

        //https://api.github.com/repos/metatypeman/a1/releases/46355625/assets
        ///repos/metatypeman/a1/releases/46355625/assets
        /*
         
         {
        "url":"https://api.github.com/repos/metatypeman/a1/releases/46355625",
        "assets_url":"https://api.github.com/repos/metatypeman/a1/releases/46355625/assets",
        "upload_url":"https://uploads.github.com/repos/metatypeman/a1/releases/46355625/assets{?name,label}",
        "html_url":"https://github.com/metatypeman/a1/releases/tag/3.6.0",
        "id":46355625,
        "author":{
            "login":"metatypeman",
            "id":13446159,
            "node_id":"MDQ6VXNlcjEzNDQ2MTU5",
            "avatar_url":"https://avatars.githubusercontent.com/u/13446159?v=4",
            "gravatar_id":"",
            "url":"https://api.github.com/users/metatypeman",
            "html_url":"https://github.com/metatypeman",
            "followers_url":"https://api.github.com/users/metatypeman/followers",
            "following_url":"https://api.github.com/users/metatypeman/following{/other_user}",
            "gists_url":"https://api.github.com/users/metatypeman/gists{/gist_id}",
            "starred_url":"https://api.github.com/users/metatypeman/starred{/owner}{/repo}",
            "subscriptions_url":"https://api.github.com/users/metatypeman/subscriptions",
            "organizations_url":"https://api.github.com/users/metatypeman/orgs",
            "repos_url":"https://api.github.com/users/metatypeman/repos",
            "events_url":"https://api.github.com/users/metatypeman/events{/privacy}",
            "received_events_url":"https://api.github.com/users/metatypeman/received_events",
            "type":"User",
            "site_admin":false
        },
        "node_id":"MDc6UmVsZWFzZTQ2MzU1NjI1",
        "tag_name":"3.6.0",
        "target_commitish":"main",
        "name":null,
        "draft":false,
        "prerelease":false,
        "created_at":"2021-07-16T14:41:51Z",
        "published_at":"2021-07-17T09:29:23Z",
        "assets":[],
        "tarball_url":"https://api.github.com/repos/metatypeman/a1/tarball/3.6.0",
        "zipball_url":"https://api.github.com/repos/metatypeman/a1/zipball/3.6.0",
        "body":"Hello, World! 3.6.0"
        } 
         */

        private class TstReleaseResponce
        {
            public int id { get; set; }
        }

        private static int TstGitHubAPICreateRelease()
        {
            _logger.Info("Begin");

            var secretsFileName = EVPath.Normalize("%USERPROFILE%/example_s.json");

            _logger.Info($"secretsFileName = {secretsFileName}");

            var secrets = TstSecretFile.ReadSecrets(secretsFileName);

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

            var secrets = TstSecretFile.ReadSecrets(secretsFileName);

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

            var coreToAssetTask = new CoreToAssetDevPipeline();
            coreToAssetTask.Run();

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

        private static void TstDeploymentTaskBasedBuildHandler()
        {
            _logger.Info("Begin");

            var handler = new DeploymentTaskBasedBuildHandler();
            handler.Run();

            _logger.Info("End");
        }

        private static void TstBuild()
        {
            _logger.Info("Begin");

            var handler = new BuildHandler();
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
                SolutionDir = "%USERPROFILE%/Documents/GitHub/SymOntoClay",
                AlternativeSolutionDir = "%USERPROFILE%/source/repos/SymOntoClay",
                XmlDocFiles = new List<string>()
                {
                    "SymOntoClayCoreHelper/SymOntoClay.CoreHelper.xml",
                    "SymOntoClayCore/SymOntoClay.Core.xml",
                    "SymOntoClayUnityAssetCore/SymOntoClay.UnityAsset.Core.xml"
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
