using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopLevelNewDeploymentTask: NewBaseDeploymentTask
    {
        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options)
            : this(options, null, null)
        {
        }

        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options, INewDeploymentPipeline deploymentPipeline)
            : this(options, deploymentPipeline.Context, null)
        {
        }

        public TopLevelNewDeploymentTask(TopLevelNewDeploymentTaskOptions options, INewDeploymentPipelineContext context, INewDeploymentTask parentTask)
            : base(context, "F32DC066-041B-4786-BCF2-D92E5F5C80F2", true, options, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");

            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), _context, this));
            Exec(new TopSubItemNewDeploymentTask(new TopSubItemNewDeploymentTaskOptions(), _context, this));

            _logger.Info("End");
        }
    }
}
