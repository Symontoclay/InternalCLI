using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment.Helpers;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.UnityTasks.ExportPackage;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.DevExportPackage
{
    public class DevExportPackageDevTask : BaseDeploymentTask
    {
        public DevExportPackageDevTask()
            : this(0u)
        {
        }

        public DevExportPackageDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var unitySolution = settings.GetSolution(KindOfProject.Unity);

            var targetUnityVersion = UnityHelper.GetTargetUnityVersion(unitySolution.Path);

            var unityExePath = settings.UtityExeInstances.SingleOrDefault(p => p.Version == targetUnityVersion).Path;

            var futureReleaseInfo = FutureReleaseInfoReader.Read();

            var version = futureReleaseInfo.Version;

            var unityArtifact = settings.GetDevArtifact(KindOfArtifact.UnityPackage);

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = unityArtifact.Path,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            var sourceDir = unitySolution.SourcePath.Replace(unitySolution.Path, string.Empty).Trim();

            if(sourceDir.StartsWith("/"))
            {
                sourceDir = sourceDir.Substring(1);
            }

            Exec(new ExportPackageTask(new ExportPackageTaskOptions()
            {
                UnityExeFilePath = unityExePath,
                RootDir = unitySolution.Path,
                SourceDir = sourceDir,
                OutputPackageName = Path.Combine(unityArtifact.Path, DeployedItemsFactory.GetUnityAssetName(version))
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Exports SymOntoClay's Unity package to dev destanation.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
