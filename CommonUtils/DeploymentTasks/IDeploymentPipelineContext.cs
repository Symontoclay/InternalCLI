using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks.Serialization;

namespace CommonUtils.DeploymentTasks
{
    public interface IDeploymentPipelineContext : IObjectToString
    {
        DeploymentTaskRunInfo GetDeploymentTaskRunInfo(string key, IDeploymentTask parentTask);
        void SaveDeploymentTaskRunInfo();
        bool IsNewSession { get; }
    }
}
