using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.IO;
using System.Text;

namespace Deployment.Tasks.GitTasks.Add
{
    public class AddTask : BaseDeploymentTask
    {
        public AddTask(AddTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.RepositoryPath), false, options, parentTask)
        {
            _options = options;
        }

        private readonly AddTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.RepositoryPath), _options.RepositoryPath);
            ValidateFileName(nameof(_options.RelativeFileName), _options.RelativeFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_options.RepositoryPath);

            var gitProcess = new GitProcessSyncWrapper($"add \"{_options.RelativeFileName}\"");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Commit changes to local branch in repository at path '{_options.RepositoryPath}' has been failed! The exit code is {exitCode}. | {string.Join(' ', gitProcess.Output)} | {string.Join(' ', gitProcess.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Adds file '{_options.RelativeFileName}' to Git source control in repository at path '{_options.RepositoryPath}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
