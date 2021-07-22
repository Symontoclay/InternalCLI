using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class GitRepositoryHelper
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static bool IsBranchExists(string repositoryPath, string branchName)
        {
#if DEBUG
            _logger.Info($"repositoryPath = '{repositoryPath}'");
            _logger.Info($"branchName = '{branchName}'");
#endif

            throw new NotImplementedException();
        }
    }
}
