using CSharpUtils;
using NLog;
using System;
using System.Collections.Generic;
using UpdateTargetFrameworkInAllCSharpProjects;

namespace TestSandBox
{
    public class TstUpdateTargetFrameworkInAllCSharpProjectsCmdlParsersHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //UpdateTargetFrameworkInAllCSharpProjectsCase4_1();
            UpdateTargetFrameworkInAllCSharpProjectsCase4();
            //UpdateTargetFrameworkInAllCSharpProjectsCase3_1();
            //UpdateTargetFrameworkInAllCSharpProjectsCase3();
            //UpdateTargetFrameworkInAllCSharpProjectsCase2();
            //UpdateTargetFrameworkInAllCSharpProjectsCase1_1();
            //UpdateTargetFrameworkInAllCSharpProjectsCase1();

            _logger.Info("End");
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase4_1()
        {
            try
            {
                var args = new List<string>()
                {
                    "TargetFramework",
                    "NetStandard",
                    "TargetVersion",
                    "Cat"
                };

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase4()
        {
            try
            {
                var args = new List<string>()
                {
                    "NetStandard",
                    "Cat"
                };

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase3_1()
        {
            try
            {
                var args = new List<string>()
                {
                    "TargetFramework",
                    "CatStandard",
                    "TargetVersion",
                    "2.0"
                };

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase3()
        {
            try
            {
                var args = new List<string>()
                {
                    "CatStandard",
                    "2.0"
                };

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase2()
        {
            try
            {
                var args = new List<string>();

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase1_1()
        {
            var args = new List<string>()
            {
                "TargetFramework",
                "NetStandard",
                "TargetVersion",
                "2.0"
            };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetFramework = (KindOfTargetCSharpFramework)result.Params["TargetFramework"];

            _logger.Info($"targetFramework = {targetFramework}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase1()
        {
            var args = new List<string>()
            {
                "NetStandard",
                "2.0"
            };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetFramework = (KindOfTargetCSharpFramework)result.Params["TargetFramework"];

            _logger.Info($"targetFramework = {targetFramework}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }
    }
}
