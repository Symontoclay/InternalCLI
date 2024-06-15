using CSharpUtils;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Exceptions;
using UpdateTargetFrameworkInAllCSharpProjects;

namespace CLI.CmdlParser.Tests
{
    public class UpdateTargetFrameworkInAllCSharpProjectsCommandLineParserTests
    {
        [Test]
        public void ValidCommandLine_Positioned_Success()
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
        public void ValidCommandLine_Named_Success()
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

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<RequiredOptionException>(() => {  
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

        [Test]
        public void WrongTargetFrameworkRightVersion_Positioned_Fail()
        {
            var args = new List<string>()
                {
                    "CatStandard",
                    "2.0"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown target framework 'CatStandard'."));
        }

        [Test]
        public void WrongTargetFrameworkRightVersion_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "CatStandard",
                    "2.0"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown target framework 'CatStandard'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongTargetFrameworkRightVersion_Named_Fail()
        {
            var args = new List<string>()
                {
                    "TargetFramework",
                    "CatStandard",
                    "TargetVersion",
                    "2.0"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown target framework 'CatStandard'."));
        }

        [Test]
        public void WrongTargetFrameworkRightVersion_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "TargetFramework",
                    "CatStandard",
                    "TargetVersion",
                    "2.0"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown target framework 'CatStandard'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongVersionRightTargetFramework_Positioned_Fail()
        {
            var args = new List<string>()
                {
                    "NetStandard",
                    "Cat"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown version 'Cat'."));
        }

        [Test]
        public void WrongVersionRightTargetFramework_Positioned_ErrorsList()
        {
            var args = new List<string>()
                {
                    "NetStandard",
                    "Cat"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown version 'Cat'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }

        [Test]
        public void WrongVersionRightTargetFramework_Named_Fail()
        {
            var args = new List<string>()
                {
                    "TargetFramework",
                    "NetStandard",
                    "TargetVersion",
                    "Cat"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(false);

            var exception = Assert.Catch<TypeCheckingException>(() => {
                parser.Parse(args.ToArray());
            });

            Assert.That(exception.Message, Is.EqualTo("Unknown version 'Cat'."));
        }

        [Test]
        public void WrongVersionRightTargetFramework_Named_ErrorsList()
        {
            var args = new List<string>()
                {
                    "TargetFramework",
                    "NetStandard",
                    "TargetVersion",
                    "Cat"
                };

            var parser = new UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(true);

            var result = parser.Parse(args.ToArray());

            Assert.That(result.Errors.Count, Is.EqualTo(1));

            Assert.That(result.Errors[0], Is.EqualTo("Unknown version 'Cat'."));

            Assert.That(result.Params.Count, Is.EqualTo(0));
        }
    }
}
