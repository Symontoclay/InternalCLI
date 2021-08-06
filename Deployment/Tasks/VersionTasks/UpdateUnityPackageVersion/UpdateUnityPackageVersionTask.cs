using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.VersionTasks.UpdateUnityPackageVersion
{
    public class UpdateUnityPackageVersionTask : BaseDeploymentTask
    {
        public UpdateUnityPackageVersionTask(UpdateUnityPackageVersionTaskOptions options)
        {
            _options = options;
        }

        private readonly UpdateUnityPackageVersionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.PackageSourcePath), _options.PackageSourcePath);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Version), _options.Version);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var manifestFileName = Path.Combine(_options.PackageSourcePath, "package.json");

            var manifest = UnityPackageManifestModelHelper.Read(manifestFileName);

            manifest.version = _options.Version;

            UnityPackageManifestModelHelper.SaveCompactFile(manifestFileName, manifest);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates version in unity package '{_options.PackageSourcePath}'.");
            sb.AppendLine($"{spaces}The version: {_options.Version}");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
