using NLog;

namespace TestSandBox
{
    public class CSharpProjectHelperTestsHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            _logger.Info("End");
        }
    }
}
