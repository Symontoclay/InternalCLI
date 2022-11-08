using BaseDevPipeline;
using BaseDevPipeline.SourceData;
using CommonMark;
using CommonUtils;
using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment;
using Deployment.DevTasks.BuildExamples;
using Deployment.DevTasks.CopyAndBuildVSProjectOrSolution;
using Deployment.DevTasks.CopyAndTest;
using Deployment.DevTasks.CoreToAsset;
using Deployment.DevTasks.CoreToInternalCLIDist;
using Deployment.DevTasks.CreateAndCommitChangeLogs;
using Deployment.DevTasks.CreateAndCommitCodeOfConducts;
using Deployment.DevTasks.CreateAndCommitContributings;
using Deployment.DevTasks.CreateAndCommitLicenses;
using Deployment.DevTasks.CreateAndCommitReadmes;
using Deployment.DevTasks.CreateExtendedDocFile;
using Deployment.DevTasks.CreateReadmes;
using Deployment.DevTasks.UpdateAndCommitUnityExampleRepositories;
using Deployment.DevTasks.UpdateUnityExampleRepository;
using Deployment.Helpers;
using Deployment.ReleaseTasks.DeploymentToProd;
using Deployment.ReleaseTasks.GitHubRelease;
using Deployment.ReleaseTasks.MarkAsCompleted;
using Deployment.Tasks;
using Deployment.Tasks.BuildChangeLog;
using Deployment.Tasks.BuildContributing;
using Deployment.Tasks.BuildExamples;
using Deployment.Tasks.BuildLicense;
using Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.ExamplesCreator;
using Deployment.Tasks.GitTasks.Clone;
using Deployment.Tasks.GitTasks.SetUpRepository;
using Deployment.Tasks.ProjectsTasks.PrepareUnityCSProjAndSolution;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NLog;
using Octokit;
using SiteBuilder;
using SiteBuilder.HtmlPreprocessors;
using SiteBuilder.HtmlPreprocessors.CodeHighlighting;
using SiteBuilder.SiteData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
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

            TstExampleCache();
            //TstMd5Hash();
            //TstCreateExtendedDocFileDevTask();
            //TstBson();
            //TstBuildExamplesDevTask();
            //TstCoreToInternalCLIDistDevTask();
            //TstPrepareLicenses();
            //TstExternalExit();
            //TstBuildExamplesTask();
            //TstBuildExamples();
            //TstSetUpRepositoryTask();
            //TstIniFile();
            //TstPrepareUnityCSProjAndSolutionTask();
            //TstExecuteMethod();
            //TstUpdateAndCommitUnityExampleRepositoriesDevTask();
            //TstCloneRepository();
            //TstUpdateUnityExampleRepository();
            //TstCopyAndBuild();
            //TstCopyAndTest();
            //TstBuildContributingTask();
            //TstBuildLicense();
            //TstUnityExampleSolutions();
            //TstBuildChangeLog();
            //TstEnumerateAssetsFiles();
            //TstCopyProjectSource();
            //TstSetXmlDocFileNameToCsProj();
            //TstRemoveDir();
            //TstRemoveDir();
            //TstFinishRelease0_3_6_p();
            //TstFinishRelease0_3_6();//<--- It has been used when 0.4.0 release has been filed.
            //TstFinishRelease0_3_2();
            //TstRestoreSlnInUnityProject();
            //TstTesting();
            //TstCreateReadme();//<==
            //TstInitCreateReadme();
            //TstCreateMyUnityPackageManifest();
            //TstChangeVersionInUnityPackageManifestModel();
            //TstUnityPackageManifestModel();
            //TstGetCurrrentBranch();
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
            //TstSiteSettings();
            //TstFutureReleaseInfo();
            //TstFutureReleaseInfoSource();
            //TstProjectsDataSource();
            //TstGetEnvironmentVariables();
            //TstReleaseItemsHandler();
            //TstLessHandler();
            //TstRoadMap();
            //TstReleaseTaskHandler();//<==
            //TstGitTasksHandler();
            //TstDeploymentTaskBasedBuildHandler();
            //TstSimplifyFullNameOfType();
            //TstCreateCSharpApiOptionsFile();
            //TstReadXMLDoc();
        }

        private static void TstExampleCache()
        {
            _logger.Info("Begin");

            var lngExamplesPage = @"c:\Users\sergiy.tolkachov\source\repos\InternalCLI\TestSandBox\bin\Debug\net6.0\runtimes\win\lib\netcoreapp3.0\System.Drawing.Common.dll";
            //var destDir = @"c:\Users\sergiy.tolkachov\source\repos\InternalCLI\TestSandBox\bin\Debug\net6.0\runtimes\win\lib\netstandard2.0\System.Security.Cryptography.ProtectedData.dll";
            var destDir = @"c:\Users\sergiy.tolkachov\source\repos\InternalCLI\TestSandBox\bin\Debug\net6.0\runtimes\win\lib\netstandard2.0";

            _logger.Info($"lngExamplesPage = '{lngExamplesPage}'");
            _logger.Info($"destDir = '{destDir}'");

            var longestBasePath = PathsHelper.GetLongestCommonPath(lngExamplesPage, destDir);

            _logger.Info($"longestBasePath = '{longestBasePath}'");

            var preparedFileName = lngExamplesPage.Replace(longestBasePath, string.Empty);

            _logger.Info($"preparedFileName = '{preparedFileName}'");

            var fileName = ExampleCacheHelper.GetFileName(lngExamplesPage, "Example1");

            _logger.Info($"fileName = '{fileName}'");

            var code = @"app PeaceKeeper
{
	on {: see(I, #a) :} with priority 1 => {
	     'D' >> @>log;
	}
}";

            _logger.Info($"code = '{code}'");

            var item = new ExampleCacheItem();




            _logger.Info($"item = {JsonConvert.SerializeObject(item, Formatting.Indented)}");

            _logger.Info("End");
        }

        private static void TstMd5Hash()
        {
            _logger.Info("Begin");

            var input = "SymOntoClay.Core.xml";

            _logger.Info($"input = '{input}'");

            var output = MD5Helper.GetHash(input);

            _logger.Info($"output = '{output}'");

            _logger.Info("End");
        }

        private static void TstCreateExtendedDocFileDevTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateExtendedDocFileDevTask(new CreateExtendedDocFileDevTaskOptions()
            {
                XmlDocFile = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCore\bin\Debug\netstandard2.0\SymOntoClay.Core.xml"),
                ExtendedDocFile = PathsHelper.Normalize(@"%USERPROFILE%\source\SymOntoClay.Core.json")
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstBson()
        {
            _logger.Info("Begin");

            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ReleaseNotes.json");

            //_logger.Info($"filePath = {filePath}");

            //var targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ReleaseNotes.bson");

            //_logger.Info($"targetFilePath = {targetFilePath}");

            //var obj = JsonConvert.DeserializeObject(File.ReadAllText(filePath));

            //var ms = new MemoryStream();

            //using var writer = new BsonWriter(ms);

            //var serializer = new JsonSerializer();
            //serializer.Serialize(writer, obj);

            //var data = Convert.ToBase64String(ms.ToArray());

            //File.WriteAllText(targetFilePath, data);

            _logger.Info("End");
        }

        private static void TstBuildExamplesDevTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildExamplesDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstCoreToInternalCLIDistDevTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CoreToInternalCLIDistDevTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstPrepareLicenses()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            //_logger.Info($"siteSolution = {siteSolution}");

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var license = ProjectsDataSource.GetLicense("MIT");

            _logger.Info($"license.HeaderContent = {license.HeaderContent}");

            var headerText = ContentPreprocessor.Run(license.HeaderContent, MarkdownStrategy.GenerateMarkdown, siteSettings);

            _logger.Info($"headerText = {headerText}");

            _logger.Info("End");
        }

        private static void TstExternalExit()
        {
            _logger.Info("Begin");

            Task.Run(() => {
                Thread.Sleep(10000);

                _logger.Info("External Exit!");

                Environment.Exit(0);
            });

            Thread.Sleep(50000);

            _logger.Info("End");
        }

        private static void TstBuildExamplesTask()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            //_logger.Info($"siteSolution = {siteSolution}");

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var socExePath = @"c:\Users\Acer\source\repos\SymOntoClay\SymOntoClayCLI\bin\Debug\net6.0\soc.exe";

            //_logger.Info($"socExePath = {socExePath}");

            //var lngExamplesPath = siteSettings.SiteSettings.LngExamplesPath;

            var lngExamplesPath = @"c:\Users\Acer\Documents\tmp3\";

            _logger.Info($"lngExamplesPath = {lngExamplesPath}");

            var lngExamplesPagesList = siteSettings.SiteSettings.LngExamplesPages;

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildExamplesTask(new BuildExamplesTaskOptions() {
                LngExamplesPages = lngExamplesPagesList,
                DestDir = lngExamplesPath,
                SocExePath = socExePath
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstBuildExamples()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            //_logger.Info($"siteSolution = {siteSolution}");

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var socExePath = @"c:\Users\Acer\source\repos\SymOntoClay\SymOntoClayCLI\bin\Debug\net6.0\soc.exe";

            //_logger.Info($"socExePath = {socExePath}");

            var tmpDir = @"c:\Users\Acer\Documents\tmp2\";

            _logger.Info($"tmpDir = {tmpDir}");

            if(Directory.Exists(tmpDir))
            {
                Directory.Delete(tmpDir, true);
            }

            Directory.CreateDirectory(tmpDir);

            //var processSyncWrapper = new ProcessSyncWrapper(socExePath, "new Example", tmpDir);

            //var exitCode = processSyncWrapper.Run();

            //_logger.Info($"exitCode = {exitCode}");

            //var mainSocFileName = Path.Combine(tmpDir, "Example", "Npcs", "Example", "Example.soc");

            //_logger.Info($"mainSocFileName = {mainSocFileName}");

            //_logger.Info($"processSyncWrapper.Output = {JsonConvert.SerializeObject(processSyncWrapper.Output, Formatting.Indented)}");

            var lngExamplesPath = siteSettings.SiteSettings.LngExamplesPath;

            _logger.Info($"lngExamplesPath = {lngExamplesPath}");

            var lngExamplesPagesList = siteSettings.SiteSettings.LngExamplesPages;

            foreach(var lngExamplesPage in lngExamplesPagesList)
            {
                _logger.Info($"lngExamplesPage = {lngExamplesPage}");

                var examplesList = CodeExampleReader.Read(lngExamplesPage);

                _logger.Info($"examplesList.Count = {examplesList.Count}");

                foreach (var example in examplesList)
                {
                    _logger.Info($"example = {example}");

                    var result = ExampleCreator.CreateExample(example, tmpDir, socExePath);

                    _logger.Info($"result = {result}");
                }
            }

            _logger.Info("End");
        }

        private static void TstSetUpRepositoryTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new SetUpRepositoryTask(new SetUpRepositoryTaskOptions() {
                RepositoryPath = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\__symontoclay.github.io")
            }));

            //_logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstIniFile()
        {
            _logger.Info("Begin");

            var fileName = Path.Combine(Directory.GetCurrentDirectory(), "tmp_config");

            _logger.Info($"fileName = {fileName}");

            var iniFile = new IniFile(fileName);

            var value = iniFile.ReadStringValue("core", "filemode");

            _logger.Info($"value = {value}");

            //iniFile.WriteStringValue("user", "name", "metatypeman");

            var intVal = iniFile.ReadInt32Value("core", "repositoryformatversion");

            _logger.Info($"intVal = {intVal}");

            var boolVal = iniFile.ReadBooleanValue("core", "filemode");

            _logger.Info($"boolVal = {boolVal}");

            _logger.Info("End");
        }

        private static void TstPrepareUnityCSProjAndSolutionTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new PrepareUnityCSProjAndSolutionTask(new PrepareUnityCSProjAndSolutionTaskOptions() {
                UnityExeFilePath = @"C:/Program Files/Unity/Hub/Editor/2021.2.9f1/Editor/Unity.exe",
                RootDir = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClayAsset")
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstExecuteMethod()
        {
            _logger.Info("Begin");

            //Yes!!! It regenerates .csproj and .sln file!
            var cmdStrBuilder = new StringBuilder(@"""C:/Program Files/Unity/Hub/Editor/2021.2.9f1/Editor/Unity.exe""");
            cmdStrBuilder.Append($" -quit -batchmode -projectPath \"{PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClayAsset").Replace("\\", "/")}\"");
            cmdStrBuilder.Append(" -executeMethod SymOntoClay.UnityAsset.Editors.EmptyScript.Run");

            _logger.Info($"cmdStrBuilder = {cmdStrBuilder}");

            _logger.Info("End");
        }

        private static void TstUpdateAndCommitUnityExampleRepositoriesDevTask()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateAndCommitUnityExampleRepositoriesDevTask());

            //_logger.Info($"deploymentPipeline = {deploymentPipeline}");

            //deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstCloneRepository()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CloneTask(new CloneTaskOptions() {
                RepositoryHref = "https://github.com/metatypeman/a1.git",
                RepositoryPath = PathsHelper.Normalize(@"%USERPROFILE%\source")
            }));

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstUpdateUnityExampleRepository()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new UpdateUnityExampleRepositoryDevTask(new UpdateUnityExampleRepositoryDevTaskOptions() {
                SourceRepository = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClayAsset"),
                DestinationRepository = @"d:\tmp\demo1_linear_code\"
            }));

            //_logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstCopyAndBuild()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = "a",
                SkipExistingFilesInTargetDir = false
            }));

            deploymentPipeline.Add(new CopyAndBuildVSProjectOrSolutionDevTask(new CopyAndBuildVSProjectOrSolutionDevTaskOptions() {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj"),
                //BuildConfiguration = KindOfBuildConfiguration.Release,
                OutputDir = Path.Combine(Directory.GetCurrentDirectory(), "a"),
                NoLogo = true
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstCopyAndTest()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CopyAndTestDevTask(new CopyAndTestDevTaskOptions()
            {
                ProjectOrSoutionFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\SymOntoClay.sln")
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstBuildContributingTask()
        {
            _logger.Info("Begin");

            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var siteSolution = settings.GetSolution(KindOfProject.ProjectSite);

            _logger.Info($"siteSolution = {siteSolution}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildContributingTask(new BuildContributingTaskOptions() {
                SiteSourcePath = siteSolution.SourcePath,
                SiteDestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
                SourceFileName = settings.ContributingSource,
                TargetFileName = Path.Combine(Directory.GetCurrentDirectory(), "CONTRIBUTING.md")
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstBuildLicense()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            _logger.Info($"siteSolution = {siteSolution}");

            //var repository = ProjectsDataSource.GetSolutionsWithMaintainedVersionsInCSharpProjects().First();

            var repository = siteSolution;

            _logger.Info($"repository = {repository}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildLicenseTask(new BuildLicenseTaskOptions() {
                SiteSourcePath = siteSolution.SourcePath,
                SiteDestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
                TargetFileName = Path.Combine(Directory.GetCurrentDirectory(), "LICENSE"),
                Content = repository.License.Content
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstUnityExampleSolutions()
        {
            _logger.Info("Begin");

            var unityExamplesSolutionsList = ProjectsDataSource.GetUnityExampleSolutions();

            foreach (var unityExamplesSolution in unityExamplesSolutionsList)
            {
                _logger.Info($"unityExamplesSolution = {unityExamplesSolution}");
            }

            _logger.Info("End");
        }

        private static void TstBuildChangeLog()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new BuildChangeLogTask(new BuildChangeLogTaskOptions() {
                TargetChangeLogFileName = Path.Combine(Directory.GetCurrentDirectory(), "CHANGELOG.md"),
                ReleaseNotesFilePath = CommonFileNamesHelper.BuildReleaseNotesPath()
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstEnumerateAssetsFiles()
        {
            _logger.Info("Begin");

            var sb = new StringBuilder();

            var slnFolder = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClayAsset\Assets\SymOntoClay");

            _logger.Info($"slnFolder = {slnFolder}");

            TstEnumerateAssetsFiles_ProcessDir(slnFolder, sb);

            slnFolder = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClayAsset\Assets\SymOntoClaySamles");

            _logger.Info($"slnFolder = {slnFolder}");

            TstEnumerateAssetsFiles_ProcessDir(slnFolder, sb);

            _logger.Info($"sb = {sb}");

            File.WriteAllText(Path.GetRandomFileName(), sb.ToString());

            _logger.Info("End");
        }

        private static void TstEnumerateAssetsFiles_ProcessDir(string dir, StringBuilder sb)
        {
            _logger.Info($"dir = {dir}");

            var subDirs = Directory.GetDirectories(dir);

            foreach (var subDir in subDirs)
            {
                TstEnumerateAssetsFiles_ProcessDir(subDir, sb);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                if (file.EndsWith(".meta"))
                {
                    continue;
                }

                if (file.EndsWith("LICENSE"))
                {
                    continue;
                }

                if (file.EndsWith("package.json"))
                {
                    continue;
                }

                if (file.EndsWith("README.md"))
                {
                    continue;
                }

                if (file.EndsWith("SymOntoClay.Core.dll"))
                {
                    continue;
                }

                if (file.EndsWith("SymOntoClay.CoreHelper.dll"))
                {
                    continue;
                }

                if (file.EndsWith("SymOntoClay.SoundBuses.dll"))
                {
                    continue;
                }

                if (file.EndsWith("SymOntoClay.UnityAsset.Core.dll"))
                {
                    continue;
                }

                var fileNameForPrinting = file.Substring(file.IndexOf("SymOntoClayAsset"));

                _logger.Info($"file = {file}");
                _logger.Info($"fileNameForPrinting = {fileNameForPrinting}");

                sb.AppendLine(fileNameForPrinting);
            }
        }

        private static void TstCopyProjectSource()
        {
            _logger.Info("Begin");

            var slnFolder = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClay");

            _logger.Info($"slnFolder = {slnFolder}");

            var targetFolder = @"d:\tmp\SymOntoClay_2";

            _logger.Info($"targetFolder = {targetFolder}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CopySourceFilesOfVSSolutionTask(new CopySourceFilesOfVSSolutionTaskOptions() {
                SourceDir = slnFolder,
                DestDir = targetFolder
            }));

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstCopyProjectSource_2()
        {
            _logger.Info("Begin");

            var slnFolder = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClay");

            _logger.Info($"slnFolder = {slnFolder}");

            var targetFolder = @"d:\tmp\SymOntoClay_1";

            _logger.Info($"targetFolder = {targetFolder}");

            TstCopyProjectSource_ProcessDir(slnFolder, targetFolder, slnFolder);

            _logger.Info("End");
        }

        private static void TstCopyProjectSource_ProcessDir(string dir, string targetFolder, string slnFolder)
        {
            if (dir.EndsWith(".git"))
            {
                return;
            }

            if (dir.EndsWith(".vs"))
            {
                return;
            }

            if (dir.EndsWith("bin"))
            {
                return;
            }

            if (dir.EndsWith("obj"))
            {
                return;
            }

            _logger.Info($"dir = {dir}");

            var newDirName = dir.Replace(slnFolder, targetFolder);

            _logger.Info($"newDirName = {newDirName}");

            if (!Directory.Exists(newDirName))
            {
                Directory.CreateDirectory(newDirName);
            }

            var subDirs = Directory.GetDirectories(dir);

            foreach (var subDir in subDirs)
            {
                TstCopyProjectSource_ProcessDir(subDir, targetFolder, slnFolder);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                _logger.Info($"file = {file}");

                var newFileName = file.Replace(slnFolder, targetFolder);

                _logger.Info($"newFileName = {newFileName}");

                File.Copy(file, newFileName, true);
            }
        }

        private static void TstSetXmlDocFileNameToCsProj()
        {
            _logger.Info("Begin");

            _logger.Info($"Path.GetDirectoryName(SymOntoClayCore.csproj) = {Path.GetDirectoryName("SymOntoClayCore.csproj")}");

            var projFileName = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClay\SymOntoClayCore\SymOntoClayCore.csproj");

            _logger.Info($"Path.GetDirectoryName(projFileName) = {Path.GetDirectoryName(projFileName)}");

            NTstSetXmlDocFileNameToCsProj(projFileName);

            projFileName = Path.Combine(EVPath.Normalize("%USERPROFILE%"), @"source\repos\SymOntoClayAsset\Assembly-CSharp.csproj");

            NTstSetXmlDocFileNameToCsProj(projFileName);

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projFileName);

            _logger.Info($"result = {result}");

            _logger.Info("End");
        }

        private static void NTstSetXmlDocFileNameToCsProj(string projFileName)
        {
            _logger.Info($"projFileName = {projFileName}");

            XElement project = null;

            using (var fs = File.OpenRead(projFileName))
            {
                project = XElement.Load(fs);
            }

            foreach (var projElem in project.Elements().Where(p => p.HasAttributes && p.Attributes().Any(x => x.Name == "Condition" && x.Value.Replace(" ", string.Empty).Trim() == "'$(Configuration)|$(Platform)'=='Debug|AnyCPU'")))
            {
                _logger.Info($"projElem = {projElem}");
            }

            var targetElem = project.Elements().FirstOrDefault(p => p.HasAttributes && p.Attributes().Any(x => x.Name == "Condition" && x.Value.Replace(" ", string.Empty).Trim() == "'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"));

            _logger.Info($"targetElem = {targetElem}");

            var documentationFileNode = targetElem.Element("DocumentationFile");

            _logger.Info($"documentationFileNode = {documentationFileNode}");
        }

        private static void TstRemoveDir()
        {
            _logger.Info("Begin");

            var dir = @"c:\Users\Acer\Documents\SymOntoClayCLIDist";

            //Directory.Move(dir, @"c:\Users\Acer\Documents\tmp2");

            //Thread.Sleep(100);

            //Directory.Delete(@"c:\Users\Acer\Documents\tmp2", true);//:(

            //try
            //{
            Directory.Delete(dir, true);
            //}
            //catch (Exception e)
            //{
            //    _logger.Info($"e = {e}");
            //}

            //var files = Directory.GetFiles(dir);

            //foreach (var file in files)
            //{
            //    _logger.Info($"file = {file}");

            //    try
            //    {
            //        File.SetAttributes(file, FileAttributes.Normal);
            //        File.Delete(file);//:(
            //    }
            //    catch (Exception e)
            //    {
            //        _logger.Info($"e = {e}");
            //    }

            //    try
            //    {
            //        var fileInfo = new FileInfo(file);
            //        fileInfo.Delete();
            //    }
            //    catch (Exception e)
            //    {
            //        _logger.Info($"e = {e}");
            //    }
            //}

            //var startInfo = new ProcessStartInfo("cmd.exe", @"rmdir /s /q ""c:\Users\Acer\Documents\SymOntoClayCLIDist\""");
            //startInfo.CreateNoWindow = false;
            //startInfo.UseShellExecute = true;

            //var proc = Process.Start(startInfo);

            //proc.WaitForExit();

            //_logger.Info($"proc.ExitCode = {proc.ExitCode}");

            _logger.Info("End");
        }

        private static void TstFinishRelease0_3_6_p()
        {
            var deploymentPipeline = new DeploymentPipeline();

            //Readmies
            deploymentPipeline.Add(new CreateAndCommitReadmesDevTask());

            //CHANGELOG.md
            deploymentPipeline.Add(new CreateAndCommitChangeLogsDevTask());

            //CODE_OF_CONDUCT.md
            deploymentPipeline.Add(new CreateAndCommitCodeOfConductsDevTask());

            //CONTRIBUTING.md
            deploymentPipeline.Add(new CreateAndCommitContributingsDevTask());

            //LICENSEs
            deploymentPipeline.Add(new CreateAndCommitLicensesDevTask());

            //Release to GitHub
            deploymentPipeline.Add(new GitHubReleaseReleaseTask());
            //-----
            deploymentPipeline.Add(new MarkAsCompletedReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();
        }

        private static void TstFinishRelease0_3_6()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new DeploymentToProdReleaseTask());

            deploymentPipeline.Add(new MarkAsCompletedReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }

        private static void TstFinishRelease0_3_2()
        {
            _logger.Info("Begin");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new GitHubReleaseReleaseTask());

            deploymentPipeline.Add(new MarkAsCompletedReleaseTask());

            _logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            _logger.Info("End");
        }
        
        private static void TstRestoreSlnInUnityProject()//:(
        {
            _logger.Info("Begin");

            var unityExeInstances = ProjectsDataSource.GetSymOntoClayProjectsSettings().UtityExeInstances;

            _logger.Info($"unityExeInstances = {JsonConvert.SerializeObject(unityExeInstances, Formatting.Indented)}");

            var unityExeFilePath = unityExeInstances.SingleOrDefault().Path;

            _logger.Info($"unityExeFilePath = {unityExeFilePath}");

            var unitySolution = ProjectsDataSource.GetSolution(KindOfProject.Unity);

            //_logger.Info($"unitySolution = {unitySolution}");

            var unitySolutionPath = unitySolution.Path;

            _logger.Info($"unitySolutionPath = {unitySolutionPath}");

            var commandLine = $"-quit -projectPath \"{unitySolutionPath.Replace("\\", "/")}\"";
            //-batchmode
            var execPath = $"\"{unityExeFilePath.Replace("\\", "/")}\"";

            _logger.Info($"commandLine = {commandLine}");
            _logger.Info($"execPath = {execPath}");

            var processWrapper = new ProcessSyncWrapper(execPath, commandLine);

            var exitCode = processWrapper.Run();

            _logger.Info($"processWrapper.Output = {processWrapper.Output}");
            _logger.Info($"processWrapper.Errors = {processWrapper.Errors}");
            _logger.Info($"exitCode = {exitCode}");

            _logger.Info("End");
        }

        [DebuggerHidden]
        private static void TstTesting()
        {
            _logger.Info("Begin");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var testAssemblyFileName = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClay\TestSandbox\bin\Debug\net5.0\SymOntoClay.UnityAsset.Core.Tests.dll");

            _logger.Info($"testAssemblyFileName = {testAssemblyFileName}");

            var targetAssembly = Assembly.LoadFrom(testAssemblyFileName);

            foreach (var type in targetAssembly.GetTypes().Where(p => p.GetMethods().Any(p => p.CustomAttributes.Any(x => x.AttributeType.FullName == "NUnit.Framework.TestAttribute"))))
            {
                _logger.Info($"type.FullName = {type.FullName}");

                var setUpMethod = type.GetMethods().SingleOrDefault(p => p.CustomAttributes.Any(x => x.AttributeType.FullName == "NUnit.Framework.SetUpAttribute"));

                foreach (var method in type.GetMethods().Where(p => p.CustomAttributes.Any(x => x.AttributeType.FullName == "NUnit.Framework.TestAttribute")))
                {
                    _logger.Info($"method.Name = {method.Name}");

                    try
                    {
                        var obj = targetAssembly.CreateInstance(type.FullName);

                        if(setUpMethod != null)
                        {
                            setUpMethod.Invoke(obj, new List<object>().ToArray());
                        }
                        
                        method.Invoke(obj, new List<object>().ToArray());
                    }catch(Exception e)
                    {
                        _logger.Info($"e = {e}");
                    }
                }
            }

            stopWatch.Stop();

            _logger.Info($"stopWatch.Elapsed = {stopWatch.Elapsed}");

            _logger.Info("End");
        }

        private static void TstCreateReadme()
        {
            _logger.Info("Begin");

            var targetSolutions = ProjectsDataSource.GetSolutionsWithMaintainedReleases();

            var targetSolution = targetSolutions.FirstOrDefault();

            //_logger.Info($"targetSolution = {targetSolution}");

            var sourceReadmePath = Path.Combine(targetSolution.Path, "README.md");

            //_logger.Info($"sourceReadmePath = {sourceReadmePath}");

            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(new CreateReadmesDevTask());

            //_logger.Info($"deploymentPipeline = {deploymentPipeline}");

            deploymentPipeline.Run();

            var readmeContent = File.ReadAllText(sourceReadmePath);

            //_logger.Info($"readmeContent = {readmeContent}");

            var htmlContent = CommonMarkConverter.Convert(readmeContent);

            //_logger.Info($"htmlContent = '{htmlContent}'");

            var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "README.html");

            File.WriteAllText(htmlPath, htmlContent);

            _logger.Info("End");
        }

        private static void TstInitCreateReadme()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            _logger.Info($"siteSolution = {siteSolution}");

            var siteSettings = new GeneralSiteBuilderSettings(new GeneralSiteBuilderSettingsOptions()
            {
                SourcePath = siteSolution.SourcePath,
                DestPath = siteSolution.Path,
                SiteName = siteSolution.RepositoryName,
            });

            var commonBadgesFileName = Path.Combine(Directory.GetCurrentDirectory(), "__common_BADGES.md");

            _logger.Info($"commonBadgesFileName = {commonBadgesFileName}");

            var commonReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "__common_README.md");

            _logger.Info($"commonReadmeFileName = {commonReadmeFileName}");

            var repositorySpecificBadgesFileName = string.Empty;

            _logger.Info($"repositorySpecificBadgesFileName = {repositorySpecificBadgesFileName}");

            var repositorySpecificReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "__ReadmeSource.md");

            _logger.Info($"repositorySpecificReadmeFileName = {repositorySpecificReadmeFileName}");

            var targetReadmeFileName = Path.Combine(Directory.GetCurrentDirectory(), "TargetReadme.md");

            _logger.Info($"targetReadmeFileName = {targetReadmeFileName}");

            var sb = new StringBuilder();

            var wasBadges = false;

            if(!string.IsNullOrWhiteSpace(commonBadgesFileName))
            {
                var content = File.ReadAllText(commonBadgesFileName);

                _logger.Info($"content = '{content}'");

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                _logger.Info($"content (after) = '{content}'");

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.Append(content);

                    wasBadges = true;
                }
            }

            if(!string.IsNullOrWhiteSpace(repositorySpecificBadgesFileName))
            {
                var content = File.ReadAllText(repositorySpecificBadgesFileName);

                _logger.Info($"content = '{content}'");

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                _logger.Info($"content (after) = '{content}'");

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.Append(content);

                    wasBadges = true;
                }
            }

            if(wasBadges)
            {
                sb.AppendLine();
                sb.AppendLine();
            }

            if(!string.IsNullOrWhiteSpace(repositorySpecificReadmeFileName))
            {
                var content = File.ReadAllText(repositorySpecificReadmeFileName);

                _logger.Info($"content = '{content}'");

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                _logger.Info($"content (after) = '{content}'");

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.AppendLine(content);
                    sb.AppendLine();
                }
            }

            if (!string.IsNullOrWhiteSpace(commonReadmeFileName))
            {
                var content = File.ReadAllText(commonReadmeFileName);

                _logger.Info($"content = '{content}'");

                content = ContentPreprocessor.Run(content, MarkdownStrategy.GenerateMarkdown, siteSettings);

                _logger.Info($"content (after) = '{content}'");

                if (!string.IsNullOrWhiteSpace(content))
                {
                    sb.AppendLine(content);
                    sb.AppendLine();
                }
            }

            File.WriteAllText(targetReadmeFileName, sb.ToString());

            _logger.Info($"sb.ToString() = '{sb}'");

            var htmlContent = CommonMarkConverter.Convert(sb.ToString());

            _logger.Info($"htmlContent = '{htmlContent}'");

            _logger.Info("End");
        }

        private static void TstCreateMyUnityPackageManifest()
        {
            _logger.Info("Begin");

            var manifest = new UnityPackageManifestModel
            {
                name = "symontoclay",
                version = "0.3.2",
                displayName = "SymOntoClay",
                description = "SymOntoClay is a hybrid language with logic programming and fuzzy logic for defining game characters behavior",
                unity = "2020.2",
                unityRelease = "3f1",
                documentationUrl = "https://symontoclay.github.io/docs/index.html",
                changelogUrl = "https://symontoclay.github.io/downloads/index.html",
                licensesUrl = "https://github.com/Symontoclay/SymOntoClayAsset/blob/master/LICENSE",
                keywords = new List<string>() { "AI" },
                author = new AuthorOfUnityPackageManifestModel()
                {
                    name = "Sergiy Tolkachov AKA metatypeman",
                    url = "https://symontoclay.github.io"
                }
            };

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            _logger.Info($"releaseNotesList = {JsonConvert.SerializeObject(manifest, Formatting.Indented, jsonSerializerSettings)}");

            UnityPackageManifestModelHelper.SaveCompactFile("package.json", manifest);

            _logger.Info("End");
        }

        private static void TstChangeVersionInUnityPackageManifestModel()
        {
            _logger.Info("Begin");

            var manifest = UnityPackageManifestModelHelper.Read("package_OnlyWithRequiredAndMandatoryProperties.json");

            _logger.Info($"manifest = {manifest}");

            var oldVersion = manifest.version;

            _logger.Info($"oldVersion = {oldVersion}");

            manifest.version = "3.1.0";

            UnityPackageManifestModelHelper.SaveCompactFile("package_OnlyWithRequiredAndMandatoryProperties_1.json", manifest);

            var newManifets = UnityPackageManifestModelHelper.Read("package_OnlyWithRequiredAndMandatoryProperties_1.json");

            _logger.Info($"newManifets = {newManifets}");

            _logger.Info("End");
        }

        private static void TstUnityPackageManifestModel()
        {
            _logger.Info("Begin");

            UnityPackageManifestModelHelper.SaveExampleFile("package_full_example.json");
            UnityPackageManifestModelHelper.SaveExampleFileOnlyWithRequiredAndMandatoryProperties("package_OnlyWithRequiredAndMandatoryProperties.json");

            _logger.Info("End");
        }

        private static void TstGetCurrrentBranch()
        {
            _logger.Info("Begin");

            var siteSolution = ProjectsDataSource.GetSolution(KindOfProject.ProjectSite);

            var currentBranchName = GitRepositoryHelper.GetCurrentBranchName(siteSolution.Path);

            _logger.Info($"currentBranchName = {currentBranchName}");

            _logger.Info($"GitRepositoryHelper.IsCurrentBranchMasterByBranchName(currentBranchName) = {GitRepositoryHelper.IsCurrentBranchMasterByBranchName(currentBranchName)}");

            _logger.Info($"GitRepositoryHelper.IsCurrentBranchMaster(siteSolution.Path) = {GitRepositoryHelper.IsCurrentBranchMaster(siteSolution.Path)}");

            _logger.Info("End");
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

            var version = "3.7.0";

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

        /*
           "UnityPaths": [
    "%ProgramFiles%/Unity/Editor/Unity.exe",
    "%ProgramFiles%/Unity/Hub/Editor/2021.1.20f1/Editor/Unity.exe",
    "%ProgramFiles%/Unity/Hub/Editor/2021.2.9f1/Editor/Unity.exe"
  ],
         */

        private static void TstProjectsDataSource()
        {
            _logger.Info("Begin");

            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            //_logger.Info($"settings = {settings}");

            //var unitySolution = ProjectsDataSource.GetSolution(KindOfProject.Unity);

            //_logger.Info($"unitySolution = {unitySolution}");

            //ProjectsDataSource.SaveExampleFile("ProjectsDataSource_1.json");

            _logger.Info($"settings.InternalCLIDist = {settings.InternalCLIDist}");
            _logger.Info($"settings.SocExePath = {settings.SocExePath}");

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

        private static void TstReleaseTaskHandler()
        {
            _logger.Info("Begin");

            var handler = new ReleaseTaskHandler();
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
                DocFiles = new List<string>()
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
