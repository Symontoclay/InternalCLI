using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildTasks.Publish
{
    public class PublishTask : BaseDeploymentTask
    {
        public PublishTask(PublishTaskOptions options)
        {
            _options = options;
        }

        private readonly PublishTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectOrSoutionFileName), _options.ProjectOrSoutionFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var sb = new StringBuilder($"publish \"{_options.ProjectOrSoutionFileName}\" --configuration {_options.BuildConfiguration}");

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

            sb.Append($" --self-contained {_options.SelfContained.ToString().ToLower()}");

            var exitCode = RunProcess("dotnet", sb.ToString());

            if (exitCode != 0)
            {
                throw new Exception($"Compilation of {_options.ProjectOrSoutionFileName} has been failed.");
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Publish project '{_options.ProjectOrSoutionFileName}' with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Published files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Published files will be put into '{_options.OutputDir}'.");
            }
            sb.AppendLine($"{spaces}With --self-contained {_options.SelfContained.ToString().ToLower()}");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
