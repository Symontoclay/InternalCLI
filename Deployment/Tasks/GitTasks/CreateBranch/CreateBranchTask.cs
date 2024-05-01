using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.IO;
using System.Text;

namespace Deployment.Tasks.GitTasks.CreateBranch
{
    public class CreateBranchTask : BaseDeploymentTask
    {
        public CreateBranchTask(CreateBranchTaskOptions options)
            : this(options, null)
        {
        }

        public CreateBranchTask(CreateBranchTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RepositoryPath), true, options, parentTask)
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
