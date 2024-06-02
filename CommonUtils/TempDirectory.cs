using NLog;
using SymOntoClay.Common.Disposing;
using System;
using System.IO;

namespace CommonUtils
{
    public class TempDirectory : Disposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TempDirectory(string rootDir = null, bool clearOnDisposing = true)
        {
            _clearOnDisposing = clearOnDisposing;

            _dir = Path.Combine(NormalizeRootDir(rootDir), CreateDirName());

            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
        }

        private string NormalizeRootDir(string rootDir)
        {
            if(string.IsNullOrWhiteSpace(rootDir))
            {//"D://"
                return Environment.GetEnvironmentVariable("TMP");
            }

            return rootDir;
        }

        private readonly string _dir;

        public string FullName => _dir;

        private readonly bool _clearOnDisposing;

        /// <inheritdoc/>
        protected override void OnDisposing()
        {
            base.OnDisposing();

            if(_clearOnDisposing)
            {
                if (Directory.Exists(_dir))
                {
                    try
                    {
                        Directory.Delete(_dir, true);
                    }
                    catch
                    {
                        _logger.Info($"Directory '{_dir}' has not been deleted.");
                    }
                }
            }
        }

        public static string CreateDirName()
        {
            return $"TempProjects_{Guid.NewGuid().ToString("D").Replace("-", string.Empty)}";
        }
    }
}
