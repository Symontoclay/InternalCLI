using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks.Serialization;
using System.Collections.Generic;

namespace CommonUtils.DeploymentTasks
{
    public interface IDeploymentTask : IObjectToString
    {
        IDeploymentPipelineContext Context { get; }
        void SetContext(IDeploymentPipelineContext context);
        void SetParentTask(IDeploymentTask parentTask);
        bool? IsValid { get; }
        IReadOnlyList<string> ValidationMessages { get; }
        void ValidateOptions();
        void Run();
        uint Deep { get; }
        uint NextDeep { get; }
        string Key { get; }
        DeploymentTaskRunInfo GetChildDeploymentTaskRunInfo(string key);
        bool ContainsChild(string key);
        void AddChildDeploymentTaskRunInfo(DeploymentTaskRunInfo item);
    }
}
