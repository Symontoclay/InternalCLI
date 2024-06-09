using CSharpUtils;
using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options.TypeCheckers;

namespace UpdateTargetFrameworkInAllCSharpProjects
{
    public class UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser : CommandLineParser
    {
        public UpdateTargetFrameworkInAllCSharpProjectsCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>()
            {
                new CommandLineArgument
                {
                    Name = "TargetFramework",
                    Index = 0,
                    Kind = KindOfCommandLineArgument.SingleValue,
                    TypeChecker = new EnumChecker<KindOfTargetCSharpFramework>(),
                    TypeCheckErrorMessage = "Unknown target framework",
                    IsRequired = true
                },
                new CommandLineArgument
                {
                    Name = "TargetVersion",
                    Index = 1,
                    Kind = KindOfCommandLineArgument.SingleValue,
                    TypeChecker = new VersionChecker(),
                    TypeCheckErrorMessage = "Unknown version",
                    IsRequired = true
                }
            }, initWithoutExceptions)
        {
        }
    }
}
