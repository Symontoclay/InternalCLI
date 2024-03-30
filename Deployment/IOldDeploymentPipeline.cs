using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    [Obsolete("It should be removed", true)]
    public interface IOldDeploymentPipeline : IObjectToString
    {
        void Add(IOldDeploymentTask deploymentTask);
        bool IsValid { get; }
        void Run();
    }
}
