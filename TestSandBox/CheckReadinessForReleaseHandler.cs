using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Deployment.ReleaseTasks.CheckReadinessForRelease;
using NLog;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestSandBox
{
    public class CheckReadinessForReleaseHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            DeploymentPipeline.Run(new CheckReadinessForReleaseTask(new CheckReadinessForReleaseTaskOptions()
            {
                OutputFileName = "ReadinessForReleaseReport.log"
            }));

            _logger.Info("End");
        }
    }
}
