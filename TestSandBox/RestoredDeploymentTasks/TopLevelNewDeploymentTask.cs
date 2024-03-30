﻿using CommonUtils.DeploymentTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopLevelNewDeploymentTask: BaseDeploymentTask
    {
        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options)
            : this(options, null, null)
        {
        }

        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options, IDeploymentPipeline deploymentPipeline)
            : this(options, deploymentPipeline.Context, null)
        {
        }

        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options, IDeploymentPipelineContext context, IDeploymentTask parentTask)
            : base(context, "F32DC066-041B-4786-BCF2-D92E5F5C80F2", true, options, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), "FCD75A10-189C-4F44-815E-0FD6F8AB9A93", _context, this));
            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), "143DEB1D-97E5-44B0-B1FF-5CA85A6A0818", _context, this));

            _logger.Info("End");
        }
    }
}
