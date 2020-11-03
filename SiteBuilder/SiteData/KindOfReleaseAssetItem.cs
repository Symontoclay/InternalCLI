using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public enum KindOfReleaseAssetItem
    {
        Unknown,
        SourceCodeZip,
        NuGet,
        LibraryArch,
        LibraryFolder,
        LibraryInstaller,
        LibraryFor3DAssetArch,
        LibraryFor3DAssetFolder,
        CLIArch,
        CLIFolder,
        CLIInstaller,
        Unity3DAsset
    }
}
