using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public enum KindOfProjectSource
    {
        Unknown,
        ProjectSite,
        GeneralSolution,
        Unity,
        CLI,
        CoreLib,
        CorePlugin,
        Library,
        UnitTest,
        IntegrationTest,
        AdditionalApp
    }
}
