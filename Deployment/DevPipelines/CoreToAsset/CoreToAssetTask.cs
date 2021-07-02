using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevPipelines.CoreToAsset
{
    public class CoreToAssetTask : BaseDeploymentTask
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Begin");

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            throw new NotImplementedException();
        }
    }
}
