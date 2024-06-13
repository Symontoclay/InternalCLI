using CSharpUtils;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Exceptions;
using UpdateTargetFrameworkInAllCSharpProjects;

namespace CLI.CmdlParser.Tests
{
    public class UpdateTargetFrameworkInAllCSharpProjectsCommandLineParserTest
    {
        [Test]
        public void ValidCommandLine_Success()
        {
            var args = new List<string>()
            {
                "NetStandard",
                "2.0"
            };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(0));

            Assert.That(result.Params.Count, Is.EqualTo(2));

            var targetFramework = (KindOfTargetCSharpFramework)result.Params["TargetFramework"];

            Assert.That(targetFramework, Is.EqualTo(KindOfTargetCSharpFramework.NetStandard));

            var targetVersion = (Version)result.Params["TargetVersion"];

            Assert.That(targetVersion, Is.EqualTo(new Version(2, 0)));
        }

        [Test]
        public void EmptyCommandLine_Fail()
        {
            var args = new List<string>();

            var exception = Assert.Catch<RequiredOptionException>(() => {
                var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Required command line argument 'TargetFramework' must be entered."));
        }

        [Test]
        public void EmptyCommandLine_ErrorsList()
        {
            var args = new List<string>();

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(2));

            Assert.That(result.Errors[0], Is.EqualTo("Required command line argument 'TargetFramework' must be entered."));
            Assert.That(result.Errors[1], Is.EqualTo("Required command line argument 'TargetVersion' must be entered."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }
    }
}
