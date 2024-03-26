using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSandBox.RestoredDeploymentTasks.Serialization;

namespace TestSandBox.RestoredDeploymentTasks
{
    public interface INewDeploymentPipelineContext : IObjectToString
    {
        NewDeploymentTaskRunInfo GetDeploymentTaskRunInfo(string key, INewDeploymentTask parentTask);
        void SaveDeploymentTaskRunInfo();
        bool IsNewSession { get; }
    }
}
