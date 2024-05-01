using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.IO;
using System.Text;

namespace Deployment.Tasks.GitTasks.Pull
{
    public class PullTask : BaseDeploymentTask
    {
        public PullTask(PullTaskOptions options)
            : this(options, null)
        {
        }

        public PullTask(PullTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RepositoryPath), false, options, parentTask)
        {
            _options = options;
        }

        private readonly PullTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_options.RepositoryPath);

            var gitProcess = new GitProcessSyncWrapper("pull");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Get commits of current branch from origin to local in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}.");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Gets commits of current branch from origin to local in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
