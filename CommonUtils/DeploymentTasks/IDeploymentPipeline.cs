using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DeploymentTasks
{
    public interface IDeploymentPipeline : IObjectToString
    {
        IDeploymentPipelineContext Context { get; }
        void Add(IDeploymentTask deploymentTask);
        bool IsValid { get; }
        void Run();
    }
}
