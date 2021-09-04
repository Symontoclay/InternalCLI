using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class DeployedItemsFactory
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif
        
        public static string GetUnityAssetName(string version)
        {
            return $"SymOntoClay-{version}.unitypackage";
        }

        public static string GetCLIArchName(string version)
        {
            return $"SymOntoClay-CLI-{version}-x64.zip";
        }

        public static List<DeployedItem> Create(string version, List<KindOfArtifact> artifactsForDeployment, string baseHref, string basePath)
        {
            var targetAtrifactsList = KindOfArtifactHelper.GetOrderedListForDeployment().Intersect(artifactsForDeployment);

#if DEBUG
            //_logger.Info($"targetAtrifactsList = {targetAtrifactsList.WritePODList()}");
#endif

            var result = new List<DeployedItem>();

            foreach (var targetArtifact in targetAtrifactsList)
            {
#if DEBUG
                //_logger.Info($"targetArtifact = {targetArtifact}");
#endif

                switch (targetArtifact)
                {
                    case KindOfArtifact.UnityPackage:
                        {
                            var item = new DeployedItem()
                            {
                                Kind = targetArtifact
                            };

                            item.FileName = DeployedItemsFactory.GetUnityAssetName(version);

                            if(!string.IsNullOrWhiteSpace(baseHref))
                            {
                                item.Href = $"{baseHref}releases/download/{version}/{item.FileName}";
                            }

                            if (!string.IsNullOrWhiteSpace(basePath))
                            {
                                item.FullFileName = Path.Combine(basePath, item.FileName);
                            }

#if DEBUG
                            //_logger.Info($"item = {item}");
#endif

                            result.Add(item);
                        }
                        break;

                    case KindOfArtifact.CLIArch:
                        {
                            var item = new DeployedItem()
                            {
                                Kind = targetArtifact
                            };

                            item.FileName = GetCLIArchName(version);

                            if (!string.IsNullOrWhiteSpace(baseHref))
                            {
                                item.Href = $"{baseHref}releases/download/{version}/{item.FileName}";
                            }

                            if (!string.IsNullOrWhiteSpace(basePath))
                            {
                                item.FullFileName = Path.Combine(basePath, item.FileName);
                            }

#if DEBUG
                            //_logger.Info($"item = {item}");
#endif

                            result.Add(item);
                        }
                        break;

                    case KindOfArtifact.SourceArch:
                        {
                            var item = new DeployedItem()
                            {
                                Kind = targetArtifact,
                                IsAutomatic = true
                            };

                            item.FileName = $"{version}.zip";

                            if (!string.IsNullOrWhiteSpace(baseHref))
                            {
                                item.Href = $"{baseHref}archive/refs/tags/{item.FileName}";
                            }

#if DEBUG
                            //_logger.Info($"item = {item}");
#endif

                            result.Add(item);

                            item = new DeployedItem()
                            {
                                Kind = targetArtifact,
                                IsAutomatic = true
                            };

                            item.FileName = $"{version}.tar.gz";

                            if (!string.IsNullOrWhiteSpace(baseHref))
                            {
                                item.Href = $"{baseHref}archive/refs/tags/{item.FileName}";
                            }

#if DEBUG
                            //_logger.Info($"item = {item}");
#endif

                            result.Add(item);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(targetArtifact), targetArtifact, null);
                }
            }

            return result;
        }
    }
}
