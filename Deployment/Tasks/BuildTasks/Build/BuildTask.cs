using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildTasks.Build
{
    public class BuildTask : BaseDeploymentTask
    {
        public BuildTask(BuildTaskOptions options)
        {
            _options = options;
        }

        private readonly BuildTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName("ProjectFileName", _options.ProjectFileName);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Buils project '{_options.ProjectFileName}' with {_options.BuildConfiguration} configuration.");
            if(_options.NoLogo)
            {
                sb.AppendLine($"{spaces}Hides Microsoft logo in ouput.");
            }
            if(string.IsNullOrWhiteSpace(_options.OutputDir))
            {
                sb.AppendLine($"{spaces}Built files will be put into default directory.");
            }
            else
            {
                sb.AppendLine($"{spaces}Built files will be put into '{_options.OutputDir}'.");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
