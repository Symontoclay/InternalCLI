using CommonUtils;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.UndoChanges
{
    public class UndoChangesTask : BaseDeploymentTask
    {
        public UndoChangesTask(UndoChangesTaskOptions options)
            : this(options, 0u)
        {
        }

        public UndoChangesTask(UndoChangesTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly UndoChangesTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.RelativeTargetFilePath), _options.RelativeTargetFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_options.RepositoryPath);

            var gitProcess = new GitProcessSyncWrapper($"restore \"{_options.RelativeTargetFilePath}\"");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Undo changes of current branch for file '{_options.RelativeTargetFilePath}' in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}.");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Undos changes of current branch for file '{_options.RelativeTargetFilePath}' in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
