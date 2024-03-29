﻿using CommonUtils;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.SetUpRepository
{
    public class SetUpRepositoryTask : BaseDeploymentTask
    {
        public SetUpRepositoryTask(SetUpRepositoryTaskOptions options)
            : this(options, 0u)
        {
        }

        public SetUpRepositoryTask(SetUpRepositoryTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly SetUpRepositoryTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var configFileName = Path.Combine(_options.RepositoryPath, ".git", "config");

            var iniFile = new IniFile(configFileName);

            var longpaths = iniFile.ReadBooleanValue("core", "longpaths");

            if(!longpaths.HasValue || longpaths == false)
            {
                iniFile.WriteBooleanValue("core", "longpaths", true);
            }

            var prune = iniFile.ReadBooleanValue("fetch", "prune");

            if(!prune.HasValue || prune == false)
            {
                iniFile.WriteBooleanValue("fetch", "prune", true);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Sets up repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
