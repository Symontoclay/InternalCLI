using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options.TypeCheckers;
using System.Collections.Generic;

namespace StartNewVersion
{
    public class StartNewVersionCommandLineParser : CommandLineParser
    {
        public StartNewVersionCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>()
            {
                new CommandLineArgument
                {
                    Name = "TargetVersion",
                    Index = 0,
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
