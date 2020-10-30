using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
    public static class SolutionHelper
    {
        private static List<string> _ignoredDirectories = new List<string>() { ".git", ".vs", "bin", "obj" };

        public static List<string> GetProjectsNames(string solutionPath)
        {
            var result = new List<string>();
            NEnumerateProjectsInSolution(solutionPath, result, ".csproj");
            return result;
        }

        public static List<string> GetSCharpFileNames(string solutionPath)
        {
            var result = new List<string>();
            NEnumerateProjectsInSolution(solutionPath, result, ".cs");
            return result;
        }

        private static void NEnumerateProjectsInSolution(string path, List<string> result, string fileExtension)
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
                NEnumerateProjectsInSolution(childDirectory, result, fileExtension);
            }
        }
    }
}
