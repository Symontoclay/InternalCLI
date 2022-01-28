using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonUtils
{
    public static class PathsHelper
    {
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
    }
}
