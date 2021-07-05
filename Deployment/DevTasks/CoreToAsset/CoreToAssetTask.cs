using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.CoreToAsset
{
    public class CoreToAssetTask //: BaseDeploymentTask
    {
        public CoreToAssetTask(CoreToAssetTaskOptions options)
        {
            _options = options;
        }

        private readonly CoreToAssetTaskOptions _options;


    }
}
