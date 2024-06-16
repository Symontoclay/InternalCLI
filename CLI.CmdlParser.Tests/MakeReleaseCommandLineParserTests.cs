using MakeRelease;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Exceptions;

namespace CLI.CmdlParser.Tests
{
    public class MakeReleaseCommandLineParserTests
    {
        [Test]
        public void ValidCommandLine_Positioned_Case1_Success()
        {
            var args = new List<string>()
                {
                    "TestFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.TestFirstProdNext));
        }

        [Test]
        public void ValidCommandLine_Positioned_Case2_Success()
        {
            var args = new List<string>()
                {
                    "testFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.TestFirstProdNext));
        }

        [Test]
        public void ValidCommandLine_Positioned_Case3_Success()
        {
            var args = new List<string>()
                {
                    "Test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Test));
        }

        [Test]
        public void ValidCommandLine_Positioned_Case4_Success()
        {
            var args = new List<string>()
                {
                    "test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Test));
        }

        [Test]
        public void ValidCommandLine_Positioned_Case5_Success()
        {
            var args = new List<string>()
                {
                    "Prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Prod));
        }

        [Test]
        public void ValidCommandLine_Positioned_Case6_Success()
        {
            var args = new List<string>()
                {
                    "prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Prod));
        }

        [Test]
        public void ValidCommandLine_Named_Case1_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "TestFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.TestFirstProdNext));
        }

        [Test]
        public void ValidCommandLine_Named_Case2_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "testFirstProdNext"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.TestFirstProdNext));
        }

        [Test]
        public void ValidCommandLine_Named_Case3_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "Test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Test));
        }

        [Test]
        public void ValidCommandLine_Named_Case4_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "test"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Test));
        }

        [Test]
        public void ValidCommandLine_Named_Case5_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "Prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Prod));
        }

        [Test]
        public void ValidCommandLine_Named_Case6_Success()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "prod"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var runMode = (RunMode)result.Params["RunMode"];

            Assert.That(runMode, Is.EqualTo(RunMode.Prod));
        }

        [Test]
        public void EmptyCommandLine_Success()
        {
            var args = new List<string>();

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongCommandLine_Positioned_Fail()
        {
            var args = new List<string>()
                {
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown run mode 'SomeWeirdMode'."));
        }

        [Test]
        public void WrongCommandLine_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown run mode 'SomeWeirdMode'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongCommandLine_Named_Fail()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown run mode 'SomeWeirdMode'."));
        }

        [Test]
        public void WrongCommandLine_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "RunMode",
                    "SomeWeirdMode"
                };

            var parser = new MakeReleaseCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown run mode 'SomeWeirdMode'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }
    }
}
