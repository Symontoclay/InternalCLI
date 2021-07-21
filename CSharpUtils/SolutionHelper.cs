using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
    public static class SolutionHelper
    {
        public static List<string> GetProjectsNames(string solutionPath)
        {
            return BaseSolutionFilesHelper.GetFileNames(solutionPath, ".csproj");
        }

        public static List<string> GetCSharpFileNames(string solutionPath)
        {
            return BaseSolutionFilesHelper.GetFileNames(solutionPath, ".cs");
        }
    }
}
