using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class KindOfArtifactHelper
    {
        public static IEnumerable<KindOfArtifact> GetOrderedListForDeployment()
        {
            return new List<KindOfArtifact>()
            { 
                KindOfArtifact.UnityPackage,
                KindOfArtifact.NuGet,
                KindOfArtifact.CLISetupExe,
                KindOfArtifact.CLIArch,
                KindOfArtifact.SourceArch 
            };
        }
    }
}
