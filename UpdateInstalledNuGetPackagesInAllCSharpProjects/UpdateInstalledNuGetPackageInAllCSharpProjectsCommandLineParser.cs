using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options.TypeCheckers;

namespace UpdateInstalledNuGetPackagesInAllCSharpProjects
{
    public class UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser : CommandLineParser
    {
        public UpdateInstalledNuGetPackageInAllCSharpProjectsCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>() 
            {
                new CommandLineArgument
                {
                    Name = "TargetPackageId",
                    Index = 0,
                    Kind = KindOfCommandLineArgument.SingleValue,
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
