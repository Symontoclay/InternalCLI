using CommonUtils;
using NLog;
using System;
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

            EVPath.RegVar("APPDIR", Directory.GetCurrentDirectory());

            TstBuild();
            //TstSimplifyFullNameOfType();
            //TstReadXMLDoc();
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
