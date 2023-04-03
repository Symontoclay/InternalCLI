using CommonUtils;
using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.DirectoriesTasks.CopySourceFilesOfProject
{
    public class CopySourceFilesOfVSSolutionTask : BaseDeploymentTask
    {
        public CopySourceFilesOfVSSolutionTask(CopySourceFilesOfVSSolutionTaskOptions options)
            : this(options, 0u)
        {
        }

        public CopySourceFilesOfVSSolutionTask(CopySourceFilesOfVSSolutionTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly CopySourceFilesOfVSSolutionTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateDirectory(nameof(_options.SourceDir), _options.SourceDir);
            ValidateDirectory(nameof(_options.DestDir), _options.DestDir);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            ProcessDir(_options.SourceDir, PathsHelper.Normalize(_options.DestDir), PathsHelper.Normalize(_options.SourceDir));
        }

        private void ProcessDir(string dir, string targetFolder, string slnFolder)
        {
            if (dir.EndsWith(".git"))
            {
                return;
            }

            if (dir.EndsWith(".github"))
            {
                return;
            }

            if (dir.EndsWith(".vs"))
            {
                return;
            }

            if (dir.EndsWith("bin"))
            {
                return;
            }

            if (dir.EndsWith("obj"))
            {
                return;
            }

            var newDirName = PathsHelper.Normalize(dir).Replace(slnFolder, targetFolder);

            if (!Directory.Exists(newDirName))
            {
                Directory.CreateDirectory(newDirName);
            }

            var subDirs = Directory.GetDirectories(dir);

            foreach (var subDir in subDirs)
            {
                ProcessDir(subDir, targetFolder, slnFolder);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                var newFileName = PathsHelper.Normalize(file).Replace(slnFolder, targetFolder);

                File.Copy(file, newFileName, true);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copies all files of solution form directory '{_options.SourceDir}' to {_options.DestDir}.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
