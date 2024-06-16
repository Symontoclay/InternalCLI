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

        public void Run()
        {
            _logger.Info("Begin");

            WrongCommandLine_Named_ErrorsList();
            WrongCommandLine_Named_Fail();
            WrongCommandLine_Positioned_ErrorsList();
            WrongCommandLine_Positioned_Fail();
            EmptyCommandLine_Success();
            ValidCommandLine_Named_Case6_Success();
            ValidCommandLine_Named_Case5_Success();
            ValidCommandLine_Named_Case4_Success();
            ValidCommandLine_Named_Case3_Success();
            ValidCommandLine_Named_Case2_Success();
            ValidCommandLine_Named_Case1_Success();
            ValidCommandLine_Positioned_Case6_Success();
            ValidCommandLine_Positioned_Case5_Success();
            ValidCommandLine_Positioned_Case4_Success();
            ValidCommandLine_Positioned_Case3_Success();
            ValidCommandLine_Positioned_Case2_Success();
            ValidCommandLine_Positioned_Case1_Success();

            _logger.Info("End");
        }

        private void WrongCommandLine_Named_ErrorsList()
        private void WrongCommandLine_Named_Fail()
        private void WrongCommandLine_Positioned_ErrorsList()
        private void WrongCommandLine_Positioned_Fail()
        private void EmptyCommandLine_Success()
        private void ValidCommandLine_Named_Case6_Success()
        private void ValidCommandLine_Named_Case5_Success()
        private void ValidCommandLine_Named_Case4_Success()
        private void ValidCommandLine_Named_Case3_Success()
        private void ValidCommandLine_Named_Case2_Success()
        private void ValidCommandLine_Named_Case1_Success()
        private void ValidCommandLine_Positioned_Case6_Success()
        private void ValidCommandLine_Positioned_Case5_Success()
        private void ValidCommandLine_Positioned_Case4_Success()
        private void ValidCommandLine_Positioned_Case3_Success()
        private void ValidCommandLine_Positioned_Case2_Success()
        private void ValidCommandLine_Positioned_Case1_Success()
    }
}
