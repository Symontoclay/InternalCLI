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

            //UpdateTargetFrameworkInAllCSharpProjectsCase5_1();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase5();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase4_1();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase4();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase3_1();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase3();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase2();
            UpdateTargetFrameworkInAllCSharpProjectsCase1_1();//It should be tested.
            //UpdateTargetFrameworkInAllCSharpProjectsCase1();

            _logger.Info("End");
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase5_1()//It should be tested.
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase5()//It should be tested.
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase4_1()//It should be tested.
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase4()//It should be tested.
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase3_1()//It should be tested.
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase3()//It should be tested.
        {
            try
            {
                var args = new List<string>()
                {
                    "CatStandard",
                    "2.0"
                };

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
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
                _logger.Info(e);
            }
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase1_1()//It should be tested.
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
