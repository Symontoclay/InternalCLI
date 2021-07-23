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
        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public GitProcessSyncWrapper(string arguments)
        {
#if DEBUG
            //_logger.Info($"arguments = '{arguments}'");
#endif

            _process = new Process();
            _process.StartInfo.FileName = "git.exe";
            _process.StartInfo.Arguments = arguments;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;

            _process.OutputDataReceived += Process_OutputDataReceived;
            _process.ErrorDataReceived += Process_ErrorDataReceived;
        }

        private readonly Process _process;
        private readonly object _lockObj = new object();

        private readonly List<string> _output = new List<string>();
        private readonly List<string> _errors = new List<string>();

        public IReadOnlyList<string> Output => _output;
        public IReadOnlyList<string> Errors => _errors;

        public int Run()
        {
            _process.Start();

            _process.BeginOutputReadLine();
            _process.WaitForExit();

            return _process.ExitCode;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_lockObj)
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                {
                    return;
                }

                _output.Add(e.Data);
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_lockObj)
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                {
                    return;
                }

                _errors.Add(e.Data);
            }
        }

        /// <inheritdoc/>
        protected override void OnDisposing()
        {
            _process.Dispose();

            base.OnDisposing();
        }
    }
}
