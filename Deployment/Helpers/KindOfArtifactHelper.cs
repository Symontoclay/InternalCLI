using BaseDevPipeline;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
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

        public static KindOfReleaseAssetItem ConvertToKindOfReleaseAssetItem(KindOfArtifact kind)
        {
            switch(kind)
            {
                case KindOfArtifact.UnityPackage:
                    return KindOfReleaseAssetItem.Unity3DAsset;

                case KindOfArtifact.CLIArch:
                    return KindOfReleaseAssetItem.CLIArch;

                case KindOfArtifact.SourceArch:
                    return KindOfReleaseAssetItem.SourceCodeZip;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}
