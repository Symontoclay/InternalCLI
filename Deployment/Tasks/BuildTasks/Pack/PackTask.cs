using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildTasks.Pack
{
    public class PackTask : BaseDeploymentTask
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PackTask(PackTaskOptions options)
        {
            _options = options;
        }

        private readonly PackTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName("ProjectFileName", _options.ProjectOrSoutionFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var sb = new StringBuilder($"pack \"{_options.ProjectOrSoutionFileName}\" --configuration {_options.BuildConfiguration}");

            if (!string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.Append($" --output \"{_options.OutputDir}\"");
            }

            if (_options.NoLogo)
            {
                sb.Append(" --nologo");
            }

            if(_options.IncludeSource)
            {
                sb.Append(" --include-source");
            }

            if(_options.IncludeSymbols)
            {
                sb.Append(" --include-symbols");
            }

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

            sb.AppendLine($"{spaces}Packs project '{_options.ProjectOrSoutionFileName}' with {_options.BuildConfiguration} configuration.");
            if (_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if (string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Packed files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Packed files will be put into '{_options.OutputDir}'.");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
