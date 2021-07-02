using BaseDevPipeline;
using CommonUtils;
using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

            TstProjectsDataSource();
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

        private static void TstProjectsDataSource()
        {
            _logger.Info("Begin");

            //var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            //_logger.Info($"settings = {settings}");

            ProjectsDataSource.SaveExampleFile("ProjectsDataSource_1.json");

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
