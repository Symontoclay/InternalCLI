using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox
{
    public class TstMakeReleaseCommandLineParserHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private enum RunMode
        {
            TestFirstProdNext,
            Test,
            Prod
        }

        public void Run()
        {
            _logger.Info("Begin");

            var tstEnum = Enum.Parse<RunMode>("prod", true);

            _logger.Info($"tstEnum = {tstEnum}");

            _logger.Info("End");
        }
    }
}
