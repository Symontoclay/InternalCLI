﻿using CommonUtils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public class TempDirectory: Disposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TempDirectory()
        {
            _dir = Path.Combine(Environment.GetEnvironmentVariable("TMP"), $"TempProjects_{Guid.NewGuid().ToString("D").Replace("-", string.Empty)}");

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
                    Directory.Delete(_dir, true);
                }catch
                {
                    _logger.Info($"Directory '{_dir}' has not been deleted.");
                }
            }            
        }
    }
}
