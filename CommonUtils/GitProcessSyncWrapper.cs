using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonUtils
{
    public class GitProcessSyncWrapper : Disposable
    {
#if DEBUG
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public GitProcessSyncWrapper(string arguments)
        {
#if DEBUG
            _logger.Info($"arguments = '{arguments}'");
#endif

            _process = new Process();
            _process.StartInfo.FileName = "git.exe";
            _process.StartInfo.Arguments = arguments;
            _process.StartInfo.UseShellExecute = false;
            //_process.StartInfo.RedirectStandardOutput = true;
        }

        private readonly Process _process;

        public int Run()
        {
            _process.Start();

            //_process.BeginOutputReadLine();
            _process.WaitForExit();

            return _process.ExitCode;
        }

        /// <inheritdoc/>
        protected override void OnDisposing()
        {
            _process.Dispose();

            base.OnDisposing();
        }
    }
}
