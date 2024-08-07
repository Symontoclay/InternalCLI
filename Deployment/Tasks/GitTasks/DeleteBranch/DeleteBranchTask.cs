﻿using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.IO;
using System.Text;

namespace Deployment.Tasks.GitTasks.DeleteBranch
{
    public class DeleteBranchTask : BaseDeploymentTask
    {
        public DeleteBranchTask(DeleteBranchTaskOptions options)
            : this(options, null)
        {
        }

        public DeleteBranchTask(DeleteBranchTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RepositoryPath), true, options, parentTask)
        {
            _options = options;
        }

        private readonly DeleteBranchTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.BranchName), _options.BranchName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_options.RepositoryPath);

            var argumentsSb = new StringBuilder("branch -d ");

            if (_options.IsOrigin)
            {
                argumentsSb.Append("-r origin/");
            }

            argumentsSb.Append(_options.BranchName);

            var gitProcess = new GitProcessSyncWrapper(argumentsSb.ToString());

            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Delete branch {(_options.IsOrigin ? "remote" : "local") } '{_options.BranchName}' in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}. | {string.Join(' ', gitProcess.Output)} | {string.Join(' ', gitProcess.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Deletes branch {(_options.IsOrigin ? "remote" : "local") } '{_options.BranchName}' in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
