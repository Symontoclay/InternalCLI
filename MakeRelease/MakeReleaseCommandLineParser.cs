using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options.TypeCheckers;
using System.Collections.Generic;

namespace MakeRelease
{
    public class MakeReleaseCommandLineParser : CommandLineParser
    {
        public MakeReleaseCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>()
            {
                new CommandLineArgument
                {
                    Name = "RunMode",
                    Index = 0,
                    Kind = KindOfCommandLineArgument.SingleValue,
                    TypeChecker = new EnumChecker<RunMode>(),
                    TypeCheckErrorMessage = "Unknown run mode",
                    IsUnique = true
                }
            }, initWithoutExceptions)
        {
        }
    }
}
