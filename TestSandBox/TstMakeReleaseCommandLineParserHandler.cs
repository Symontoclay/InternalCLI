using MakeRelease;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class TstMakeReleaseCommandLineParserHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //WrongCommandLine_Named_ErrorsList();
            //WrongCommandLine_Named_Fail();
            //WrongCommandLine_Positioned_ErrorsList();
            WrongCommandLine_Positioned_Fail();
            //EmptyCommandLine_Success();
            //ValidCommandLine_Named_Case6_Success();
            //ValidCommandLine_Named_Case5_Success();
            //ValidCommandLine_Named_Case4_Success();
            //ValidCommandLine_Named_Case3_Success();
            //ValidCommandLine_Named_Case2_Success();
            //ValidCommandLine_Named_Case1_Success();
            //ValidCommandLine_Positioned_Case6_Success();
            //ValidCommandLine_Positioned_Case5_Success();
            //ValidCommandLine_Positioned_Case4_Success();
            //ValidCommandLine_Positioned_Case3_Success();
            //ValidCommandLine_Positioned_Case2_Success();
            //ValidCommandLine_Positioned_Case1_Success();

            _logger.Info("End");
        }

        private void WrongCommandLine_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongCommandLine_Named_Fail()
        {
            try
            {
                var args = new List<string>()
                {
                    "RunMode",
                    "SomeWeirdMode"
                };

                var parser = new MakeReleaseCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void WrongCommandLine_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void WrongCommandLine_Positioned_Fail()
        {
            try
            {
                var args = new List<string>()
                {
                    "SomeWeirdMode"
                };

                var parser = new MakeReleaseCommandLineParser(false);

                var result = parser.Parse(args.ToArray());

                _logger.Info($"result = {result}");
            }
            catch (Exception e)
            {
                _logger.Info($"e.Message = '{e.Message}'");
                _logger.Info(e);
            }
        }

        private void EmptyCommandLine_Success()
        {
            var args = new List<string>();

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");
        }

        private void ValidCommandLine_Named_Case6_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Named_Case5_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "Prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Named_Case4_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Named_Case3_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "Test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Named_Case2_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "testFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Named_Case1_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "TestFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case6_Success()
        {
            var args = new List<string>()
                {
                    "prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case5_Success()
        {
            var args = new List<string>()
                {
                    "Prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case4_Success()
        {
            var args = new List<string>()
                {
                    "test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case3_Success()
        {
            var args = new List<string>()
                {
                    "Test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case2_Success()
        {
            var args = new List<string>()
                {
                    "testFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }

        private void ValidCommandLine_Positioned_Case1_Success()
        {
            var args = new List<string>()
                {
                    "TestFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            _logger.Info($"result = {result}");

            var runMode = (RunMode)result.Params["RunMode"];

            _logger.Info($"runMode = {runMode}");
        }
    }
}
