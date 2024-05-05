using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Text;

namespace Deployment.DevTasks.InstalledNuGetPackages.UpdateInstalledNuGetPackagesInAllCSharpProjects
{
    public class UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTask : BaseDeploymentTask
    {
        public UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTask(UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions options)
            : this(options, null)
        {
        }

        public UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTask(UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions options, IDeploymentTask parentTask)
            : base("F360031A-E6E3-4310-B4BF-1F03ADF1F8E5", false, options, parentTask)
        {
            _options = options;
        }

        private readonly UpdateInstalledNuGetPackagesInAllCSharpProjectsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
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

            sb.AppendLine($"{spaces}Updates version of installed package '{_options.PackageId}' to '{_options.Version}' for all C# projects in organization.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
