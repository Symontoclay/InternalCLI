using BaseDevPipeline.SourceData;
using CommonUtils;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public static class SymOntoClayProjectsSettingsConvertor
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static ISymOntoClayProjectsSettings Convert(SymOntoClaySettingsSource source)
        {
            var result = new SymOntoClayProjectsSettings();
            result.BasePath = DetectBasePath(source.BasePaths);
            result.UtityExeInstances = DetectUnities(source.UnityPaths);

            _logger.Info($"result = {result}");

            //throw new NotImplementedException();

            return result;
        }

        private static string DetectBasePath(List<string> basePaths)
        {
            var normalizedBasePaths = basePaths.Select(p => PathsHelper.Normalize(p));

            _logger.Info($"normalizedBasePaths = {JsonConvert.SerializeObject(normalizedBasePaths, Formatting.Indented)}");

            var existingBasePaths = normalizedBasePaths.Where(p => Directory.Exists(p));

            _logger.Info($"existingBasePaths = {JsonConvert.SerializeObject(existingBasePaths, Formatting.Indented)}");

            var count = existingBasePaths.Count();

            switch (count)
            {
                case 1:
                    return existingBasePaths.Single();
            }

            throw new NotImplementedException();
        }

        private static List<UtityExeInstance> DetectUnities(List<string> unityPaths)
        {
            var normalizedUnityPaths = unityPaths.Select(p => PathsHelper.Normalize(p));

            _logger.Info($"normalizedUnityPaths = {JsonConvert.SerializeObject(normalizedUnityPaths, Formatting.Indented)}");

            var existingUnityPaths = normalizedUnityPaths.Where(p => File.Exists(p));

            _logger.Info($"existingUnityPaths = {JsonConvert.SerializeObject(existingUnityPaths, Formatting.Indented)}");

            throw new NotImplementedException();
        }
    }
}
