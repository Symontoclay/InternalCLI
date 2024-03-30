using CommonUtils;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildTasks.Test
{
    public class TestTask : OldBaseDeploymentTask
    {
        public TestTask(TestTaskOptions options)
            : this(options, 0u)
        {
        }

        public TestTask(TestTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly TestTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectOrSoutionFileName), _options.ProjectOrSoutionFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var sb = new StringBuilder($"test \"{_options.ProjectOrSoutionFileName}\" --configuration {_options.BuildConfiguration}");

            if (!string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.Append($" --output \"{_options.OutputDir}\"");
            }

            if (_options.NoLogo)
            {
                sb.Append(" --nologo");
            }

            if (!string.IsNullOrWhiteSpace(_options.RuntimeIdentifier))
            {
                sb.Append($" --runtime {_options.RuntimeIdentifier}");
            }

            var processWrapper = new ProcessSyncWrapper("dotnet", sb.ToString());

            var exitCode = processWrapper.Run();

            if (exitCode != 0)
            {
                throw new Exception($"Compilation of {_options.ProjectOrSoutionFileName} has been failed. | {string.Join(' ', processWrapper.Output)} | {string.Join(' ', processWrapper.Errors)}");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Tests project '{_options.ProjectOrSoutionFileName}' with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Tested files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Tested files will be put into '{_options.OutputDir}'.");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
