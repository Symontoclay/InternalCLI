using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public enum KindOfArtifact
    {
        Unknown,
        ProjectSite,
        UnityPackage,
        CLISetupExe,
        CLIArch,
        CLIFolder,
        NuGet,
        SourceArch
    }
}
