using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class SubItemNewDeploymentTask : NewBaseDeploymentTask
    {
        public SubItemNewDeploymentTask(SubItemNewDeploymentTaskOptions options)
            : this(options, null, null)
        {
        }

        public SubItemNewDeploymentTask(SubItemNewDeploymentTaskOptions options, INewDeploymentPipelineContext context, INewDeploymentTask parentTask)
            : base(context, "AD2EEDF6-7850-4590-98BB-CC4E32791D0B", false, options, parentTask)
        {
            _options = options;
        }

        private readonly SubItemNewDeploymentTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");
            _logger.Info($"_options.DirectoryName = {_options.DirectoryName}");
            _n++;
            _logger.Info($"_n = {_n}");
            //if(_n == 5)
            //{
            //    throw new NotImplementedException();
            //}
            _logger.Info("End");
        }

        private static int _n;
    }
}
