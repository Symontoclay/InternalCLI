using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    public static class BaseSolutionFilesHelper
    {
        private static List<string> _ignoredDirectories = new List<string>() { ".git", ".vs", "bin", "obj" };

        public static List<string> GetFileNames(string basePath, string fileExt)
        {
            if(!fileExt.StartsWith("."))
            {
                fileExt = $".{fileExt}";
            }

            var result = new List<string>();
            NEnumerateSourceFiles(basePath, result, fileExt);
            return result;
        }

        private static void NEnumerateSourceFiles(string path, List<string> result, string fileExtension)
        {
            var directoryInfo = new DirectoryInfo(path);

            var directoryName = directoryInfo.Name;

            if (_ignoredDirectories.Any(p => p == directoryName))
            {
                return;
            }

            var targetFilesList = Directory.EnumerateFiles(path).Where(p => p.EndsWith(fileExtension));

            result.AddRange(targetFilesList);

            var childDirectories = Directory.EnumerateDirectories(path);

            foreach (var childDirectory in childDirectories)
            {
                NEnumerateSourceFiles(childDirectory, result, fileExtension);
            }
        }
    }
}
