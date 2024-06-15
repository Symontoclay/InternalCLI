using SymOntoClay.CLI.Helpers.CommandLineParsing.Exceptions;
using UpdateInstalledNuGetPackagesInAllCSharpProjects;

namespace CLI.CmdlParser.Tests
{
    public class UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParserTests
    {
        [Test]
        public void ValidCommandLine_Positioned_Success()
        {
            var args = new List<string>()
            {
                "NLog",
                "5.1.4"
            };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(2));

            var targetPackageId = (string)result.Params["TargetPackageId"];

            Assert.That(targetPackageId, Is.EqualTo("NLog"));

            var targetVersion = (Version)result.Params["TargetVersion"];

            Assert.That(targetVersion, Is.EqualTo(new Version(5, 1, 4)));
        }

        [Test]
        public void ValidCommandLine_Named_Success()
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

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(2));

            var targetPackageId = (string)result.Params["TargetPackageId"];

            Assert.That(targetPackageId, Is.EqualTo("NLog"));

            var targetVersion = (Version)result.Params["TargetVersion"];

            Assert.That(targetVersion, Is.EqualTo(new Version(5, 1, 4)));
        }

        [Test]
        public void EmptyCommandLine_Fail()
        {
            var args = new List<string>();

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<RequiredOptionException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Required command line argument 'TargetPackageId' must be entered."));
        }

        [Test]
        public void EmptyCommandLine_ErrorsList()
        {
            var args = new List<string>();

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(2));

            Assert.That(result.Errors[0], Is.EqualTo("Required command line argument 'TargetPackageId' must be entered."));
            Assert.That(result.Errors[1], Is.EqualTo("Required command line argument 'TargetVersion' must be entered."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongVersion_Positioned_Fail()
        {
            var args = new List<string>()
                {
                    "NLog",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

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
                    "NLog",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

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
                    "TargetPackageId",
                    "NLog",
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(false);

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
                    "TargetPackageId",
                    "NLog",
                    "TargetVersion",
                    "SomeVersion"
                };

            var parser = new UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown version 'SomeVersion'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }
    }
}
