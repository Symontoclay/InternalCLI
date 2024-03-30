using CommonUtils;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.CreateBranch
{
    public class CreateBranchTask : OldBaseDeploymentTask
    {
        public CreateBranchTask(CreateBranchTaskOptions options)
            : this(options, 0u)
        {
        }

        public CreateBranchTask(CreateBranchTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly CreateBranchTaskOptions _options;

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

            var gitProcess = new GitProcessSyncWrapper($"branch {_options.BranchName}");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Creating branch '{_options.BranchName}' in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}.");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Creates new branch '{_options.BranchName}' in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
