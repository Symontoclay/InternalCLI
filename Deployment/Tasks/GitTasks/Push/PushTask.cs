using CommonUtils;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.Push
{
    public class PushTask : OldBaseDeploymentTask
    {
        public PushTask(PushTaskOptions options)
            : this(options, 0u)
        {
        }

        public PushTask(PushTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly PushTaskOptions _options;

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

            var gitProcess = new GitProcessSyncWrapper("push origin");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Push current branch to origin in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}. | {string.Join(' ', gitProcess.Output)} | {string.Join(' ', gitProcess.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Push current branch to origin in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
