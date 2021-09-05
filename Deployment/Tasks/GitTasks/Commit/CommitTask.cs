using CommonUtils;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.Commit
{
    public class CommitTask : BaseDeploymentTask
    {
        public CommitTask(CommitTaskOptions options)
            : this(options, 0u)
        {
        }

        public CommitTask(CommitTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly CommitTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_options.RepositoryPath);

            var gitProcess = new GitProcessSyncWrapper($"commit -a -m \"{_options.Message}\"");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0 && !gitProcess.Output.Contains("nothing to commit"))
            {
                throw new Exception($"Commit changes to local branch in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}. | {string.Join(' ', gitProcess.Output)} | {string.Join(' ', gitProcess.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Commits changes to local branch in repository at path '{_options.RepositoryPath}'.");
            sb.AppendLine($"{spaces}The reason of the commit: '{_options.Message}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
