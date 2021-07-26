using BaseDevPipeline;
using Deployment.DevTasks.CoreToAsset;
using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevPipelines.CoreToAsset
{
    public class CoreToAssetDevPipeline : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            Exec(new CoreToAssetDevTask());
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            throw new NotImplementedException();
        }
    }
}
