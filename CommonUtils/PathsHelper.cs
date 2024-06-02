using NLog;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace CommonUtils
{
    public static class PathsHelper
    {
#if DEBUG
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string Normalize(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            return EVPath.Normalize(path).Replace("\\", "/");
        }

        public static string GetSlnFolder(string projectOrSoutionFileName)
        {
            if (projectOrSoutionFileName.EndsWith(".sln"))
            {
                return Path.GetDirectoryName(projectOrSoutionFileName);
            }

            if(projectOrSoutionFileName.EndsWith(".csproj"))
            {
                var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(projectOrSoutionFileName));
                return directoryInfo.Parent.FullName;
            }

            throw new NotImplementedException();
        }

        public static string GetLongestCommonPath(string path1, string path2)
        {
#if DEBUG
            //_logger.Info($"path1 = '{path1}'");
            //_logger.Info($"path2 = '{path2}'");
#endif

            var pathList1 = PathToHierarchyList(path1);

#if DEBUG
            //_logger.Info($"pathList1 = {pathList1.WritePODList()}");
#endif

            var pathList2 = PathToHierarchyList(path2);

#if DEBUG
            //_logger.Info($"pathList2 = {pathList2.WritePODList()}");
#endif

            var pathList1Enumerator = pathList1.GetEnumerator();
            var pathList2Enumerator = pathList2.GetEnumerator();

            var prevPath = string.Empty;

            while(pathList1Enumerator.MoveNext())
            {
                var pathItem1 = pathList1Enumerator.Current;

#if DEBUG
                //_logger.Info($"pathItem1 = '{pathItem1}'");
#endif

                if(pathList2Enumerator.MoveNext())
                {
                    var pathItem2 = pathList2Enumerator.Current;

#if DEBUG
                    //_logger.Info($"pathItem2 = '{pathItem2}'");
#endif

                    if(pathItem1 == pathItem2)
                    {
                        prevPath = pathItem1;
                        continue;
                    }

                    break;
                }
                else
                {
                    break;
                }
            }

            return prevPath;
        }

        private static List<string> PathToHierarchyList(string path)
        {
#if DEBUG
            //_logger.Info($"path = '{path}'");
#endif

            var result = new List<string>() { path };

#if DEBUG
            //_logger.Info($"File.Exists(path) = {File.Exists(path)}");
            //_logger.Info($"Directory.Exists(path) = {Directory.Exists(path)}");
#endif

            if(File.Exists(path))
            {
                var fileInfo = new FileInfo(path);


                FillUpPathToHierarchyList(fileInfo.Directory, result);
            }
            else
            {
                if(Directory.Exists(path))
                {
                    var directoryInfo = new DirectoryInfo(path);
                    FillUpPathToHierarchyList(directoryInfo.Parent, result);
                }
            }

            result.Reverse();

            return result;
        }

        private static void FillUpPathToHierarchyList(DirectoryInfo directory, List<string> result)
        {
#if DEBUG
            //_logger.Info($"directory == null = {directory == null}");
            //_logger.Info($"directory?.FullName = '{directory?.FullName}'");
#endif

            if(directory == null)
            {
                return;
            }

            result.Add(directory.FullName);

            FillUpPathToHierarchyList(directory.Parent, result);
        }
    }
}
