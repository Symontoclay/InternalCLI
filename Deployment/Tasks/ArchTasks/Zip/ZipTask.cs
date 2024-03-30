using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.ArchTasks.Zip
{
    public class ZipTask : OldBaseDeploymentTask
    {
        public ZipTask(ZipTaskOptions options)
            : this(options, 0u)
        {
        }

        public ZipTask(ZipTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly ZipTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
            ValidateFileName(nameof(_options.OutputFilePath), _options.OutputFilePath);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            ZipFile.CreateFromDirectory(_options.SourceDir, _options.OutputFilePath);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Packs content of '{_options.SourceDir}' to .zip arch.");
            sb.AppendLine($"{spaces}Packed files will be put into '{_options.OutputFilePath}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
