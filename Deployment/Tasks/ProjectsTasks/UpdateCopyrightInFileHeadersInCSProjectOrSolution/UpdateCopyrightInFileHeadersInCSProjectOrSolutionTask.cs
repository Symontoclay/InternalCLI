using CommonUtils;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeaders;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.Tasks.ProjectsTasks.UpdateCopyrightInFileHeadersInCSProjectOrSolution
{
    public class UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask : BaseDeploymentTask
    {
        public UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateCopyrightInFileHeadersInCSProjectOrSolutionTask(UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.SourceDir), false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateCopyrightInFileHeadersInCSProjectOrSolutionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var fileNamesGetterOptions = new FileNamesGetterOptions();

            fileNamesGetterOptions.SourceDir = _options.SourceDir;
            fileNamesGetterOptions.OnlyFileExts = new List<string>()
            {
                "cs"
            };

            fileNamesGetterOptions.ExceptSubDirs = new List<string>() 
            {
                ".git",
                ".vs",
                "bin",
                "obj"
            };

            var fileNamesGetter = new FileNamesGetter(fileNamesGetterOptions);

            var sourceFullFileNamesList = fileNamesGetter.GetFileNames();

            if (!sourceFullFileNamesList.Any())
            {
                return;
            }

            Exec(new UpdateCopyrightInFileHeadersTask(new UpdateCopyrightInFileHeadersTaskOptions()
            {
                Text = _options.Text,
                TargetFiles = sourceFullFileNamesList
            }, this));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates copyright in files' headers of project or solution '{_options.SourceDir}'.");
            sb.AppendLine($"{spaces}The copyright:");
            sb.AppendLine(_options.Text);
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
