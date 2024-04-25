using SymOntoClay.Common;

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
