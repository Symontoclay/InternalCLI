using NLog;
using System;
using System.Collections.Generic;
using UpdateTargetFrameworkInAllCSharpProjects;

namespace TestSandBox
{
    public class TstAppCmdlParsersHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            UpdateTargetFrameworkInAllCSharpProjects();

            _logger.Info("End");
        }

        private void UpdateTargetFrameworkInAllCSharpProjects()
        {
            //UpdateTargetFrameworkInAllCSharpProjectsCase1();
            UpdateTargetFrameworkInAllCSharpProjectsCase2();
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
        }

        private void UpdateTargetFrameworkInAllCSharpProjectsCase2()
        {
            try
            {
                var args = new List<string>();

                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info(e);
            }
        }
    }
}
