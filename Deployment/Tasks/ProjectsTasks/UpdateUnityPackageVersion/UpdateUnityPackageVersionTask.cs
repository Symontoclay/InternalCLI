using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ProjectsTasks.UpdateUnityPackageVersion
{
    public class UpdateUnityPackageVersionTask : OldBaseDeploymentTask
    {
        public UpdateUnityPackageVersionTask(UpdateUnityPackageVersionTaskOptions options)
            : this(options, 0u)
        {
        }

        public UpdateUnityPackageVersionTask(UpdateUnityPackageVersionTaskOptions options, uint deep)
            : base(options, deep)
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

            if(!string.IsNullOrWhiteSpace(_options.UnityVersion))
            {
                manifest.unity = _options.UnityVersion;
            }

            if (!string.IsNullOrWhiteSpace(_options.UnityRelease))
            {
                manifest.unityRelease = _options.UnityRelease;
            }

            UnityPackageManifestModelHelper.SaveCompactFile(manifestFileName, manifest);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates version in unity package '{_options.PackageSourcePath}'.");
            sb.AppendLine($"{spaces}The version: {_options.Version}");
            if (!string.IsNullOrWhiteSpace(_options.UnityVersion))
            {
                sb.AppendLine($"{spaces}The unity version: {_options.UnityVersion}");
            }
            if (!string.IsNullOrWhiteSpace(_options.UnityRelease))
            {
                sb.AppendLine($"{spaces}The unity release: {_options.UnityRelease}");
            }
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
