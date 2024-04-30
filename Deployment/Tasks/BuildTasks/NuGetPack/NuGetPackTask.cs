using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.Tasks.BuildTasks.NuGetPack
{
    public class NuGetPackTask : BaseDeploymentTask
    {
        public NuGetPackTask(NuGetPackTaskOptions options)
            : this(options, null)
        {
        }

        public NuGetPackTask(NuGetPackTaskOptions options, IDeploymentTask parentTask)
            : base("67C8143B-C383-4FAB-9177-43AF4375C276", false, options, parentTask)
        {
            _options = options;
        }

        private readonly NuGetPackTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.ProjectOrSoutionFileName), _options.ProjectOrSoutionFileName);
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

            sb.AppendLine($"{spaces}Packs project '{_options.ProjectOrSoutionFileName}' with '{_options.BuildConfiguration}' configuration for runtime '{_options.RuntimeIdentifier}'.");
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

            if (_options.IncludeSource)
            {
                sb.AppendLine($"{spaces}With --include-source");
            }

            if (_options.IncludeSymbols)
            {
                sb.AppendLine($"{spaces}With --include-symbols");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
