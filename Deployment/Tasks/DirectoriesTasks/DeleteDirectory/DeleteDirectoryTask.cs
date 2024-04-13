using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Deployment.Tasks.DirectoriesTasks.DeleteDirectory
{
    public class DeleteDirectoryTask : BaseDeploymentTask
    {
        public DeleteDirectoryTask(DeleteDirectoryTaskOptions options)
            : this(options, null)
        {
        }

        public DeleteDirectoryTask(DeleteDirectoryTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.TargetDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly DeleteDirectoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.TargetDir), _options.TargetDir);
        }

        /// <inheritdoc/>
        [DebuggerHidden]
        protected override void OnRun()
        {
            if(!Directory.Exists(_options.TargetDir))
            {
                return;
            }

            NDeleteDir(_options.TargetDir);
        }

        private void NDeleteDir(string targetDir)
        {
#if DEBUG
            //_logger.Info($"targetDir = {targetDir}");
#endif

            var files = Directory.GetFiles(targetDir);

            foreach (var file in files)
            {
#if DEBUG
                //_logger.Info($"file = {file}");
#endif

                var isFirstAttempt = true;
                var oldCount = 0;

                while(true)
                {
                    var lstProcs = ProcessHandler.WhoIsLocking(file);

#if DEBUG
                    //_logger.Info($"lstProcs.Count = {lstProcs.Count}");
#endif

                    if (lstProcs.Count == 0)
                    {
                        File.Delete(file); 
                        break;
                    }

#if DEBUG
                    //_logger.Info($"lstProcs.Count = {lstProcs.Count}");
#endif

                    if(isFirstAttempt)
                    {
                        _logger.Info($"Waiting for access to {file}");
                        foreach (var p in lstProcs)
                        {
                            _logger.Info($"p.MachineName = {p.MachineName}; p.ProcessName = {p.ProcessName}");
                        }

                        oldCount = lstProcs.Count;
                        isFirstAttempt = false;
                    }

                    if(oldCount != lstProcs.Count)
                    {
                        oldCount = lstProcs.Count;

                        _logger.Info($"Waiting for access to {file}");
                        foreach (var p in lstProcs)
                        {
                            _logger.Info($"p.MachineName = {p.MachineName}; p.ProcessName = {p.ProcessName}");
                        }
                    }

                    Thread.Sleep(50);
                }
            }

            foreach (var subDir in Directory.GetDirectories(targetDir))
            {
                NDeleteDir(subDir);
            }

            Directory.Delete(targetDir);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Creates directory '{_options.TargetDir}' if It needs.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
