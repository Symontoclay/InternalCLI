using StartNewVersion;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Exceptions;

namespace CLI.CmdlParser.Tests
{
    public class StartNewVersionCommandLineParserTests
    {
        [Test]
        public void ValidCommandLine_Positioned_Success()
        {
            var args = new List<string>()
                {
                    "5.1.4"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var targetVersion = (Version)result.Params["TargetVersion"];

            Assert.That(targetVersion, Is.EqualTo(new Version(5, 1, 4)));
        }

        [Test]
        public void ValidCommandLine_Named_Success()
        {
            var args = new List<string>()
                {
                    "TargetVersion",
                    "5.1.4"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(1));

            var targetVersion = (Version)result.Params["TargetVersion"];

            Assert.That(targetVersion, Is.EqualTo(new Version(5, 1, 4)));
        }

        [Test]
        public void EmptyCommandLine_Fail()
        {
            var args = new List<string>();

            var parser = new StartNewVersionCommandLineParser(false);

            var exception = Assert.Catch<RequiredOptionException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Required command line argument 'TargetVersion' must be entered."));
        }

        [Test]
        public void EmptyCommandLine_ErrorsList()
        {
            var args = new List<string>();

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Required command line argument 'TargetVersion' must be entered."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongVersion_Positioned_Fail()
        {
            var args = new List<string>()
                {
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown version 'SomeVersion'."));
        }

        [Test]
        public void WrongVersion_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown version 'SomeVersion'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongVersion_Named_Fail()
        {
            var args = new List<string>()
                {
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown version 'SomeVersion'."));
        }

        [Test]
        public void WrongVersion_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new StartNewVersionCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown version 'SomeVersion'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }
    }
}
