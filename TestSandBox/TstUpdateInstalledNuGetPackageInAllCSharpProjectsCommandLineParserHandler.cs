using CSharpUtils;
using dotless.Core.Parser;
using NLog;
using System;
using System.Collections.Generic;
using UpdateInstalledNuGetPackagesInAllCSharpProjects;

namespace TestSandBox
{
    public class TstUpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParserHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //WrongVersion_Named_ErrorsList();//It should be tested
            //WrongVersion_Named_Fail();
            WrongVersion_Positioned_ErrorsList();//It should be tested
            //WrongVersion_Positioned_Fail();
            //EmptyCommandLine_ErrorsList();
            //EmptyCommandLine_Fail();
            //ValidCommandLine_Named_Success();
            //ValidCommandLine_Positioned_Success();

            _logger.Info("End");
        }

        private void WrongVersion_Named_ErrorsList()//It should be tested
        {
            var args = new List<string>()
                {
                    "TargetPackageId",
                    "NLog",
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongVersion_Named_Fail()
        {
            try
            {
                var args = new List<string>()
                    {
                        "TargetPackageId",
                        "NLog",
                        "TargetVersion",
                        "SomeVersion"
                    };

                var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void WrongVersion_Positioned_ErrorsList()//It should be tested
        {
            var args = new List<string>()
                {
                    "NLog",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongVersion_Positioned_Fail()
        {
            try
            {
                var args = new List<string>()
                    {
                        "NLog",
                        "SomeVersion"
                    };

                var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void EmptyCommandLine_ErrorsList()
        {
            var args = new List<string>();

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void EmptyCommandLine_Fail()
        {
            try
            {
                var args = new List<string>();

                var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void ValidCommandLine_Named_Success()
        {
            var args = new List<string>()
                {
                    "TargetPackageId",
                    "NLog",
                    "TargetVersion",
                    "5.1.4"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetPackageId = (string)result.Params["TargetPackageId"];

            _logger.Info($"targetPackageId = {targetPackageId}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }

        private void ValidCommandLine_Positioned_Success()
        {
            var args = new List<string>()
                {
                    "NLog",
                    "5.1.4"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetPackageId = (string)result.Params["TargetPackageId"];

            _logger.Info($"targetPackageId = {targetPackageId}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }
    }
}
