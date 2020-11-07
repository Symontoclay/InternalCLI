using dotless.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSandBox
{
    public class LessHandler
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public void Run()
        {
            _logger.Info("Begin");

            var inputStr = @"div { width: 1 + 1 }";

            _logger.Info($"inputStr = {inputStr}");

            var outPutStr = Less.Parse(inputStr);

            _logger.Info($"outPutStr = {outPutStr}");

            inputStr = "div { width: 2px; }";

            _logger.Info($"inputStr (2) = {inputStr}");

            outPutStr = Less.Parse(inputStr);

            _logger.Info($"outPutStr (2) = {outPutStr}");

            _logger.Info("End");
        }
    }
}
