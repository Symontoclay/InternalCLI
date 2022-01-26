using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public enum KindOfProject
    {
        Unknown,
        ProjectSite,
        CoreSolution,
        Unity,
        CLI,
        CoreLib,
        CoreAssetLib,
        CorePlugin,
        Library,
        UnitTest,
        IntegrationTest,
        AdditionalApp,
        ReleaseMngrSolution,
        /// <summary>
        /// Describes repository with Unity example demoscene.
        /// </summary>
        UnityExample
    }
}
