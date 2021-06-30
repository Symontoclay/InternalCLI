using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    public static class PathsHelper
    {
        public static string Normalize(string path)
        {
            return EVPath.Normalize(path).Replace("\\", "/");
        }
    }
}
