﻿using NLog;
using SymOntoClay.Common.Disposing;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommonUtils
{
    public class ProcessSyncWrapper : Disposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ProcessSyncWrapper(string fileName, string arguments)
            : this(fileName, arguments, null)
        {
        }

        public ProcessSyncWrapper(string fileName, string arguments, string workingDirectory)
        {
            _process = new Process();
            _process.StartInfo.FileName = fileName;
            _process.StartInfo.Arguments = arguments;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;

            if(!string.IsNullOrWhiteSpace(workingDirectory))
            {
                _process.StartInfo.WorkingDirectory = workingDirectory;
            }

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
            _logger.Info($"{_process.StartInfo.FileName} {_process.StartInfo.Arguments}");

            _process.Start();
            _process.BeginOutputReadLine();
            _process.WaitForExit();

            _logger.Info($"ExitCode = {_process.ExitCode}");

            return _process.ExitCode;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_lockObj)
            {
#if DEBUG
                //_logger.Info($"e.Data = {e.Data}");
#endif

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
#if DEBUG
                //_logger.Info($"e.Data = {e.Data}");
#endif

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
