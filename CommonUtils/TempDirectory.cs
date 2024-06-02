using NLog;
using SymOntoClay.Common.Disposing;
using System;
using System.IO;

namespace CommonUtils
{
    public class TempDirectory : Disposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TempDirectory()
        {
            _dir = Path.Combine("D://", CreateDirName());
            //_dir = Path.Combine(Environment.GetEnvironmentVariable("TMP"), CreateDirName());

            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
        }

        private readonly string _dir;

        public string FullName => _dir;

        /// <inheritdoc/>
        protected override void OnDisposing()
        {
            base.OnDisposing();

            if (Directory.Exists(_dir))
            {
                try
                {
                    //Directory.Delete(_dir, true);
                }
                catch
                {
                    _logger.Info($"Directory '{_dir}' has not been deleted.");
                }
            }
        }

        public static string CreateDirName()
        {
            return $"TempProjects_{Guid.NewGuid().ToString("D").Replace("-", string.Empty)}";
        }
    }
}
