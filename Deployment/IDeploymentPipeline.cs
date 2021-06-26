using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public interface IDeploymentPipeline : IObjectToString
    {
        void Add(IDeploymentTask deploymentTask);
        bool IsValid { get; }
        void Run();
    }
}
