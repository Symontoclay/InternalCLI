using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;

namespace CheckInstalledNuGetPackagesInAllCSharpProjects
{
    public class CheckInstalledNuGetPackagesInAllCSharpProjectsCommandLineParser : CommandLineParser
    {
        public CheckInstalledNuGetPackagesInAllCSharpProjectsCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>
            {
                new CommandLineArgument()
                {
                    Name = "-ShowOnlyOutdated",
                    Kind = KindOfCommandLineArgument.Flag
                }
            }, initWithoutExceptions)
        {
        }
    }
}
