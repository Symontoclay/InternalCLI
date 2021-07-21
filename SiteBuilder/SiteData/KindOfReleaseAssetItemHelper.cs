using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteBuilder.SiteData
{
    public static class KindOfReleaseAssetItemHelper
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string GetAssetTitle(KindOfReleaseAssetItem kind, string href)
        {
#if DEBUG
            //_logger.Info($"href = {href}");
#endif

            var extension = GetFileExtension(href);
#if DEBUG
            //_logger.Info($"extension = {extension}");
#endif
            switch (kind)
            {
                case KindOfReleaseAssetItem.SourceCodeZip:
                    return $"Source code ({extension})";

                case KindOfReleaseAssetItem.NuGet:
                    return "Nuget package";

                case KindOfReleaseAssetItem.LibraryArch:
                    return "Engine (.zip)";

                case KindOfReleaseAssetItem.LibraryFolder:
                    return "Engine (folder)";

                case KindOfReleaseAssetItem.CLIArch:
                    return "Portable CLI (.zip)";

                case KindOfReleaseAssetItem.CLIFolder:
                    return "CLI (folder)";

                case KindOfReleaseAssetItem.Unity3DAsset:
                    return "Unity package";

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static string GetFileExtension(string href)
        {
            var fileInfo = new FileInfo(href);

            var extension = fileInfo.Extension;

#if DEBUG
            //_logger.Info($"href = {href}");

            //_logger.Info($"extension = {extension}");
#endif

            switch (extension)
            {
                case ".zip":
                case ".unitypackage":
                    return extension;

                case ".gz":
                    if (href.EndsWith(".tar.gz"))
                    {
                        return ".tar.gz";
                    }
                    throw new ArgumentOutOfRangeException(nameof(extension), extension, null);

                default:
                    throw new ArgumentOutOfRangeException(nameof(extension), extension, null);
            }
        }
    }
}
