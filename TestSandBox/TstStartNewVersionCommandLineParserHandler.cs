using NLog;
using StartNewVersion;
using System;
using System.Collections.Generic;

namespace TestSandBox
{
    public class TstStartNewVersionCommandLineParserHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //WrongVersion_Named_ErrorsList();
            //WrongVersion_Named_Fail();
            //WrongVersion_Positioned_ErrorsList();
            //WrongVersion_Positioned_Fail();
            //EmptyCommandLine_ErrorsList();
            //EmptyCommandLine_Fail();
            //ValidCommandLine_Named_Success();
            ValidCommandLine_Positioned_Success();

            _logger.Info("End");
        }

        private void WrongVersion_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongVersion_Named_Fail()
        {
            try
            {
                var args = new List<string>()
                {
                    "TargetVersion",
                    "SomeVersion"
                };

                var parser = new StartNewVersionCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void WrongVersion_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongVersion_Positioned_Fail()
        {
            try
            {
                var args = new List<string>()
                {
                    "SomeVersion"
                };

                var parser = new StartNewVersionCommandLineParser(false);

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

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void EmptyCommandLine_Fail()
        {
            try
            {
                var args = new List<string>();

                var parser = new StartNewVersionCommandLineParser(false);

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
                    "TargetVersion",
                    "5.1.4"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }

        private void ValidCommandLine_Positioned_Success()
        {
            var args = new List<string>()
                {
                    "5.1.4"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var targetVersion = (Version)result.Params["TargetVersion"];

            _logger.Info($"targetVersion = {targetVersion}");
        }
    }
}
