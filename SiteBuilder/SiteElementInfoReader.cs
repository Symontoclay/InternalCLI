using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder
{
    public static class SiteElementInfoReader
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static List<SiteElementInfo> Read(string sourceDir, List<string> forbidenDirectoriesList, List<string> forbidenFileNamesList)
        {
#if DEBUG
            _logger.Info($"sourceDir = {sourceDir}");
#endif

            ProcessDirectory(sourceDir, forbidenDirectoriesList, forbidenFileNamesList);

#if DEBUG
            //_logger.Info($" = {}");
#endif

            throw new NotImplementedException();
        }

        private static void ProcessDirectory(string directory, List<string> forbidenDirectoriesList, List<string> forbidenFileNamesList)
        {
#if DEBUG
            _logger.Info($"directory = {directory}");
#endif

            var fileNamesList = Directory.EnumerateFiles(directory);

#if DEBUG
            _logger.Info($"fileNamesList = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif

            var indexSpFileName = fileNamesList.Where(p => p.EndsWith("index.sp")).SingleOrDefault();

#if DEBUG
            _logger.Info($"indexSpFileName = {indexSpFileName}");
#endif

            if(!string.IsNullOrWhiteSpace(indexSpFileName))
            {
                var indexTHtmlFileName = fileNamesList.Where(p => p.EndsWith("index.thtml")).Single();

#if DEBUG
                _logger.Info($"indexTHtmlFileName = {indexTHtmlFileName}");
#endif

                fileNamesList = fileNamesList.Except(new List<string> { indexSpFileName, indexTHtmlFileName });

#if DEBUG
                _logger.Info($"fileNamesList (2) = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif

                //throw new NotImplementedException();
            }

            var spFileNamesList = fileNamesList.Where(p => p.EndsWith(".sp")).ToList();

#if DEBUG
            _logger.Info($"spFileNamesList = {JsonConvert.SerializeObject(spFileNamesList, Formatting.Indented)}");
#endif

            if(spFileNamesList.Any())
            {
                var exceptFileNamesList = new List<string>();

                foreach(var spFileName in spFileNamesList)
                {
                    var fileInfo = new FileInfo(spFileName);

#if DEBUG
                    _logger.Info($"fileInfo.Name = {fileInfo.Name}");
#endif

                    var tHtmlFileName = spFileName.Replace(fileInfo.Name, fileInfo.Name.Replace(".sp", ".thtml"));

#if DEBUG
                    _logger.Info($"tHtmlFileName = {tHtmlFileName}");
#endif

                    if(!File.Exists(tHtmlFileName))
                    {
                        throw new FileNotFoundException(null, tHtmlFileName);
                    }

                    exceptFileNamesList.Add(spFileName);
                    exceptFileNamesList.Add(tHtmlFileName);
                }

                fileNamesList = fileNamesList.Except(exceptFileNamesList);

#if DEBUG
                _logger.Info($"fileNamesList (3) = {JsonConvert.SerializeObject(fileNamesList, Formatting.Indented)}");
#endif
            }

            foreach(var fileName in fileNamesList)
            {
                if(forbidenFileNamesList.Any(p => fileName.EndsWith(p)))
                {
                    continue;
                }

#if DEBUG
                _logger.Info($"fileName = {fileName}");
#endif
            }

            var subDirsList = Directory.EnumerateDirectories(directory).Where(p => !forbidenDirectoriesList.Contains(p)).ToList();

            foreach(var subDir in subDirsList)
            {
                ProcessDirectory(subDir, forbidenDirectoriesList, forbidenFileNamesList);
            }
        }
    }
}
