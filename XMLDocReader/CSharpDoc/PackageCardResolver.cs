using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class PackageCardResolver
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Resolve(List<PackageCard> packageCardsList, bool ignoreErrors)
        {
            _logger.Info($"ignoreErrors = {ignoreErrors}");
        }
    }
}
