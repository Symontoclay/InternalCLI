﻿using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.CopyAndPublishVSProjectOrSolution;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToInternalCLIDist
{
    public class CoreToInternalCLIDistDevTask : BaseDeploymentTask
    {
        public CoreToInternalCLIDistDevTask()
            : this(0u)
        {
        }

        public CoreToInternalCLIDistDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var cliProject = ProjectsDataSource.GetProject(KindOfProject.CLI);

            var cliFolderTargetPath = settings.InternalCLIDist;

            Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
            {
                TargetDir = cliFolderTargetPath,
                SkipExistingFilesInTargetDir = false
            }, NextDeep));

            Exec(new CopyAndPublishVSProjectOrSolutionDevTask(new CopyAndPublishVSProjectOrSolutionDevTaskOptions()
            {
                ProjectOrSoutionFileName = cliProject.CsProjPath,
                OutputDir = cliFolderTargetPath,
                NoLogo = true
            }, NextDeep));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Exports CLI to internal CLI destanation.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}