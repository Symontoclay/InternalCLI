using CommonUtils.DebugHelpers;
using Deployment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public interface INewDeploymentPipeline : IObjectToString
    {
        INewDeploymentPipelineContext Context { get; }
        void Add(INewDeploymentTask deploymentTask);
        bool IsValid { get; }
        void Run();
    }
}
