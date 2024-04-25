using CommonUtils.DeploymentTasks.Serialization;
using SymOntoClay.Common;

namespace CommonUtils.DeploymentTasks
{
    public interface IDeploymentPipelineContext : IObjectToString
    {
        DeploymentTaskRunInfo GetDeploymentTaskRunInfo(string key, IDeploymentTask parentTask);
        void SaveDeploymentTaskRunInfo();
        bool IsNewSession { get; }
    }
}
