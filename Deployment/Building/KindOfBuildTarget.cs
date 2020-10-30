using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public enum KindOfBuildTarget
    {
        Unknown,
        NuGet,
        LibraryArch,
        LibraryFolder,
        LibraryFor3DAssetArch,
        LibraryFor3DAssetFolder,
        CLIArch,
        CLIFolder,
        Unity3DAsset
    }
}
