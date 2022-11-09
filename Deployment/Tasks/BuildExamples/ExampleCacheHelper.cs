using CommonUtils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildExamples
{
    public static class ExampleCacheHelper
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static string GetFileName(string lngExamplesPage, string exampleName)
        {
            return $"{MD5Helper.GetHash(lngExamplesPage)}_{exampleName}.json";
        }
    }
}
