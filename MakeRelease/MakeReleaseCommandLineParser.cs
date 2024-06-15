using SymOntoClay.CLI.Helpers.CommandLineParsing;
using SymOntoClay.CLI.Helpers.CommandLineParsing.Options;
using System.Collections.Generic;

namespace MakeRelease
{
    public class MakeReleaseCommandLineParser : CommandLineParser
    {
        public MakeReleaseCommandLineParser(bool initWithoutExceptions)
            : base(new List<BaseCommandLineArgument>()
            {
            }, initWithoutExceptions)
        { 
        }
    }
}
