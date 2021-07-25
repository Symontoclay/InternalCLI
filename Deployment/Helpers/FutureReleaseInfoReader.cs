using BaseDevPipeline;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public class FutureReleaseInfoReader
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public FutureReleaseInfoReader()
            : this(new FutureReleaseInfoReaderOptions() {
                RepositoryPath = ProjectsDataSource.GetSolution( KindOfProject.ReleaseMngrSolution).Path
            })
        {
        }

        public FutureReleaseInfoReader(FutureReleaseInfoReaderOptions options)
        {
#if DEBUG
            _logger.Info($"options = {options}");
#endif
        }
    }
}
